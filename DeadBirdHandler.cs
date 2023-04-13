using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBirdHandler : MonoBehaviour {

	AudioSource splat;
	Rigidbody2D rb;
	bool hasPlayed;
	private float yVel;
	private GameManager gManager;
	// Use this for initialization

	void Awake(){
		gManager = GameManager.Instance;
	}

	void Start () {
		splat = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody2D>();
		hasPlayed = false;
		yVel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -2 && yVel == 0){
			yVel = rb.velocity.y;
		}
	}
	void OnCollisionEnter2D (Collision2D coll){
		if(gManager.isSFXOn()){
			if(coll.collider.gameObject.name.Equals("platform")){
				if(!hasPlayed && yVel < -5f){
					hasPlayed = true;
					splat.Play();
				}
			}
		}
	}
}
