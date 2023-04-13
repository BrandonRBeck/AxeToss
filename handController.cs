using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController : MonoBehaviour {

	// Use this for initialization
	int shift;
	public SceneHandler scene;

	void Start () {
		shift = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(scene.getGameState() != 2 ){
			if(!scene.getAxeCount()){
				if(transform.position.x < -5.79f){
					shift = 1;
				}else if(transform.position.x > -5.73f){
					shift = -1;
				}
				transform.position = new Vector3(transform.position.x + (.00286f * shift), transform.position.y, transform.position.z);
				transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + (.01f * shift), transform.rotation.w);
			}else{
				transform.position = new Vector3(-5.77f, transform.position.y, transform.position.z);
				transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
			}
		}
	}
}
