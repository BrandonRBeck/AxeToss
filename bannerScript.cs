using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bannerScript : MonoBehaviour {

	private AudioSource punch;

	float speed;
	float t;
	bool hasPlayed;
	float wait;
	// Use this for initialization
	void Start () {
		speed = 1.5f;
		t = 0f;
		wait = 0f;
		hasPlayed = false;
		punch = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		wait += Time.deltaTime;
		if(wait > 2){
			transform.position = new Vector3(Mathf.Lerp(16, 0, t), Mathf.Lerp(-6.5f, -3.5f, t), 0);
			t += speed * Time.deltaTime;
			if(t > 1 && !hasPlayed){
				hasPlayed = true;
				punch.Play();
			}
		}
	}
}
