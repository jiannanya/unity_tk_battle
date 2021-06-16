using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float moveSpeed = 3; //坦克的移动速度
	public GameObject bullectPrefab; //子弹引用
	public GameObject explosionPrefag; //爆炸引用
	public GameObject defendEffectPrefag; //无敌效果引用
	private Vector3 bullectEularAngles; //子弹的朝向
	private float timeVal; //攻击限速
	private float defendTimeVal = 3; //3s无敌时间
	public bool isDefended = true; //是否处于防卫状态
	public bool isDefendedForBones;

	public AudioSource moveAudio; //本体的audio引用
	public AudioClip[] tankAudio; //储存一些使用的音效
	private Animator anim; //动画控制器
	private bool isHasMove = false; //是否已经走了

	private GameObject bonesDefenceFinal; //全局变量存储数据
	void Awake () {
		anim = gameObject.GetComponent<Animator> (); //设置自身的动画控制器
	}

	void Update () {
		//保护!是否处于无敌状态	
		if (isDefended) {
			defendEffectPrefag.SetActive (true);
			defendTimeVal -= Time.deltaTime;
			if (defendTimeVal <= 0) {
				isDefended = false;
				defendEffectPrefag.SetActive (false);
			}
		}
	}
	private void FixedUpdate () {
		//坦克的移动方法
		if (PlayerManager.Instance.isDeafeatAll) { //死了就不让走了
			return;
		}
		Move ();
		//攻击CD
		if (timeVal >= 0.4f) { //每0.4s才能攻击一次
			Attack ();
		} else {
			timeVal += Time.fixedDeltaTime;
		}
	}

	private void Attack () { //坦克的攻击方法
		if (Input.GetKeyDown (KeyCode.RightControl)) {
			GameObject go = Instantiate (bullectPrefab, transform.position, Quaternion.Euler (bullectEularAngles));
			go.GetComponent<Bullect> ().isPlayer1Bullect = true;
			timeVal = 0;
		}
	}
	private void moveFactory (int type) {
		if (type == 1) { //up
			transform.Translate (Vector3.up * 1 * moveSpeed * Time.fixedDeltaTime, Space.World);
			bullectEularAngles = new Vector3 (0, 0, 0);
			transform.rotation = Quaternion.Euler (bullectEularAngles);
		} else if (type == 2) { //right
			transform.Translate (Vector3.right * 1 * moveSpeed * Time.fixedDeltaTime, Space.World);
			bullectEularAngles = new Vector3 (0, 0, -90);
			transform.rotation = Quaternion.Euler (bullectEularAngles);
		} else if (type == 3) { //down
			transform.Translate (Vector3.up * -1 * moveSpeed * Time.fixedDeltaTime, Space.World);
			bullectEularAngles = new Vector3 (0, 0, 180);
			transform.rotation = Quaternion.Euler (bullectEularAngles);
		} else if (type == 4) { //left
			transform.Translate (Vector3.right * -1 * moveSpeed * Time.fixedDeltaTime, Space.World);
			bullectEularAngles = new Vector3 (0, 0, 90);
			transform.rotation = Quaternion.Euler (bullectEularAngles);
		}
	}
	private void Move () {
		//GET键盘信息
		float v = Input.GetAxisRaw ("Vertical");
		float h = Input.GetAxisRaw ("Horizontal");
		//动画切换 //音效
		if (Mathf.Abs (v) > 0.07 || Mathf.Abs (h) > 0.07) {
			if (!isHasMove) { //移动且第一次移动才切换动画，切换之后unity会自动稳定到过度后的动画
				this.anim.SetTrigger ("Player1AdleToMove");
			}
			moveAudio.clip = tankAudio[1];
			if (!moveAudio.isPlaying) {
				moveAudio.Play ();
			}
			isHasMove = true;
		} else {
			if (isHasMove) {
				this.anim.SetTrigger ("Player1MoveToAdle");
			}
			moveAudio.clip = tankAudio[0];
			if (!moveAudio.isPlaying) {
				moveAudio.Play ();
			}
			isHasMove = false;
		}
		//移动，还是不会栈，只能牺牲一部分按键
		if (v > 0) {
			moveFactory (1);
		} else if (v < 0) {
			moveFactory (3);
		}
		if (v != 0) {
			return;
		}
		if (h > 0) {
			moveFactory (2);
		} else if (h < 0) {
			moveFactory (4);
		}
	}
	//坦克的死亡方法
	private void Die () {
		if (isDefended||isDefendedForBones) {//如果处于复活或者bones的防护状态的话，死亡方法无效
			return;
		}
		PlayerManager.Instance.isDied1 = true;
		//产生爆炸特效
		Instantiate (explosionPrefag, transform.position, transform.rotation);
		//死亡  
		Destroy (gameObject);
	}
	public void DefenceForBones () {
		defendEffectPrefag.SetActive (true);
		isDefendedForBones = true;
		Invoke ("DestoryDenfenceForBones", 20f);
	}
	public void DestoryDenfenceForBones () {
		defendEffectPrefag.SetActive (false);
		isDefendedForBones=false;
	}
}