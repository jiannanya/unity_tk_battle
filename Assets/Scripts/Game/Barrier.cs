using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

	public AudioClip hitAudio;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void PlayAudio () {
		AudioSource.PlayClipAtPoint (hitAudio, transform.position);
	}
	private void OnCollisionEnter2D (Collision2D other) {

	}
}