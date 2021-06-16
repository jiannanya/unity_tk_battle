using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour {
	//用来装饰初始化的地图，所需的数组
	//0.老家 1.墙 2.障碍 3.出生效果 4.	河流 5.草 6.空气墙
	public GameObject[] item;
	// Use this for initialization

	//已经放置东西的列表
	private List<Vector3> itemPositionList = new List<Vector3> ();
	public int gameType;
	private void Awake () {
		InitMap ();
		if (gameType == 1) {
			PlayerManager.Instance.type = 1;
		} else if (gameType == 2) {
			PlayerManager.Instance.type = 2;

		}
	}

	private void InitMap () {

		//实例化老家
		//实例化元素，坐标，无旋转
		CreateItem (item[0], new Vector3 (0, -8, 0), Quaternion.identity);
		//用墙把老家围起来
		CreateItem (item[1], new Vector3 (-1, -8, 0), Quaternion.identity);
		CreateItem (item[1], new Vector3 (1, -8, 0), Quaternion.identity);
		for (int i = -1; i < 2; i++) {
			CreateItem (item[1], new Vector3 (i, -7, 0), Quaternion.identity);
		}
		//初始化玩家
		GameObject go = Instantiate (item[3], new Vector3 (-2, -8, 0), Quaternion.identity);
		go.GetComponent<Born> ().createPlayer = true; //得到born的script
		go.GetComponent<Born> ().isPlayer1 = true;
		itemPositionList.Add (new Vector3 (-2, -8, 0));
		if (gameType == 2) {
			GameObject go2 = Instantiate (item[3], new Vector3 (2, -8, 0), Quaternion.identity);
			go2.GetComponent<Born> ().createPlayer = true;
			go2.GetComponent<Born> ().isPlayer1 = false;
			itemPositionList.Add (new Vector3 (2, -8, 0));
		}
		//初始化敌人
		CreateItem (item[3], new Vector3 (-10, 8, 0), Quaternion.identity);
		CreateItem (item[3], new Vector3 (0, 8, 0), Quaternion.identity);
		CreateItem (item[3], new Vector3 (10, 8, 0), Quaternion.identity);

		//实例化外围空气墙
		for (int i = -11; i <= 11; i++) {
			CreateItem (item[6], new Vector3 (i, 9, 0), Quaternion.identity);

		}
		for (int i = -11; i <= 11; i++) {
			CreateItem (item[6], new Vector3 (i, -9, 0), Quaternion.identity);

		}
		for (int i = -8; i <= 8; i++) {
			CreateItem (item[6], new Vector3 (-11, i, 0), Quaternion.identity);
		}

		for (int i = -8; i <= 8; i++) {
			CreateItem (item[6], new Vector3 (11, i, 0), Quaternion.identity);
		}
		//示例化地图

		for (int i = 0; i < 60; i++) {
			CreateItem (item[1], createRandomPosition (), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++) {
			CreateItem (item[2], createRandomPosition (), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++) {
			CreateItem (item[4], createRandomPosition (), Quaternion.identity);
		}
		for (int i = 0; i < 20; i++) {
			CreateItem (item[5], createRandomPosition (), Quaternion.identity);
		}

		InvokeRepeating ("CreateEnemy", 4, 5); //第一次产生在几秒，以后每几秒产生一次!!!!!!!!!!!!!!!!!!!!!!!!!!!1

	}

	//产生敌人的方法

	private void CreateEnemy () {
		int num = Random.Range (0, 3);
		Vector3 EnemyPos = new Vector3 ();
		if (num == 0) {
			EnemyPos = new Vector3 (-10, 8, 0);
		} else if (num == 1) {
			EnemyPos = new Vector3 (0, 8, 0);
		} else {
			EnemyPos = new Vector3 (10, 8, 0);

		}
		CreateItem (item[3], EnemyPos, Quaternion.identity);

	}
	private void CreateItem (GameObject createGameObject, Vector3 createPostion, Quaternion createRotation) {
		GameObject itemGo = Instantiate (createGameObject, createPostion, createRotation);
		itemGo.transform.SetParent (gameObject.transform);
		itemPositionList.Add (createPostion);
	}
	//产生随机位置的方法
	private Vector3 createRandomPosition () {
		while (true) {
			Vector3 createPostion = new Vector3 (Random.Range (-10, 11), Random.Range (-8, 9), 0);
			if (!HasThePosition (createPostion)) {
				return createPostion;
			}
		}
	}
	//用来判断位置列表中，是否有这个位置
	private bool HasThePosition (Vector3 createPostion) {
		for (int i = 0; i < itemPositionList.Count; i++) {
			if (createPostion == itemPositionList[i]) {
				return true;
			}
		}
		return false;
	}

}