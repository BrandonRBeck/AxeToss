using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour {

	protected GameManager gManager;

	void Awake(){
		gManager = GameManager.Instance;
	}

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene(gManager.getScene());
	}
	
	// Update is called once per frame
	void Update () {

	}
}
