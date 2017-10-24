using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomFloor : MonoBehaviour {

    public List<Transform> holes = new List<Transform>();
	public List<GameObject> Items = new List<GameObject>();
    //System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator RunRandom() {
        int[] sels = new int[5];
        int i;
        Vector3 temp;

        yield return 0;
        for (i = 0; i < sels.Length; i++)
        {
            sels[i] = RandomVariable.getRandomValue(0, holes.Count - 1);
        }

        for (i = 1; i < sels.Length; i++) {
            if (sels[i - 1] != sels[i]) {
                temp = holes[sels[i - 1]].position;
                holes[sels[i - 1]].position = holes[sels[i]].position;
                holes[sels[i]].position = temp;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(RunRandom());
        }
    }

}
