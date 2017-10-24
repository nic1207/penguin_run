using UnityEngine;
using System.Collections.Generic;

public class RandomVariable : MonoBehaviour {

    private static System.Random rnd = new System.Random();

    public static int FISH_RATE = 4;        // 魚產生的機率, 4表示1/4, 1表示1/1
    public static int SEAL_RATE = 8;      // 海豹出現機率 1: 100%, 4: 25%
    public static int TRAP_RATE = 16;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static int getRandomValue() {
        return rnd.Next();
    }

    public static int getRandomValue(int min, int max)
    {
        return rnd.Next(min, max);
    }

    public static int getRandomFish() {
        return rnd.Next(0, FISH_RATE - 1);
    }

    public static int getRandomSeal() {
        return rnd.Next(0, SEAL_RATE - 1);
    }

    public static int getRandomTrap()
    {
        return rnd.Next(0, TRAP_RATE - 1);
    }

}
