using UnityEngine;
using System.Collections;

public class LocTargerCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TouchGround() {
        SoundManager.Instance.PlaySE("step_on", false);
    }

    public void FinishHit() {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;

        //Debug.Log("FinishHit(): " + target);
        if (target != null) {
            Controller ctrl = target.GetComponent<Controller>();
            ctrl.ishited = false;
            //Debug.Log("FinishHit(): ctrl.ishited = " + ctrl.ishited);
        }
    }

}
