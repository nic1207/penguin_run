using UnityEngine;
using System.Collections;

public class TrapManager : MonoBehaviour {

    public static TrapManager Instance;

    public GameObject[] trapItems;
	public GameObject[] seajs;

    // Use this for initialization
    void Start () {
		Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void enableAllTraps(bool enable) {
		//Debug.Log ("enableAllTraps("+enable+")");
		if (trapItems == null||trapItems.Length<=0)
        {
            trapItems = GameObject.FindGameObjectsWithTag("TrapItems");
        }

        for (int i = 0; i < trapItems.Length; i++) {
            trapItems[i].SetActive(enable);
        }

    }

	public void setAllVisible(bool enable) {
		if (seajs == null||seajs.Length<=0)
		{
			seajs = GameObject.FindGameObjectsWithTag("Enemy");
		}

		for (int i = 0; i < seajs.Length; i++) {
			seajs[i].SetActive(enable);
		}
	}

}
