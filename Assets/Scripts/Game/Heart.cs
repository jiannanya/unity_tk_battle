using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
	private SpriteRenderer sr;
	// Use this for initialization
	public Sprite BrokenSprite;
	public GameObject explosionPrefab;
	public AudioClip dieAudio;
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {

	}

	//Game over
	public void Die () {
		sr.sprite = BrokenSprite;
		AudioSource.PlayClipAtPoint (dieAudio, transform.position);
		Instantiate (explosionPrefab, transform.position, transform.rotation);
		PlayerManager.Instance.isDeafeat1 = true;
		PlayerManager.Instance.isDeafeat2 = true; //双人时有效

	}
	void OnCollisionEnter2D (Collision2D other) {
		this.Die ();
	}

}