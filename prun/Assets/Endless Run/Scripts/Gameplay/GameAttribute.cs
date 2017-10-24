/// <summary>
/// Game attribute.
/// this script use for set all attribute in game(ex speedgame,character life)
/// </summary>

using UnityEngine;
using System.Collections;
using Chronos;
using uSky;

public class GameAttribute : MonoBehaviour {
	public int level = 1;
	public float starterSpeed = 5; //Speed Character
	public float starterLife = 1; //Life character
	public Clock clock;
	//public DistanceCloud dcloud;
	/*[HideInInspector]*/
	public float time = 120.0f;
	public int FinalDistance = 100;
	public float Distance = 0.0f;
	public bool isFinalLoop = false;
	private float remainDistance;
	/*[HideInInspector]*/
	public int fish = 0;
	[HideInInspector]
	public bool isPlaying;
	[HideInInspector]
	public bool pause = false;
	public bool passed = false;
	[HideInInspector]
	public bool ageless = false;
	[HideInInspector]
	public bool deleyDetect = false;
	[HideInInspector]
	public int multiplyValue;
	
	//[HideInInspector]
	public float speed = 5;
	[HideInInspector]
	public float life = 1;
	private float rtime = 0;

	public static GameAttribute Instance;
	
	void Start(){
		//Setup all attribute
		Instance = this;
		//DontDestroyOnLoad(this);
		speed = starterSpeed;
		//time = 120.0f;
		rtime = time;
		//Distance = 0;
		remainDistance = FinalDistance - Distance;
		fish = 0;
		life = starterLife;
		//level = 1;
		pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
		clock = Timekeeper.instance.Clock("enemies");
	}
	
	public float getRemainDistance(){
		remainDistance = FinalDistance - Distance;
		return remainDistance;
	}
	
	public void ActiveShakeCamera(){
		CameraFollow.Instance.ActiveShake();	
	}
	
	public void Pause(bool isPause){
		//pause varible
		pause = isPause;
		clock.paused = isPause;
		if(isPause)
			SoundManager.Instance.pauseSound ();
		else 
			SoundManager.Instance.resumeSound ();
		//if (dcloud != null)
		//	dcloud.CloudMaterial.SetFloat ("Rotate speed", 0.1f);
	}

	public void Passed(bool isPassed) {
		passed = isPassed;
		SoundManager.Instance.StopBGM ();
	}
	
	public void Resume(){
		//resume
		pause = false;
		clock.paused = false;
		SoundManager.Instance.resumeSound ();
	}
	
	public void Reset(){
		//Reset all attribute when character die
		speed = starterSpeed;
		time = rtime;
		Distance = 0;
		remainDistance = FinalDistance - Distance;
		fish = 0;
		life = starterLife;
		//level = 1;
		pause = false;
		passed = false;
		clock.paused = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
		Building.instance.Reset();
		//Item.instance.Reset();
		PatternSystem.instance.Reseted();
		CameraFollow.Instance.Reset();
		Controller.Instance.Reset();
		Controller.Instance.timeJump = 0;
		Controller.Instance.timeMagnet = 0;
		Controller.Instance.timeMultiply = 0;
		Controller.Instance.timeSprint = 0;
		//SoundManager.Instance.StopBGM ();
		//Debug.Log("xxxxxxxxxxxxxxxxxxxxx");
		SoundManager.Instance.resumeSound ();
		//SoundManager.Instance.PlayBGM ("main", true);
		//GUIManager.Instance.Reset();
	}
}
