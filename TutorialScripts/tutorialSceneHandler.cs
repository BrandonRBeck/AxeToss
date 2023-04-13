using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class tutorialSceneHandler : MonoBehaviour{

	public TutorialBirdScript bird;
	public TutorialAxeScript axe;
	public TutorialHandScript hand;
	public Text bannerText;
	protected SpriteRenderer arrowRenderer;
	protected Transform continueText;
	protected bool continueTextGrow;
	protected bool acceptInput;
	protected float timer;
	protected int tutorialPhase;
	protected GameManager gManager;

	protected int tutorialPhaseWelcome = 1;
	protected int tutorialPhaseBird = 2;
	protected int tutorialPhaseAxeSpin = 3;
	protected int tutorialPhaseAngle = 4;
	protected int tutorialPhaseToss = 5;

	void Awake() {
		gManager = GameManager.Instance;
		gManager.setGameState(3);//tutorial
		gManager.setScene("tutorialScene");
	}

	// Use this for initialization
	void Start () {
		arrowRenderer = GameObject.Find("arrow").GetComponent<SpriteRenderer>();
		continueText = GameObject.Find("ContinueText").GetComponent<Transform>();
		timer = 0f;
		continueTextGrow = true;
		arrowRenderer.enabled = false;
		continueText.gameObject.SetActive(false);
		acceptInput = false;
		tutorialPhase = tutorialPhaseWelcome;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(timer > getPhaseDuration()){
			acceptInput = true;
			continueText.gameObject.SetActive(true);
		}

		if(continueTextGrow){
			continueText.localScale = new Vector3(continueText.localScale.x + .005f, continueText.localScale.y + .005f, continueText.localScale.z);
			if(continueText.localScale.x >  1.05f){
				continueTextGrow= false;
			}
		}else{
			continueText.localScale = new Vector3(continueText.localScale.x - .005f, continueText.localScale.y - .005f, continueText.localScale.z);
			if(continueText.localScale.x < .95f){
				continueTextGrow= true;
			}
		}

		if(hasInput() && acceptInput){
			acceptInput = false;
			timer = 0f;
			continueText.gameObject.SetActive(false);
			switch(tutorialPhase){
			case 1:
				bannerText.text = "Birds are attacking you";
				bird.setMove(true);
			break;
			case 2:
				bannerText.text = "Your axe spins in a circle";
				axe.setSpin(true);
				hand.setMove(true);
			break;
			case 3:
				arrowRenderer.enabled = true;
				bannerText.text = "Watch the angle of the axe";
			break;
			case 4:
				bird.setMove(true);
				bannerText.text = "Tap the screen to throw";
			break;
			case 5: 
				axe.setThrowAxe(true);
				arrowRenderer.enabled = false;
				bannerText.text = "Enjoy!";
			break;
			case 6:
				gManager.setScene("playScene");
				gManager.markTutorialComplete();
				gManager.resetTimeSinceLastAd();
				SceneManager.LoadScene("transitionScene");
				break;
			}

			tutorialPhase++;

		}

	}

	public float getPhaseDuration(){
		switch(tutorialPhase){
		case 1:
			return 1;
		case 2:
			return 2;	
		case 3:
			return 1;
		case 4:
			return 1;
		case 5:
			return 1;
		}
		return 3;
	}


	public bool hasInput(){
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
			if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
				return true;
			}
		}else if(Input.GetMouseButtonDown(0)){
			if(!EventSystem.current.IsPointerOverGameObject()){
				return true;
			}
		}
		return false;
	}
}
