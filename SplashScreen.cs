using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	float time;
	protected GameManager gManager;

	void Awake(){
		gManager = GameManager.Instance;
	}

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time+= Time.deltaTime;
		if(time > 4){
			if(gManager.hasPlayedTutorial()){
				gManager.setScene("menuScene");
			}else{
				gManager.setScene("tutorialScene");
			}
			SceneManager.LoadScene("transitionScene");
		}
	}
}
