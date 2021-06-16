using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bonus : MonoBehaviour {
	public float existTime; //物品显示30s
	private bool isShowImage; //为了图片的闪烁
	private float changeImageTime;
	public Sprite[] typeRoom;
	public int type;
	// Use this for initialization

	void Start () {
		existTime = 30f;
		changeImageTime = 0.2f;
		gameObject.GetComponent<SpriteRenderer> ().sprite = typeRoom[type];

	}

	// Update is called once per frame
	void Update () {
		DestoryAndShow ();
	}
	void DestoryAndShow () {
		existTime -= Time.deltaTime;
		if (existTime <= 0) {
			Destroy (gameObject);
			return;

		}
		if (changeImageTime <= 0) {
			changeImageTime = 0.2f;

			if (existTime < 10f) {
				if (isShowImage) {
					//只是隐藏，不会影响到内容，要是用active就不行了
					gameObject.GetComponent<Renderer> ().enabled = true;
					isShowImage = false;
				} else {
					gameObject.GetComponent<Renderer> ().enabled = false;
					isShowImage = true;
				}
			}
		} else {
			changeImageTime -= Time.deltaTime;
		}
	}
	private void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Tank") {
			switch (type) {
				case 0: //加命
					PlayerManager.Instance.lifeValue += 1;
					PlayerManager.Instance.lifeValue2 += 1;
					break;
				case 1: //时间停止
					PlayerManager.Instance.SendMessage ("TimeSetting");
					break;
				case 2: //屋子保护
					PlayerManager.Instance.SendMessage ("createHelpHeart");
					break;
				case 3: //手榴弹
					PlayerManager.Instance.SendMessage("DestoryAllEnemys");
					break;
				case 4: //升级
					PlayerManager.Instance.SendMessage("CanDestoryBarrier");
					break;
				case 5: //无敌
					other.SendMessage("DefenceForBones");
					break;
			}

			ClearIt ();
		}

	}
	public void ClearIt () {
		PlayerManager.Instance.getBonesSound.Play();
		Destroy (this.gameObject);
	}
}