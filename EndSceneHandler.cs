using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GoogleMobileAds.Api;

public class EndSceneHandler : MonoBehaviour {

	public GameObject deadBird;
	public float elapsed;
	public Text scoreText;
	public Text highScoreText;
	public Text debugText;
	public Text scorePlaceholder;
	public Text highPlaceholder;
	public GameObject uiPanel;
	int score;
	protected GameManager gManager;
	//private InterstitialAd interstitial;
	private bool hasRequestedAd;

	void Awake(){
		gManager = GameManager.Instance;
	}

	// Use this for initialization
	void Start () {
		score = gManager.getScore();
		int scoreHigh = gManager.getHighScore();
		if(score > scoreHigh){
			scoreHigh = score;
			gManager.setHighScore(score);
		}
	
		scoreText.text = "Score: " + score;
		highScoreText.text = "High: " + scoreHigh;
		for(int i = score > 500 ? 500 : score; i > 0; i--){
			Instantiate(deadBird, new Vector2(UnityEngine.Random.Range(-3.8f,3.8f), UnityEngine.Random.Range(5+i,10+i)), Quaternion.Euler(new Vector3(0,0, UnityEngine.Random.Range(0,360))));
		}
		elapsed = 0f;
		uiPanel.gameObject.SetActive(false);
		hasRequestedAd = false;
		//RequestInterstitial();
		
	}

	void Update () {
		elapsed += Time.deltaTime;
		if (gManager.getTimeSinceLastAd() > 240f)
		{
			if (gManager.isAdLoaded()){
				debugText.text = "Is ad Loaded " + gManager.isAdLoaded();
			}
            else
            {
				debugText.text = gManager.getLoadAdError();
			}
			
			if (gManager.isAdLoaded() && hasInput() && elapsed > 1.5f && !hasRequestedAd) {
				debugText.text = "Ad Loaded: Calling show";
                if (gManager.isMusicOn())
                {
					gManager.musicPause();
                }
				gManager.showAd();
                //this.interstitial.Show();
                hasRequestedAd = true;
            }
            if (hasInput() && elapsed > 5f)
            {
				
				uiPanel.gameObject.SetActive(true);
				gManager.destroyAd();
				//this.interstitial.Destroy();
			}
        }
		else if (hasInput() && elapsed > 1.5f){
			uiPanel.gameObject.SetActive(true);
		}
	}

	bool hasInput(){
		if(Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began)){
			return true;
		}
		if(Input.GetMouseButtonDown(0)){
			return true;
		}
		return false;
	}

	
}
