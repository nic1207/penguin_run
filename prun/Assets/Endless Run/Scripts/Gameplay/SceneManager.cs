using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public List<string> Levels = new List<string> ();
	public GameObject LoadingCanvas = null;
	public Image loadingBarImage = null;

	private static AsyncOperation operation;
	private float currentProgress = 0f;
	public float delayAfter = 0.5f;
	private bool finishing = false;
	private static SceneManager _instance;

	public static SceneManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (SceneManager) FindObjectOfType(typeof(SceneManager));

				if (_instance == null)
				{
					Debug.LogError("SceneManager Instance Error");
				}
			}
			return _instance;
		}
	}

	void Awake (){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("SceneManager") as GameObject[];
		if(obj!=null && obj.Length > 1 ){
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (operation == null) return;
		if (finishing) return;

		if (operation.progress >= 0.9f) {
			currentProgress = 1.0f;
			Invoke("ActivateScene", delayAfter);
			finishing = true;
		}
	}

	public void Play(int index) {
		if (index >= Levels.Count) {
			Debug.Log (string.Format("index={0} >= Levels.Count={1}", index, Levels.Count));
			return;
		}
		string levelname = Levels[index];
		//Application.LoadLevelAsync(levelname);
		StartCoroutine(LoadScene(levelname));
		if (LoadingCanvas != null)
			LoadingCanvas.SetActive (true);
	}

	private IEnumerator LoadScene(string levelname) {
		// Start load scene.
		operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelname);
		// Mark that scene should not be activated by itself. We will handle it.
		operation.allowSceneActivation = false;

		// Update current progress for GUI display.
		currentProgress = operation.progress;

		yield return operation;
	}

	private void ActivateScene() {
		operation.allowSceneActivation = true;
		finishing = false;
		StartCoroutine (hideLoadCanvas());
	}

	private IEnumerator hideLoadCanvas() {
		yield return new WaitForSeconds (1);
		if (LoadingCanvas != null)
			LoadingCanvas.SetActive (false);
	}
	private void OnGUI() {
		if (operation == null) return;

		//Rect position = new Rect(10f, 10f, Screen.width - 20f, 100f);
		//Debug.Log ("Progress: "+ currentProgress*100+"%");
		if (loadingBarImage != null)
			loadingBarImage.fillAmount = currentProgress;
		//GUI.Label(position, "Progress: " + currentProgress*100+"%");
	}
}
