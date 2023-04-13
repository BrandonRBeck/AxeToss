using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	public Button start;
	public Button quit;
	public Button tutorial;
	public Button settings;
	public Button back;

	public Text musicText;
	public Text sfxText;

	protected GameManager gManager;
	private Text startText;

	public Toggle music;
	public Toggle sfx;
	public int menuState;

	public static int STATE_PLAYING = 1;
	public static int STATE_PAUSED = 2;

	public static int MENU_MAIN = 1;
	public static int MENU_SETTINGS = 2;

	void Awake(){
		gManager = GameManager.Instance;
		menuState = 1;
	}

	// Use this for initialization
	void Start () {
		Button startButton = start.GetComponent<Button>();
		startText = startButton.GetComponentInChildren<Text>();

		if(gManager.getScene() == "playScene"){
			startText.text = "Resume";
			startButton.onClick.AddListener(UnPause);
		}else{
			startText.text = "Play";
			startButton.onClick.AddListener(LoadScene);
		}

		Button quitButton = quit.GetComponent<Button>();
		quitButton.onClick.AddListener(QuitGame);

		Button backButton = back.GetComponent<Button>();
		backButton.onClick.AddListener(MenuBack);

		Button settingsButton = settings.GetComponent<Button>();
		settingsButton.onClick.AddListener(MenuSettings);

		Button tutorialButton = tutorial.GetComponent<Button>();
		tutorialButton.onClick.AddListener(LoadTutorial);

		music.isOn = gManager.isMusicOn();
		sfx.isOn = gManager.isSFXOn();

		music.onValueChanged.AddListener(delegate {
			ToggleMusic();
		});
		sfx.onValueChanged.AddListener(delegate {
			ToggleSFX();
		});
		MenuBack();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			QuitGame();
		}
	}

	void LoadScene(){
		gManager.setScene("playScene");
		SceneManager.LoadScene("transitionScene");
	}

	void MenuSettings(){
		menuState = 2;

		settings.gameObject.SetActive(false);
		start.gameObject.SetActive(false);
		quit.gameObject.SetActive(false);

		back.gameObject.SetActive(true);
		tutorial.gameObject.SetActive(true);
		music.gameObject.SetActive(true);
		sfx.gameObject.SetActive(true);
		sfxText.gameObject.SetActive(true);
		musicText.gameObject.SetActive(true);

	}

	void MenuBack(){
		menuState = 1;

		settings.gameObject.SetActive(true);
		start.gameObject.SetActive(true);
		quit.gameObject.SetActive(true);

		back.gameObject.SetActive(false);
		tutorial.gameObject.SetActive(false);
		music.gameObject.SetActive(false);
		sfx.gameObject.SetActive(false);
		sfxText.gameObject.SetActive(false);
		musicText.gameObject.SetActive(false);
	}

	void QuitGame(){
		Application.Quit();
	}

	void LoadTutorial(){
		Time.timeScale = 1;
		gManager.setScene("tutorialScene");
		SceneManager.LoadScene("transitionScene");
	}

	void ToggleMusic(){
		gManager.setMusicOn();
	}
	void ToggleSFX(){
		gManager.setSFXOn();
	}
	void UnPause(){
		gManager.setGameState(STATE_PLAYING);
	}
}
