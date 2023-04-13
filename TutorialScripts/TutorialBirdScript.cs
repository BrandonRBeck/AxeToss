using UnityEngine;
using System.Collections;

public class TutorialBirdScript : MonoBehaviour
{
	private bool move;
	private bool hit;
	private float speed;
	private SpriteRenderer renderer;
	private Sprite deadBird;
	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();
		deadBird = Resources.Load <Sprite> ("Bird- dead");
		speed = 0.8f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(move && !hit){
			transform.position = new Vector2(transform.position.x - .1f * speed, transform.position.y);
			if(transform.position.x < -15f){
				move = false;
				transform.position = new Vector3(15f, transform.position.y, transform.position.z);
			}
		}
		if(hit){
			transform.position = new Vector2(transform.position.x + .01f, transform.position.y - 0.1f);
			transform.Rotate(new Vector3(0,0,-2));
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		hit = true;
		Destroy(GetComponent<Collider2D>());
		Destroy(GetComponent<Animator>());
		renderer.sprite = deadBird;
	}


	public void setMove(bool move){
		this.move = move;
	}
}

