using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fader : MonoBehaviour {

	float time;
	SpriteRenderer r;
	// Use this for initialization
	void Start () {
		time = 0;
		r = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if(time < 2){
			r.color = new Color(1f,1f,1f,(1f - (time/2)));
		}else if (time < 4){
			r.color = new Color(1f,1f,1f,(-1f + (time/2)));
		}
	}
}
