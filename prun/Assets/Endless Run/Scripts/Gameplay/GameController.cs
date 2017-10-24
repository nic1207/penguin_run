/// <summary>
/// Game controller.
/// This script use for control game loading and spawn character when load complete
/// </summary>

using UnityEngine;
using System.Collections;

/*[ExecuteInEditMode]*/
public class GameController : MonoBehaviour {
	
	public PatternSystem patSysm; //pattern system
	public CameraFollow cameraFol;	//camera
	public int selectPlayer;
	public GameObject[] playerPref;
	public GameObject player = null;
	public Controller pcontroller = null;
	public Vector3 posStart;
	public GameObject FinalStationPref = null;
	public GameObject FinalStation = null;
	public GameObject FireWork = null;

	private float percentCount;
	private float lasttime;
	private const int WalkDist = 5;

	public static GameController Instance;
	
	void Awake(){
		if (FinalStationPref != null) {
			if (FinalStation == null) {
				FinalStation = Instantiate (FinalStationPref) as GameObject;
				FinalStation.name = "FinalStation";
				FinalStation.transform.position = new Vector3 (-100, -100, -100);
			}
		}
	}

	void Start(){
		if(Application.isPlaying == true){
			selectPlayer = PlayerPrefs.GetInt("SelectPlayer");
			Instance = this;

			StartCoroutine(WaitLoading());
		}
	}
	
	void OnGUI(){
		if(Application.isPlaying == true){
			if (patSysm.loadingComplete == false) {
				//percentCount = Mathf.Lerp (percentCount, patSysm.loadingPercent, 5 * Time.deltaTime);
				percentCount = patSysm.loadingPercent/100;
				GUIManager.Instance.showLoading (percentCount);
			} else {
				GUIManager.Instance.hideLoading ();
			}
		} else {
			//GUIManager.Instance.showLoading (percentCount);
		}
	}
	
	//Loading method
	IEnumerator WaitLoading(){
		while(patSysm.loadingComplete == false){
			yield return 0;	
		}
		SoundManager.Instance.PlayBGM ("main", true);
		StartCoroutine(InitPlayer());
	}
	
	//Spawn player method
	IEnumerator InitPlayer(){
		player = (GameObject)Instantiate(playerPref[selectPlayer], posStart, Quaternion.identity);
		player.name = "Player";
		pcontroller = player.GetComponent<Controller> ();
		cameraFol.Target = player.transform;
		yield return 0;

		StartCoroutine(UpdatePerDistance());
	}
	
	//update distance score
	IEnumerator UpdatePerDistance(){
		bool paa = false;
		while(true){
			if(PatternSystem.instance!=null && PatternSystem.instance.loadingComplete){
				paa = GameAttribute.Instance.pause || GameAttribute.Instance.passed;
				if(!paa && GameAttribute.Instance.isPlaying == true && GameAttribute.Instance.life > 0){
					if(Controller.Instance && Controller.Instance.transform.position.z > 0){
						if (GameAttribute.Instance.time > 0)
							GameAttribute.Instance.time -= Time.deltaTime;
						else
							GameAttribute.Instance.time = 0;
						GameAttribute.Instance.Distance = player.transform.position.z;
						if (GameAttribute.Instance.getRemainDistance () <= 0) {
							yield return StartCoroutine (PassedGame ());
						} else if (GameAttribute.Instance.getRemainDistance () <= WalkDist+100) {
							changeFloor();
						} else if (GameAttribute.Instance.Distance >= 0 && GameAttribute.Instance.Distance <100) {
							SettingMaterial.needChange ();
							SettingMaterial.ChangeX (3000);
						} else if (GameAttribute.Instance.Distance >= 200 && GameAttribute.Instance.Distance <500) {
							SettingMaterial.needChange ();
							SettingMaterial.ChangeX (0);
						} else if (GameAttribute.Instance.Distance >= 500 && GameAttribute.Instance.Distance <1000) {
							SettingMaterial.needChange ();
							SettingMaterial.ChangeX (-3000);
						}
						if (GameAttribute.Instance.time <=0) {
							yield return StartCoroutine (FailedGame());
						}
						if (GameAttribute.Instance.time <= 10 && GameAttribute.Instance.time > 0) {
							StartCoroutine (alarmTime());
						}
					}
				}
			}
			yield return 0;
		}
	}

	public IEnumerator alarmTime() {
		if (Time.time >= lasttime + 0.5) {
			lasttime = Time.time;
			GUIManager.Instance.AlarmClock ();
			yield return null;
		}
	}

	public void changeFloor() {
		if(GameAttribute.Instance.isFinalLoop)
			return;
		//Debug.Log (PatternSystem.instance.qFloor.Count);
		if(FinalStation!=null)
			FinalStation.transform.position = new Vector3(0, 0, GameAttribute.Instance.FinalDistance +10+ WalkDist);
		GameAttribute.Instance.isFinalLoop = true;
		//PatternSystem.instance.qFloor.Clear ();
		//int i = 0;
		//while (i < PatternSystem.instance.qFloor.Count) {
			//PatternSystem.instance.qFloor [i].floorObj.SetActive (false);
		//	i++;
		//}
	}

	public IEnumerator PassedGame() {
		//Debug.Log ("PassedGame()");
		if (GameAttribute.Instance.passed)
			yield return null; 

		pcontroller.walkToStation ();

		//if(FinalFloor!=null)
		//	FinalFloor.transform.position = new Vector3(0, 0, 50+pcontroller.gameObject.transform.position.z);
		ItemManager.Instance.HideAll ();
		TrapManager.Instance.enableAllTraps(false);
		TrapManager.Instance.setAllVisible (false);
		while (pcontroller.gameObject.transform.position.z < GameAttribute.Instance.FinalDistance+WalkDist) {
			yield return new WaitForSeconds(0.3f);	
		}
		//Debug.Log ("zzzzzzzzzzzzzzzzzzzzz");
		SoundManager.Instance.StopBGM ();
		GameAttribute.Instance.Passed (true);
		pcontroller.WinGame ();
		int oldFishd = GameData.LoadFish ();
		GameData.SaveFish((int)GameAttribute.Instance.fish+oldFishd);

		Animator ani = FinalStation.GetComponentInChildren<Animator> ();
		FinalStation.GetComponentInChildren<SpriteRenderer>().enabled = true;
		ani.SetTrigger ("Up");

		GameObject go1 = Instantiate (FireWork) as GameObject;
		go1.transform.position = new Vector3 (FinalStation.transform.position.x, FinalStation.transform.position.y+5, FinalStation.transform.position.z-2);
		yield return new WaitForSeconds(1.5f);
		GameObject go2 = Instantiate (FireWork) as GameObject;
		go2.transform.position = new Vector3 (FinalStation.transform.position.x+1, FinalStation.transform.position.y+4, FinalStation.transform.position.z-3);
		yield return new WaitForSeconds(1.5f);
		GameObject go3 = Instantiate (FireWork) as GameObject;
		go3.transform.position = new Vector3 (FinalStation.transform.position.x-1, FinalStation.transform.position.y+4, FinalStation.transform.position.z-1);
		yield return new WaitForSeconds(3.0f);
		GUIManager.Instance.ShowPassedMenu ();
		yield return 0;
	}

	public IEnumerator FailedGame() {
		pcontroller.stopFly ();
		SoundManager.Instance.StopBGM ();
		GameAttribute.Instance.Pause(true);
		GUIManager.Instance.ShowFailedMenu();
		yield return 0;
	}
	
	//reset game
	public void ResetGame(){
		//SoundManager.Instance.StopBGM ();
		//int oldFishd = GameData.LoadFish ();
		//GameData.SaveFish((int)GameAttribute.gameAttribute.fish+oldFishd);
		GameAttribute.Instance.Reset ();
		GameAttribute.Instance.isPlaying = true;
		player.transform.position = posStart;
		cameraFol.Target = player.transform;
		//pcontroller.reset ();
		SoundManager.Instance.PlayBGM ("main", true);
	}
}
