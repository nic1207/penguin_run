using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static void SaveFish(int f){
		PlayerPrefs.SetInt ("Fish", f);
	}

	public static int LoadFish(){
		return PlayerPrefs.GetInt ("Fish", 0);
	}
}
