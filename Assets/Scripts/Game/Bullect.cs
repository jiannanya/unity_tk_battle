using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour {
	public float moveSpeed = 10;
	// Use this for initialization
	public bool isPlayerBullect;
	public bool isPlayer1Bullect;
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.Translate (transform.up * moveSpeed * Time.deltaTime, Space.World);
	}
	private void OnTriggerEnter2D (Collider2D collision) {
		switch (collision.tag) {
			case "Tank":
				if (!isPlayerBullect) {
					collision.SendMessage ("Die");
					Destroy (gameObject);
				}

				break;
			case "Heart":
				collision.SendMessage ("Die");
				Destroy (gameObject);
				break;
			case "Enemy":
				if (isPlayerBullect) {
					if (isPlayer1Bullect) {
						collision.SendMessage ("DieFrom1");
					} else {
						collision.SendMessage ("DieFrom2");
					}

					Destroy (gameObject);
				}

				break;
			case "Wall":
				Destroy (collision.gameObject);
				Destroy (gameObject);
				break;
			case "Barrier":
				if (isPlayerBullect) {
					collision.SendMessage ("PlayAudio");
					if (PlayerManager.Instance.canDestoryBarrier){
						Destroy(collision.gameObject);
					}
				}
				Destroy (gameObject);
				break;
			case "AirBarrier":
				Destroy (gameObject);
				break;
			default:
				break;
		}

	}
}