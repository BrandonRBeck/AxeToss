using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHandler : MonoBehaviour {

	public SceneHandler scene;
	public Transform[] children;
	float speed;
	float childSpeed;
	float speedModifier;
	int childDirection;
	float flipCounter;

	// Use this for initialization
	void Start () {
		speed = .1f;
		childSpeed = .0015f;
		childDirection = 1;
		speedModifier = 1;
		flipCounter = 0;
		scene = FindObjectOfType<SceneHandler>();
		children = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(scene.getGameState() != 2){
			if(transform.position.x > 15){
				Destroy(gameObject);
			}
			transform.position = new Vector2(transform.position.x + .1f * speed, transform.position.y);

			flipCounter += Time.deltaTime;
			if(flipCounter > 1){
				flipCounter =  0;
				childDirection *= -1;
			}
			int i = 0;
			foreach(Transform child in children){
				i++;
				if(i % 2 == 0){
					child.position = new Vector3(child.position.x, child.position.y + (childSpeed * childDirection), child.position.z);
				}else{
					if(childDirection == -1){
						speedModifier = .33f;
					}
					child.position = new Vector3(child.position.x + (childSpeed * childDirection  * speedModifier), child.position.y, child.position.z);
				}
			}

		}
	}
}
