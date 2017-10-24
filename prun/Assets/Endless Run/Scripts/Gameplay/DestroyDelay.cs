using UnityEngine;
using System.Collections;

public class DestroyDelay : MonoBehaviour {
	public int Delay = 2;
	// Use this for initialization
	void Start () {
		//StartCoroutine(DestroySelf(gameObject));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DestroySelf(GameObject obj) {
        //play your sound
        Debug.Log("DestroySelf: " + obj.name);
		yield return new WaitForSeconds(Delay); //waits 3 seconds
        //Destroy(gameObject); //this will work after 3 seconds.

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, 0));
        rb.useGravity = false;

    }
}
