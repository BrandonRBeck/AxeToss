using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class AxeHandler : MonoBehaviour {

	public float speed;
	public GameObject targetObject;
	public Transform target;
	public bool rotateAround;
	private Rigidbody2D rigidBody;
	public float force;
	public float resetTimer;
	public bool resetCount;
	public SceneHandler scene;
	public bool wasThrown;
	public int collisionCounter;
	public List<string> colList;

	private Vector3 zAxis = new Vector3(0, 0, -1);

	void Awake(){
		scene = FindObjectOfType<SceneHandler>();
	}
	// Use this for initialization
	void Start () {
		collisionCounter = 0;
		targetObject = GameObject.Find("rotatePoint");
		target = targetObject.GetComponent<Transform>();
		rigidBody = GetComponent<Rigidbody2D>();
		resetTimer = 0;
		resetCount = false;
		rotateAround = true;
		rigidBody.gravityScale = 0;
		rigidBody.velocity = new Vector2(0,0);
		rigidBody.freezeRotation = true;
		rigidBody.inertia = 0;
		rigidBody.isKinematic = true;
		speed = 480;
		wasThrown = false;
		force = 800;
		colList = new List<string>();
		colList.Add("Axe2(Clone)");
		ignoreAxeCollisions(GameObject.FindGameObjectsWithTag("Axe"));
	}
	
	// Update is called once per frame
	void Update() {
		if(scene.getGameState() != 2){

			if(resetCount){
				resetTimer += Time.deltaTime;
			}

			if(resetTimer > 2f){
				resetCount = false;
				resetTimer = 0;
			}

			if(transform.position.y < -10){
				Destroy(gameObject);
			}
			

			if(rotateAround){
				transform.RotateAround(target.position, zAxis, speed * Time.deltaTime); 
			}else{
				transform.Rotate(new Vector3(0,0,-1200 * Time.deltaTime));
			}

			if (hasInput() && rotateAround && !wasThrown){
				wasThrown = true;
				scene.spawnAxe();
				rigidBody.isKinematic = false;
			
				resetCount = true;
				rotateAround = false;

				float angle = transform.rotation.eulerAngles.z - 30;

				float x =  Mathf.Sin(angle * Mathf.Deg2Rad);
				float y =  Mathf.Cos(angle * Mathf.Deg2Rad);

				rigidBody.AddForce(new Vector2(x * force * -1,y * force));
				rigidBody.gravityScale = 1;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		if(!rotateAround){
			resetCount = true;
		}
		if(!colList.Contains(coll.collider.gameObject.name)){
			colList.Add(coll.collider.gameObject.name);
			collisionCounter++;
			if(collisionCounter >= 2 && this.transform.localScale.x <= .2f && !scene.isMegaAxe()){
				scene.setMegaAxe();
			}
		}
	}
	bool hasInput(){
		if(scene.getGameState() == 1){
			if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
				if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
					return true;
				}
			}else if(Input.GetMouseButtonDown(0)){
				if(!EventSystem.current.IsPointerOverGameObject()){
					return true;
				}
			}
		}
		return false;
	}

	void ignoreAxeCollisions(GameObject[] otherAxes){
		foreach(Collider2D coll in this.GetComponents<Collider2D>()){
			foreach(GameObject obj in otherAxes){
				Collider2D[] colliders = obj.GetComponents<Collider2D>();
				foreach(Collider2D coll2 in colliders){
					Physics2D.IgnoreCollision(coll, coll2);
				}
			}
		}
	}
}
