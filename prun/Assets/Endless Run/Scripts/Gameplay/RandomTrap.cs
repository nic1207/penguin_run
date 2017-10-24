using UnityEngine;
using System.Collections;

public class RandomTrap : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GameObject go = RandomTrapItemManager.Instance.getTrap(this.transform);
        if (go != null) 
            go.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
