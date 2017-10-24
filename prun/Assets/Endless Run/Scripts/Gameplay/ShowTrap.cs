using UnityEngine;
using System.Collections.Generic;

public class ShowTrap : MonoBehaviour {

    public GameObject trapSeal;

    //private const int RANDOM_RATE = 4;          // 海豹出現機率 1: 100%, 4: 25%

    //private static System.Random rnd = new System.Random();

    public void randomTrap() {
        if (trapSeal != null) {
            trapSeal.SetActive(RandomVariable.getRandomSeal() == 0);
        }
    }

	// Use this for initialization
	void Start () {
        randomTrap();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
