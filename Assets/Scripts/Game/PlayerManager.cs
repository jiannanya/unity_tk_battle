using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用UI的命名空间 
using System.IO;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour {
	//属性值
	public int lifeValue = 3;
	public int playerScore = 0;
	public int lifeValue2 = 3;
	public int player2Score = 0;
	public bool isDied1 = false;
	public bool isDied2 = false;
	public bool isDeafeat1;
	public bool isDeafeat2;
	public bool isDeafeatAll;
	public GameObject born;
	public Text playerScoreText;
	public Text playLifeValueText;
	public Text player2ScoreTest;
	public Text playlifeValue2Text;
	public GameObject isDefeatUI; //显示GAME OVER UI 的引用
	public int type;

	public float timeSettingVal;
	public GameObject bones; //引用bones预制体

	public List<GameObject> enemys; //储存着所有敌人的引用

	public AudioSource getBonesSound;
	public List<GameObject> helpBlocks;
	public GameObject helpingBarrier; //引用保护心脏的预制体
	public bool canDestoryBarrier;//能否摧毁白色块
	//单例
	private static PlayerManager instance;
	public static PlayerManager Instance {
		get {
			return instance;
		}
		set {
			instance = value;
		}
	}
	void Start () {

	}
	private void Awake () {
		Instance = this; //创建单例模式！！！！！！！！！
	}

	// Update is called once per frame
	void Update () {

		if ((this.type == 1 && this.isDeafeat1) || (this.type == 2 && this.isDeafeat1 && this.isDeafeat2)) {
			isDeafeatAll = true;
			isDefeatUI.SetActive (true);
			Invoke ("ReturnToTheMainMenu", 3);
			return;
		}
		if (isDied1) { //玩家一死了，恢复玩家1
			Recover (1);
		}
		if (isDied2) { //玩家二死了，恢复玩家2
			Recover (2);
		}
		//记分榜显示
		playerScoreText.text = playerScore.ToString ();
		playLifeValueText.text = lifeValue.ToString ();
		if (this.type == 2) { //防止Game1场景空指针异常
			player2ScoreTest.text = player2Score.ToString ();
			playlifeValue2Text.text = lifeValue2.ToString ();
		}
	}
	private void Recover (int ina) {
		switch (ina) {
			case 1:

				if (lifeValue < 1) {

					isDeafeat1 = true;
				} else {
					lifeValue--;
					Debug.Log (lifeValue.ToString ());
					if (!(lifeValue == 0)) { //防止再生成一次坦克
						GameObject go = Instantiate (born, new Vector3 (-2, -8, 0), Quaternion.identity);
						go.GetComponent<Born> ().createPlayer = true;
						go.GetComponent<Born> ().isPlayer1 = true;
						isDied1 = false;
					}
				}
				break;
			case 2:
				if (lifeValue2 < 1) {
					isDeafeat2 = true;
				} else {
					lifeValue2--;
					if (!(lifeValue2 == 0)) {
						GameObject go = Instantiate (born, new Vector3 (2, -8, 0), Quaternion.identity);
						go.GetComponent<Born> ().createPlayer = true;
						go.GetComponent<Born> ().isPlayer1 = false;
						isDied2 = false;
					}
				}
				break;
		}
	}

	void FixedUpdate () {

		if (timeSettingVal <= 0) {
			BackTime ();
		} else {
			timeSettingVal -= Time.fixedDeltaTime;
		}

	}
	private void ReturnToTheMainMenu () {
		//保存最高分数据
		int bigScore = 0;
		if (playerScore > player2Score) {
			bigScore = playerScore;
		} else {
			bigScore = player2Score;
		}
		string[] strs = File.ReadAllLines ("Data/Score.dat");
		int temp = int.Parse (strs[0]);
		if (bigScore > temp) {

		} else {
			bigScore = temp;
		}
		File.WriteAllText ("Data/Score.dat", bigScore.ToString ());
		//返回首页
		SceneManager.LoadScene (0);
	}

	void CreateBones () {
		int type = Random.Range (0, 6);
		Vector3 createPostion = new Vector3 (Random.Range (-10.00f, 10.00f), Random.Range (-8.00f, 8.00f), 0);
		GameObject bonesItem = Instantiate (bones, createPostion, Quaternion.identity);
		bonesItem.GetComponent<Bonus> ().type = type;
		bonesItem.transform.SetParent (gameObject.transform);
	}
	void TimeSetting () { //时间静止开始，上方fixupdate执行
		timeSettingVal = 15f;
		for (int i = 0; i < enemys.Count; i++) {
			GameObject go = enemys[i];
			go.GetComponent<Enemy> ().timeSetting = false;
		}
	}
	void BackTime () { //恢复
		for (int i = 0; i < enemys.Count; i++) {
			GameObject go = enemys[i];
			go.GetComponent<Enemy> ().timeSetting = true;
		}
	}
	void createHelpHeart () {

		GameObject go1 = Instantiate (helpingBarrier, new Vector3 (-1, -8, 0), Quaternion.identity);
		GameObject go2 = Instantiate (helpingBarrier, new Vector3 (-1, -7, 0), Quaternion.identity);
		GameObject go3 = Instantiate (helpingBarrier, new Vector3 (0, -7, 0), Quaternion.identity);
		GameObject go4 = Instantiate (helpingBarrier, new Vector3 (1, -7, 0), Quaternion.identity);
		GameObject go5 = Instantiate (helpingBarrier, new Vector3 (1, -8, 0), Quaternion.identity);
		helpBlocks.Add (go1);
		helpBlocks.Add (go2);
		helpBlocks.Add (go3);
		helpBlocks.Add (go4);
		helpBlocks.Add (go5);
		Invoke ("destoryHelpingHeart", 30f); //30s之后销毁
	}
	void destoryHelpingHeart () {
		for (int i = 0; i < helpBlocks.Count; i++) {
			Destroy (helpBlocks[i]);
		}
		helpBlocks.Clear ();
	}
	void DestoryAllEnemys () {
		for (int i = 0; i < enemys.Count; i++) {
			enemys[i].SendMessage ("Die");
		}
		for (int i = 0; i < enemys.Count; i++) {//清理得更彻底
			enemys[i].SendMessage ("Die");
		}
		//不能clear，因为在相同的帧下如果在clear之前创建新的enemy的话，就不会被记录在enemys列表里的
	}
	void CanDestoryBarrier(){
		canDestoryBarrier=true;
		Invoke("CancelToDestroyBarrier",20f);
	}
	void CancelToDestroyBarrier(){
		canDestoryBarrier=false;
	}
}