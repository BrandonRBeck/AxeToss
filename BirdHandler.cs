using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHandler : MonoBehaviour {

	private bool hit;
	public SceneHandler scene;
	private bool hasUpdatedScore;
	private float speed;
	private AudioSource whack;
	private AudioSource squawk;
	private float aspectRatio;
	private Sprite deadBird;
	private SpriteRenderer renderer;
	// Use this for initialization
	void Start () {
		aspectRatio = Screen.width * 1f / Screen.height * 1f;
		hit = false;
		renderer = GetComponent<SpriteRenderer>();
		scene = FindObjectOfType<SceneHandler>();
		hasUpdatedScore = false;
		float maxSpeed = .5f;
		if(scene.isBirdLimited()){
			maxSpeed = .3f;
			scene.setLimitedBird(false);
		}else{
			if(transform.position.y > 1){
				maxSpeed = .45f;
			}
			if(transform.position.y > 2){
				maxSpeed = .4f;
			}
			if(transform.position.y > 3){
				maxSpeed = .35f;
			}
		}
		float speedRandom = Random.Range(.2f, maxSpeed);
		if(speedRandom > .45f){
			scene.setLimitedBird(true);
		}
		speed = speedRandom * aspectRatio;
	
		whack = GetComponents<AudioSource>()[0];
		squawk = GetComponents<AudioSource>()[1];
		getSquawk();
		deadBird = Resources.Load <Sprite> ("Bird- dead");
	}
	
	// Update is called once per frame
	void Update () {
		if(scene.getGameState() != 2){
			if(transform.position.y < -15){
				Destroy(gameObject);
			}
			if(transform.position.x < -15){
				if(hit){
					Destroy(gameObject);
				}else{
					if(scene.getGameState() == 1){
						scene.gameOver();
					}else{
						Destroy(gameObject);
					}					
				}
			}

			if(hit){
				transform.position = new Vector2(transform.position.x + (Time.deltaTime * 0.6f), transform.position.y - (Time.deltaTime * 6f));
				transform.Rotate(new Vector3(0,0,-2));
			}else{
				transform.position = new Vector2(transform.position.x - (Time.deltaTime * 6f) * speed, transform.position.y);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		hit = true;
		if(scene.isSFXOn()){
			whack.Play();
			squawk.Play();
		}
		if(!hasUpdatedScore){
			scene.incrementScore();
			hasUpdatedScore = true;
		}
		Destroy(GetComponent<Collider2D>());
		Destroy(GetComponent<Animator>());
		renderer.sprite = deadBird;
	}

	void getSquawk(){
		AudioClip squawkClip;
		int squawksound = Random.Range(1,6);

		switch(squawksound){
		case 1:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird");
			break;
		case 2:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird2");
			break;
		case 3:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird3");
			break;
		case 4:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird4");
			break;
		case 5:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird5");
			break;
		default:
			squawkClip = Resources.Load<AudioClip>("Sounds/bird4");
			break;
		}
		squawk.clip = squawkClip;
	}
}