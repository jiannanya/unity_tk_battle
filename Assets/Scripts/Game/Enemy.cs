using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float moveSpeed = 3;
	public Sprite[] TankSprite; //shang you xia zuo
	public GameObject bullectPrefab;
	public GameObject explosionPrefag;
	// Use this for initialization
	private Vector3 bullectEularAngles;

	private float v = -1; ///上下
	private float h; //左右
	//计时器
	private float timeVal;
	private float timeValChangeDirection;

	public bool isBones; //是否为带有奖励的敌人
	public bool timeSetting;
	void Start () {
		timeSetting = true;
	}

	// Update is called once per frame
	void Update () {

		//攻击的时间间隔
		if (timeVal >= 3) {
			if (timeSetting) {
				Attack ();
			}
		} else {
			timeVal += Time.deltaTime;
		}

	}
	private void FixedUpdate () {
		//坦克的移动方法
		if (timeSetting) {
			Move ();
		}

		//	Attack();
	}
	//坦克的攻击方法
	private void Attack () {

		//transform.EularAngles+bullectEularAngles
		Instantiate (bullectPrefab, transform.position, Quaternion.Euler (bullectEularAngles));
		timeVal = 0;

	}
	private void Move () {

		if (timeValChangeDirection >= 4) {
			int num = Random.Range (0, 8);
			if (num > 5) {
				v = -1;
				h = 0;

			} else if (num == 0) {
				v = 1;
				h = 0;

			} else if (num > 0 && num <= 2) {
				v = 0;
				h = -1;

			} else if (num > 2 && num <= 4) {
				v = 0;
				h = 1;

			}
			timeValChangeDirection = 0;
		} else {
			timeValChangeDirection += Time.fixedDeltaTime;
		}
		moveMOVE (v, h);
	}
	private void moveMOVE (float v, float h) {
		transform.Translate (Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
		if (v < 0) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = TankSprite[2];
			bullectEularAngles = new Vector3 (0, 0, 180);
		} else if (v > 0) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = TankSprite[0];
			bullectEularAngles = new Vector3 (0, 0, 0);
		}

		if (v != 0) {
			return;
		}

		transform.Translate (Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World); //x 轴 .up //y .forward //z
		if (h < 0) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = TankSprite[3];
			bullectEularAngles = new Vector3 (0, 0, 90);
		} else if (h > 0) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = TankSprite[1];
			bullectEularAngles = new Vector3 (0, 0, -90);
		}
	}
	//坦克的死亡方法
	private void DieFrom1 () {
		PlayerManager.Instance.playerScore++; //给P1加分
		//产生爆炸特效
		Instantiate (explosionPrefag, transform.position, transform.rotation);

		if (isBones) { //如果是带有奖励的坦克的话！
			PlayerManager.Instance.SendMessage ("CreateBones");
		}
		//死亡  
		PlayerManager.Instance.enemys.Remove (this.gameObject);
		Destroy (gameObject);

	}
	private void Die () { //如果是手榴弹，则给两个玩家都加同样的分数
		PlayerManager.Instance.playerScore++; //给P1加分
		PlayerManager.Instance.player2Score++; //给P2加分
		//产生爆炸特效
		Instantiate (explosionPrefag, transform.position, transform.rotation);

		if (isBones) { //如果是带有奖励的坦克的话！
			PlayerManager.Instance.SendMessage ("CreateBones");
		}
		//死亡  
		PlayerManager.Instance.enemys.Remove (gameObject); //在这里会移除enemys中的这个GAmeobject
		Destroy (gameObject);
	}

	private void DieFrom2 () {
		PlayerManager.Instance.player2Score++; //给P2加分
		//产生爆炸特效
		Instantiate (explosionPrefag, transform.position, transform.rotation);
		if (isBones) { //如果是带有奖励的坦克的话！
			PlayerManager.Instance.SendMessage ("CreateBones");
		}
		//死亡  
		PlayerManager.Instance.enemys.Remove (this.gameObject);
		Destroy (gameObject);
	}

	private void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Barrier" || other.gameObject.tag == "River"||other.gameObject.tag == "Wall"||other.gameObject.tag == "AirBarrier") {
			timeValChangeDirection = 4;
		}

	}
}