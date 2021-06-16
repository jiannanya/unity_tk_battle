using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour {
	public GameObject[] playerPrefabList;
	public GameObject[] enemyPrefabList;
	public bool isPlayer1;

	public bool createPlayer;

	// Use this for initialization
	void Start () {

		//延时调用
		Invoke ("BornTank", 1f);
		Destroy (gameObject, 1f);

	}

	// Update is called once per frame
	void Update () {

	}
	private void BornTank () {
		//	Instantiate (playerPrefabList[1], transform.position, Quaternion.identity);
		if (createPlayer) {
			if (isPlayer1) {
				Instantiate (playerPrefabList[0], transform.position, Quaternion.identity);

			} else {
				Instantiate (playerPrefabList[1], transform.position, Quaternion.identity);
			}
		} else {
			int num = Random.Range (0, 100);
			GameObject go = null;
			if (num < 40) { //40%
				go = Instantiate (enemyPrefabList[0], transform.position, Quaternion.identity);
			} else if (num < 80) { //40%
				go = Instantiate (enemyPrefabList[1], transform.position, Quaternion.identity);
			} else { //20%
				go = Instantiate (enemyPrefabList[2], transform.position, Quaternion.identity);
				go.GetComponent<Enemy> ().isBones = true;
			}
			PlayerManager.Instance.enemys.Add (go);
		}
	}
}