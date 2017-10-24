/// <summary>
/// GUI manager
/// this script use to control all GUI in game
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {
	public GameObject GUI = null;
	public Text TotalTime = null;
	public Text Distance = null;
	public Text Fish = null;
	public GameObject btnPause = null;
	public GameObject btnExit = null;
	public GameObject btnResume = null;
	public GameObject topPanel = null;
	public GameObject pausedPanel = null;
	public GameObject passedPanel = null;
	public GameObject loadingPanel = null;
	public Image loadingPercent = null;
	public Text pTime = null;
	public Text pDist = null;
	public Text pFish = null;
	public Image pClock = null;
	//public Button pOk = null;
	public List<Image> Stars = new List<Image>();
	public List<GameObject> StarEffs = new List<GameObject>();

	public GameObject failedPanel = null;
	public GameController gameController = null;


	private int hh;
	private int mm;
	private int ss;
	private int _fish = 0;
	//private int _dist = 0;
	private int _time = 0;
	private int _rank = 0;
	private int _level = 0;

	private static GUIManager _instance;

	public static GUIManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (GUIManager) FindObjectOfType(typeof(GUIManager));

				if (_instance == null)
				{
					Debug.LogError("GUIManager Instance Error");
				}
			}
			return _instance;
		}
	}

	void Awake(){
		//loadingPanel.SetActive (true);
	}
	
	void Start(){
		//DontDestroyOnLoad (gameObject);
		//instance = this;
		init();

	}
	
	void Update(){
		if (GameAttribute.Instance == null)
			return;
		//Set to preview GUI in editor
		if(Application.isPlaying == false){
			if (GUI != null && GUI.gameObject.activeSelf)
				GUI.gameObject.SetActive (false);
		}else{
			if(PatternSystem.instance && PatternSystem.instance.loadingComplete == true){
				if (GUI != null && !GUI.gameObject.activeSelf)
					GUI.gameObject.SetActive (true);
				if(topPanel!=null && !topPanel.activeSelf 
					&& (!GameAttribute.Instance.pause||!GameAttribute.Instance.passed) )
					topPanel.SetActive (true);
				setTime(GameAttribute.Instance.time);
				setDist(GameAttribute.Instance.getRemainDistance());
				setFish(GameAttribute.Instance.fish);
			} else {
				topPanel.SetActive (false);
				//if (GUI != null && GUI.gameObject.active)
				//	GUI.gameObject.SetActive (false);
				//showLoading ();
			}
		}

	}

	public string convertTime(int t) {
		string res = string.Empty;
		if (t>=0) {
			hh = ((int)(t / 3600));
			mm = ((int)(t / 60));
			ss = ((int)(t % 60));
			//TotalTime.text = string.Format("{0:00}:{1:00}:{2:00}", hh, mm, ss);
			res = string.Format("{0:00}:{1:00}:{2:00}", hh, mm, ss);
		}
		return res;
	}

	public void setTime(float t) {
		if (TotalTime)
			TotalTime.text = convertTime ((int)t);
	}

	public void setDist(float t) {
		if(Distance)
			Distance.text = ((int)t).ToString ();
	}

	public void setFish(int t) {
		if(Fish)
			Fish.text = t.ToString ();
	}

	public void ShowPausedMenu() {
		SoundManager.Instance.PlaySE ("pause", false);
		GameAttribute.Instance.Pause(true);
		//pausedPanel.SetActive (false);
		//LeanTween.moveLocalX (pausedPanel, -1000, 0);
		pausedPanel.SetActive (true);
		//LeanTween.move (pausedPanel, new Vector3 (0, 0, 0), 5f).setEase(LeanTweenType.pingPong);
		//LeanTween.alpha(pausedPanel, 0.5f, 3.0f);
		//LeanTween.moveLocalX (pausedPanel, 0, .5f);
		//btnExit.SetActive(true);
		//btnResume.SetActive(true);
	}

	public void HidePausedMenu() {
		SoundManager.Instance.PlaySE ("button", false);
		GameAttribute.Instance.Resume ();
		//pausedPanel.SetActive (true);
		//LeanTween.moveLocalX (pausedPanel, -0, 0);
		//LeanTween.moveLocalX (pausedPanel, -1000, .5f);
		pausedPanel.SetActive(false);
	}

	public void ShowPassedMenu() {
		//GameAttribute.gameAttribute.Pause(true);
		//SoundManager.Instance.PlaySE ("button", false);
		GameAttribute.Instance.Passed(true);
		//LeanTween.scale(passedPanel, new Vector3(0, 0, 0), 0);
		topPanel.SetActive (false);
		pausedPanel.SetActive (false);
		passedPanel.SetActive (true);
		//LeanTween.scale (passedPanel, new Vector3 (1, 1, 1), 1.0f);
		//LeanTween.scale(passedPanel, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutBounce);
		//string fish = string.Format("{0}", GameAttribute.gameAttribute.fish);
		//string dist = string.Format ("{0:0}", GameAttribute.gameAttribute.distance);
		//pFish.text = fish;
		//pTime.text = convertTime(GameAttribute.gameAttribute.time);
		//pDist.text = dist+"m";
		//btnExit.SetActive(true);
		//btnResume.SetActive(true);
		_fish = GameAttribute.Instance.fish;
		//_dist = (int)GameAttribute.Instance.Distance;
		_time = (int)GameAttribute.Instance.time;
		_level = (int)GameAttribute.Instance.level;
		//int score = 0;
		//score += _time * 10;
		//score += _fish * 2;
		if(_time >= 30) {
			_rank = 3;
		} else if(_time >=15 && _time <30) {
			_rank = 2;
		} else {
			_rank = 1;
		}
		//_fish += _time*5;
		int locked = LevelData.getLevelUnlock ();
		if (_level > locked) {
			LevelData.setLevelUnlock (_level);
		}
		if (_rank >= 1) {
			//unlock next level
			if(_level + 1 > locked)
				LevelData.setLevelUnlock (_level + 1);
		}
		int star = LevelData.getLevelStar (_level);
		if (_rank > star) {
			LevelData.setLevelStar (_level, _rank);
		}

		StartCoroutine(AnimationScore());
	}

	private IEnumerator AnimationScore() {
		yield return new WaitForSeconds (1);
		yield return StartCoroutine (RankScore());
		StartCoroutine(animationFish ());
		//StartCoroutine(animationDist ());
		StartCoroutine(animationTime ());

		//pOk.enabled = true;
		//LeanTween.scale (pOk.gameObject, new Vector3 (.9f, .9f, .9f), 1.0f).setLoopPingPong ();
	}

	private IEnumerator RankScore() {
		int r = 0;
		while (r < _rank) {
			Stars [r].color = new Color32 (247, 138, 38, 255);
			StarEffs [r].SetActive (true);
			SoundManager.Instance.PlaySE ("score", false);
			//Stars [r].color = Color.magenta;
			//yield return null;
			yield return new WaitForSeconds(1);
			r++;
		}
	}

	private IEnumerator animationFish() {
		int f = _fish;
		int af = (_fish + (_time * 5)) / 20;
		SoundManager.Instance.PlaySE ("scoreloop", true);
		while (f <= _fish+(_time*5)) {
			pFish.text = string.Format("{0}",f);
			//yield return null;
			//SoundManager.Instance.PlaySE ("scoreloop", true);
			//yield return null;
			yield return new WaitForSeconds (0.1f);
			f += af;
		}
		SoundManager.Instance.StopSE ();
	}

	/*
	private IEnumerator animationDist() {
		int d = 0;
		while (d <= _dist) {
			pDist.text = string.Format("{0}",d);
			//SoundManager.Instance.PlaySE ("score", false);
			//yield return new WaitForSeconds (0.1f);
			yield return null;
			//SoundManager.Instance.PlaySE ("score", false);
			//yield return new WaitForSeconds (0.3f);
			d += 10;
		}
	}
	*/

	private IEnumerator animationTime() {
		int t = _time;
		while (t >= 0) {
			pTime.text = convertTime(t);
			//yield return new WaitForSeconds (0.1f);
			//yield return null;
			//SoundManager.Instance.PlaySE ("score", false);
			yield return null;
			//yield return new WaitForSeconds (0.1f);
			t--;
		}
	}

	public void HidePassedMenu() {
		SoundManager.Instance.PlaySE ("button", false);
		GameAttribute.Instance.Pause(false);
		topPanel.SetActive (true);
		pausedPanel.SetActive (false);
		passedPanel.SetActive (false);
		//btnExit.SetActive(false);
		//btnResume.SetActive(false);
	}

	public void showLoading(float t) {
		loadingPanel.SetActive (true);
		//string p = string.Format ("{0:0.0}", t);
		//Debug.Log(t);
		if (loadingPercent)
			loadingPercent.fillAmount = t;
	}

	public void hideLoading() {
		loadingPanel.SetActive (false);
	}

	public void ShowFailedMenu() {
		SoundManager.Instance.PlaySE ("failed", false);
		GameAttribute.Instance.Pause(true);
		topPanel.SetActive (false);
		pausedPanel.SetActive (false);
		failedPanel.SetActive (true);
		//btnExit.SetActive(true);
		//btnResume.SetActive(true);
	}

	public void HideFailedMenu() {
		SoundManager.Instance.PlaySE ("button", false);
		GameAttribute.Instance.Pause(false);
		failedPanel.SetActive (false);
		//btnExit.SetActive(false);
		//btnResume.SetActive(false);
	}

	public void goExit() {
		SoundManager.Instance.StopSE ();
		SoundManager.Instance.PlaySE ("button", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
	}

	public void goCurrent() {
		SoundManager.Instance.StopSE ();
		SoundManager.Instance.PlaySE ("button", false);
		int lv = GameAttribute.Instance.level;
		if (lv <= GlobalConst.MaxLevel) {
			Debug.Log ("goCurrent("+ (lv) +")");
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Level0" +lv);
		}
	}
	public void goNext() {
		SoundManager.Instance.StopSE ();
		SoundManager.Instance.PlaySE ("button", false);
		int lv = GameAttribute.Instance.level;
		if (lv + 1 <= GlobalConst.MaxLevel) {
			Debug.Log ("goNext("+ (lv+1) +")");
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Level0" + (int)(lv + 1));
		}
	}

	public void goCharacterSelect() {
		SoundManager.Instance.StopSE ();
		SoundManager.Instance.PlaySE ("button", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("SelCharacter");
	}

	public void goStageSelect() {
		SoundManager.Instance.StopSE ();
		SoundManager.Instance.PlaySE ("button", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("StageSel");
	}

	private void init() {
		if (TotalTime != null)
			TotalTime.text = "00:00:00";
		if (Distance != null)
			Distance.text = "0";
		if (Fish != null)
			Fish.text = "0";
		if (pTime != null)
			pTime.text = "00:00:00";
		if (pDist != null)
			pDist.text = "0";
		if (pFish != null)
			pFish.text = "0";


		topPanel.SetActive (true);
		pausedPanel.SetActive (false);
		//LeanTween.moveLocalX (pausedPanel, -1000, 0);
		passedPanel.SetActive (false);
		//LeanTween.scale(passedPanel, new Vector3(0, 0, 0), 0);
		failedPanel.SetActive (false);
		//if (pOk != null)
		//	pOk.enabled = false;
	}

	public void Reset() {
		SoundManager.Instance.PlaySE ("button", false);
		//AudioListener.pause = false;
		init ();

		if(gameController!=null) {
			//GameAttribute.gameAttribute.Reset ();
			gameController.ResetGame();
		}

		//if (btnExit != null)
		//	btnExit.SetActive(false);
		//if (btnResume != null)
		//	btnResume.SetActive(false);
	}

	public void AlarmClock() {
		if(pClock!=null)
			pClock.enabled = !pClock.enabled;
		if (TotalTime != null)
			TotalTime.enabled = !TotalTime.enabled;
		SoundManager.Instance.PlaySE ("alarm", false);
		//gameObject.SetActive (false);
	}
}

