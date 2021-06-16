using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {
	public float endPosition; //游戏开始的UI向上浮动
	public float timeVal; //动态的坦克效果
	private bool isMoveNow;
	public Sprite[] pictureStore;
	public Image tankChoice; //UI下物体的引用
	string maxScore; //最高分，从文件中取得
	public Text ScoreTitle;
	void Start () {
		string[] strs = File.ReadAllLines ("Data/Score.dat");
		maxScore = strs[0]; // for (int i = 0; i < strs.Length; i++){
		ScoreTitle.text = "HIGH SCORES:" + maxScore;
	}
	// Update is called once per frame
	void Update () {
		this.endPosition = transform.localPosition.y; //获取本地坐标系里的数据，而不是世界坐标系
		if (endPosition < 0f) {
			endPosition += 0.1f;
			transform.Translate (Vector3.up * 1 * 300 * Time.deltaTime, Space.World);
		}
		if (timeVal <= 0.02f) {
			timeVal += Time.deltaTime;
		} else { //每0.02秒换一张图片，相当于播放动画（canvas不知道怎么加动画实体，因为canvas好像会覆盖他）
			timeVal = 0;
			if (isMoveNow) {
				isMoveNow = false;
			} else {
				isMoveNow = true;
			}
		}
		Image i = tankChoice.GetComponent<Image> ();
		if (isMoveNow) {
			i.overrideSprite = pictureStore[0]; //修改image的方法，需要赋给他精灵
		} else {
			i.overrideSprite = pictureStore[1];
		}
	}
}