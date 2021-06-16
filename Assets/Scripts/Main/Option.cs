using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //场景管理
public class Option : MonoBehaviour {
	private int choice = 1;
	public Transform posOne; //第一个位置Object的引用
	public Transform posTwo;
	public Transform posThree;
	//public AudioClip changeOptionAudio; //切换选项音效的引用
	public AudioSource changeOptionAudio;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (choice < 1) {
			choice = 1;
			return;
		}
		if (choice > 3) {
			choice = 3;
			return;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			choice--;
			changeOptionAudio.Play ();
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			choice++;
			changeOptionAudio.Play ();
		}

		switch (choice) {
			case 1:
				transform.position = posOne.position;
				break;
			case 2:
				transform.position = posTwo.position;
				break;
			case 3:
				transform.position = posThree.position;
				break;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log(choice.ToString());
			switch (choice) {
				case 1:
					SceneManager.LoadScene (1);
					break;
				case 2:
					SceneManager.LoadScene (2);
					break;
				case 3:
					Application.Quit ();//编译成程序之后此功能才会可用
					break;
			}
		}

	}
}