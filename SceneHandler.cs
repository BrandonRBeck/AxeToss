using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour {

	public Button pause;
	private float birdTime;
	private float axeTime;
	private bool axeCount;
	public GameObject bird;
	public GameObject axe;
	public GameObject cloud1;
	public GameObject cloud2;
	public GameObject cloud3;
	public SpriteRenderer menuBird;
	public SpriteRenderer menuViking;
	public SpriteRenderer menuHand;
	public int birdStrikes;
	public Text scoreText;
	public Text megaText;
	private bool megaAxe;
	private float bonusAxeCounter;
	private int cloneCount;
	protected GameManager gManager;
	protected int gameState;
	private float gameTime;
	public GameObject uiPanel;
	private bool megaAxeAnimation;
	private float megaAxeAnimationTime;
//	private float aspectRatio;
	private float screenRightEdge;
	private bool animateScore;
	private float scoreAnimateTime;
	private float cloudCounter;
	private int cloudThreshold;
	private bool limitNextBird;

	public static int STATE_PLAYING = 1;
	public static int STATE_PAUSED = 2;

	void Awake(){
		gManager = GameManager.Instance;
		gameState = STATE_PLAYING;
		gManager.setGameState(gameState);
		gManager.setScene("playScene");
	}

	// Use this for initialization
	void Start () {
		birdTime = 0;
		gameTime = 0;
		birdStrikes = 0;
		updateScore();
		bonusAxeCounter = 10;
		megaAxe = false;
		axeTime = 2f;
		cloneCount = 0;
		Button pauseButton = pause.GetComponent<Button>();
		pauseButton.onClick.AddListener(PauseGame);
		gManager.setGameTime(0);
		gManager.setSpawnCount(0);
		uiPanel.gameObject.SetActive(false);
		megaText.gameObject.SetActive(false);
//		aspectRatio = Screen.width * 1f / Screen.height * 1f;
		Vector3 ScreenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		screenRightEdge = ScreenSize.x;
		animateScore = false;
		scoreAnimateTime = 0;
		cloudThreshold = 10;
		menuBird.enabled = false;
		menuViking.enabled = false;
		menuHand.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState == STATE_PLAYING){
			gameTime += Time.deltaTime;
			birdTime += Time.deltaTime;
			if(axeCount){
				axeTime += Time.deltaTime;
			}

			cloudCounter += Time.deltaTime;
			if(cloudCounter > cloudThreshold){
				cloudCounter = 0;
				cloudThreshold = Random.Range(10,20);
				GameObject cloud;
				switch(Random.Range(1,4)){
				case 1:
					cloud = cloud1;
				break;
				case 2:
					cloud = cloud2;
				break;
				case 3:
					cloud = cloud3;
				break;
					default:
					cloud = cloud1;
					break;
				}

				GameObject cloudClone = (GameObject) Instantiate(cloud, new Vector2(-screenRightEdge - 5, Random.Range(1.1f,4.9f)), transform.rotation);
			}

			if(animateScore){
				scoreAnimateTime -= Time.deltaTime;
				if(scoreAnimateTime > .2f){
					scoreText.fontSize = scoreText.fontSize + 1;
				}else if (scoreAnimateTime > 0){
					scoreText.fontSize = scoreText.fontSize - 1;
				}else{
					scoreText.fontSize = 50;
					animateScore = false;
				}
			}

			if(megaAxe){
				if(!megaAxeAnimation){
					megaAxeAnimationTime = 4f;
					gameState = STATE_PAUSED;
					gManager.setGameState(STATE_PAUSED);
					Time.timeScale = 0;
				}

				bonusAxeCounter -= Time.deltaTime;
				if(bonusAxeCounter <= 0){
					megaAxe = false;
					megaAxeAnimation = false;
					bonusAxeCounter = 10;
				}
			}

			if(axeTime > 1.2f){
				axeTime = 0;
				axeCount = false;
				GameObject axeClone = (GameObject) Instantiate(axe, new Vector2(-5.77f, -2.36f), new Quaternion(0,0,0,0));
				if(megaAxe){
					axeClone.transform.localScale += new Vector3(.2f,.2f,.2f);
					axeClone.transform.position = new Vector2(axeClone.transform.position.x, axeClone.transform.position.y +.5f);
				}
			}

			if(birdTime > 3){
				GameObject birdClone = (GameObject) Instantiate(bird, new Vector2(screenRightEdge + 1, Random.Range(-2.1f,4.1f)), transform.rotation);
				cloneCount++;
				birdClone.name = "birdClone" + cloneCount;
				birdTime = 0;
			}
		}else{
			if(gManager.getGameState() == STATE_PLAYING){
				PauseGame();
			}

			if(megaAxe && !megaAxeAnimation){
				pause.gameObject.SetActive(false);
				animateMegaAxe();
			}


		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			gManager.setScene("menuScene");
			SceneManager.LoadScene("transitionScene");
		}
	}

	public void incrementScore(){
//		checkLength();
		birdStrikes++;
		scoreAnimateTime = .4f;
		updateScore();
	}

	void updateScore(){
		gManager.setScore(birdStrikes);
		scoreText.text = birdStrikes.ToString();
		animateScore = true;
	}
//
//	void checkLength(){
//		string strikesString = birdStrikes.ToString();
//		string incStrikeString = (birdStrikes + 1).ToString();
//		if(incStrikeString.Length > strikesString.Length){
//			positionScore();
//		}
//	}
//
//	void positionScore(){
//		scoreText.transform.localPosition = new Vector3(scoreText.transform.localPosition.x - 8f, scoreText.transform.localPosition.y, scoreText.transform.localPosition.z);
//	}

	public void spawnAxe(){
		axeCount = true;
	}

	public bool getAxeCount(){
		return axeCount;
	}

	public void gameOver(){
		gManager.setGameTime(gameTime);
		gManager.setSpawnCount(cloneCount);
		gManager.setScene("endScene");
		SceneManager.LoadScene("transitionScene");
	}

	public void setMegaAxe(){
		megaAxe= true;
	}
	public void PauseGame(){
		if(gameState == STATE_PLAYING){
			uiPanel.gameObject.SetActive(true);
			pause.gameObject.SetActive(false);
			gameState = STATE_PAUSED;
			gManager.setGameState(STATE_PAUSED);
			Time.timeScale = 0;
			menuBird.enabled = true;
			menuViking.enabled = true;
			menuHand.enabled = true;
		}else{
			uiPanel.gameObject.SetActive(false);
			pause.gameObject.SetActive(true);
			gameState = STATE_PLAYING;
			Time.timeScale = 1;
			menuBird.enabled = false;
			menuViking.enabled = false;
			menuHand.enabled = false;
		}
	}

	public int getGameState(){
		return gameState;
	}

	public bool isSFXOn(){
		return gManager.isSFXOn();
	}

	public void animateMegaAxe(){
		megaAxeAnimationTime -= .033f;
		if(!megaText.gameObject.activeSelf){
			megaText.gameObject.SetActive(true);
		}
		if(megaAxeAnimationTime > 2.5f){
			megaText.fontSize = megaText.fontSize + 2;
		}else if (megaAxeAnimationTime > 2){
			megaText.fontSize = megaText.fontSize - 1;
		}else if (megaAxeAnimationTime > 1.5f){
			megaText.fontSize = megaText.fontSize + 2;
		}else if (megaAxeAnimationTime > 0.1f){
			megaText.fontSize = megaText.fontSize - 3;
		}else{
			pause.gameObject.SetActive(true);
			megaText.fontSize = 35;
			megaText.gameObject.SetActive(false);
			megaAxeAnimation = true;
			gameState = STATE_PLAYING;
			gManager.setGameState(STATE_PLAYING);
			Time.timeScale = 1;
		}
	}

	public bool isMegaAxe(){
		return megaAxe;
	}

	public void setLimitedBird(bool limit){
		limitNextBird = limit;
	}

	public bool isBirdLimited(){
		return limitNextBird;
	}

//	public float getAspectRatio(){
//		return aspectRatio;
//	}
}
