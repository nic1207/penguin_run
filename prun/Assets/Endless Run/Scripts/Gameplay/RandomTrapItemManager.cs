using UnityEngine;
using System.Collections;

public class RandomTrapItemManager : MonoBehaviour {

    private static bool[,] DEFAULT_ENABLETRAP = new bool[,] {
        { false, false, false, false},      // Level 1
        { true, true, true, true},          // Level 2
        { true, true, true, true},          // Level 3
        { true, true, true, true},          // Level 4
        { true, true, true, true},          // Level 5
        { true, true, true, true},          // Level 6
        { true, true, true, true}          // Level 7
    };

    public GameObject[] trapPrefab;

    public static RandomTrapItemManager Instance;

    // Use this for initialization
    void Start () {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject getTrap(Transform trans) {
        int val;

        if (RandomVariable.getRandomTrap() != 0) {
            return null;
        }

        do
        {
            val = RandomVariable.getRandomValue(0, trapPrefab.Length);
            if (val >= DEFAULT_ENABLETRAP.GetLength(1)) {
                return null;
            }
        } while (!DEFAULT_ENABLETRAP[GameAttribute.Instance.level, val] && val < DEFAULT_ENABLETRAP.GetLength(1));

        return Instantiate(trapPrefab[val], trans.position, Quaternion.identity) as GameObject;
    }
}
