using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishPool : MonoBehaviour {
	public List<GameObject> fish_perfab;

    public static int POOL_SIZE = 10;
	public static FishPool Instance;

	private GameObject[,] Fishes;
	private const string LOCK_FISHPOOL = "lock_fishpool";
	private Vector3 posStart = new Vector3(-100, -100, -100);
	private int[] fishIdx;

	// Use this for initialization
	void Start () {
		Instance = this;
		fishIdx = new int[fish_perfab.Count];
		//Debug.Log ("fish_perfab.Count = " + fish_perfab.Count);
		Fishes = new GameObject[fish_perfab.Count, POOL_SIZE];
		for (int i = 0; i < fishIdx.Length; i++) {
			fishIdx [i] = 0;
		}
		generateFishPool ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void generateFishPool() {
		//GameObject fishObj = new GameObject();
		//fishObj.name = "Fishes";
		for (int j = 0; j < fish_perfab.Count; j++) {
			int idx = 0;
			while (idx < POOL_SIZE) {
				GameObject go = Instantiate (fish_perfab[j], posStart, Quaternion.identity) as GameObject;
				go.name = "Fish[" + j + "][" + idx + "]";
				go.transform.parent = this.transform;
				//Fishes.Add (go);
				//Debug.Log("j = " + j + ", idx = " + idx);
				Fishes[j, idx++] = go;
				//yield return 0;
			}
		}
	}

	private GameObject getFish(int colorIdx) {
		GameObject fish = null;
		lock(LOCK_FISHPOOL) {
			fish = Fishes[colorIdx, fishIdx[colorIdx]++];
			if (fishIdx[colorIdx] >= POOL_SIZE) {
				fishIdx[colorIdx] = 0;
			}
		}

		return fish;
	}

	public GameObject getFish() {
		int rndVal;

		for (int i = fishIdx.Length - 1; i >= 0; i--) {
			rndVal = RandomVariable.getRandomValue (0, i * 3);
			if (rndVal == 0) {
				return getFish(i);
			}
		}

		return getFish (0);
	}
}
