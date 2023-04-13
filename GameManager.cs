using UnityEngine;
using System.Collections;
using System;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;

	private string currentScene;
	public int score;
	private float gameTime;
	private int spawnCount;
	private string result;
	private bool musicOn;
	private bool sfxOn;
	AudioSource music;
	private int gameState;
	private float timeSinceLastAd;
	private bool hasTutorial;
	private InterstitialAd interstitial;
	private string failedToLoadError;

	public static int STATE_PLAYING = 1;
	public static int STATE_PAUSED = 2;
	public static int STATE_TUTORIAL = 3;

	private bool hasRequestedAd;

	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType(typeof (GameManager)) as GameManager;
			}

			if (instance == null) {
				GameObject obj = new GameObject("GameManager");
				instance = obj.AddComponent(typeof (GameManager)) as GameManager;
				Debug.Log("Could not locate a GameManager object, one was generated automatically.");
			}

			return instance;
		}
	}

	void Awake() {
		DontDestroyOnLoad(this);
		currentScene = "menuScene";
		//Time.fixedDeltaTime = 0.0167f;
		Application.targetFrameRate = 60;
		Application.runInBackground = false;
		music = gameObject.AddComponent<AudioSource>();
		music.loop= true;
		music.volume =.3f;
		music.clip = Resources.Load<AudioClip>("Sounds/song");
		musicOn = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
		sfxOn = PlayerPrefs.GetInt("sfx", 1) == 1 ? true : false;
		gameState = STATE_PLAYING;
		hasTutorial = PlayerPrefs.GetInt("tutorial", 0) == 1 ? true : false;
		MobileAds.Initialize(initStatus => { });
		timeSinceLastAd = 200f;
		hasRequestedAd = false;
	}

	public float getTimeSinceLastAd()
    {
		return timeSinceLastAd;
    }
	public void resetTimeSinceLastAd()
    {
		timeSinceLastAd = 0;
    }


    // Use this for initialization
    void Start () {
		gameTime = 0;
		spawnCount = 0;
		if(musicOn){
			music.Play();
		}
	}

	// Update is called once per frame
	void Update () {
		timeSinceLastAd += Time.deltaTime;

        if (timeSinceLastAd > 180f && !hasRequestedAd)
        {
			RequestInterstitial();
			hasRequestedAd = true;
		}
	}

	void OnApplicationQuit() {
		instance = null;
	}
	public string getScene(){
		return currentScene;	
	}
	public void setScene(string newScene){
		currentScene = newScene;
	}
	public void setScore(int score){
		this.score = score;
	}
	public int getScore(){
		return score;
	}

	public int getHighScore(){
		return PlayerPrefs.GetInt("score", 0);
	}

	public void setHighScore(int score){
		PlayerPrefs.SetInt("score", score);
		PlayerPrefs.Save();
	}
	public void setGameTime(float gameTime){
		this.gameTime = gameTime;
	}
	public void setSpawnCount(int spawnCount){
		this.spawnCount = spawnCount;
	}

	public void musicPause()
    {
		music.Pause();
    }

	public void musicResume()
	{
		if (!music.isPlaying && isMusicOn()) { 
			music.Play();
		}
    }
	public bool isMusicOn(){
		return musicOn;
	}
	public void setMusicOn(){
		musicOn = !musicOn;
		if(!musicOn){
			music.Stop();
		}else{
			music.Play();
		}
		PlayerPrefs.SetInt("music", musicOn ? 1 : 0);
		PlayerPrefs.Save();
	}

	public bool isSFXOn(){
		return sfxOn;
	}

	public void setSFXOn(){
		sfxOn = !sfxOn;
		PlayerPrefs.SetInt("sfx", sfxOn ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void setGameState(int state){
		gameState = state;
	}
	public int getGameState(){
		return gameState;
	}
	public bool hasPlayedTutorial(){
		return hasTutorial;
	}
	public void markTutorialComplete(){
		PlayerPrefs.SetInt("tutorial", 1);
	}

	public bool isAdLoaded()
    {
		return interstitial.IsLoaded();
    }

	public void showAd()
    {
		interstitial.Show();
    }

	public void destroyAd()
    {
		this.interstitial.Destroy();
		hasRequestedAd = false;
	}

	public string getLoadAdError()
    {
		return failedToLoadError;
    }

	private void RequestInterstitial()
	{
		//string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Ads
        //string adUnitId = "ca-app-pub-7860142491657514/7012159278"; //Android ads
		string adUnitId = "ca-app-pub-7860142491657514/2112455307"; //iOS ads
		this.interstitial = new InterstitialAd(adUnitId);
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		AdRequest request = new AdRequest.Builder().Build();
		this.interstitial.LoadAd(request);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		resetTimeSinceLastAd();
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		failedToLoadError = args.LoadAdError.ToString() + " : " + args.LoadAdError.GetMessage() +  " : " + args.ToString();
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		destroyAd();
		musicResume();
	}

}