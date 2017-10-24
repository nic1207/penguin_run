using UnityEngine;
using System.Collections;

public class LevelData : MonoBehaviour {

	public static void setLevelStar(int i, int rank){
		PlayerPrefs.SetInt ("LevelStar_"+i, rank);
	}

	public static int getLevelStar(int i){
		return PlayerPrefs.GetInt ("LevelStar_"+i);
	}

	public static void setLevelUnlock(int i){
		PlayerPrefs.SetInt ("LevelLock", i);
	}

	public static int getLevelUnlock(){
		return PlayerPrefs.GetInt ("LevelLock");
	}

	public static void clearAll(){
		PlayerPrefs.DeleteAll ();
	}
}
