/// <summary>
/// Collider spawn check.
/// This script use to check collider when character hit object(obstacle)
/// </summary>

using UnityEngine;
using System.Collections;

public class ColliderSpawnCheck : MonoBehaviour {

	public bool isCollision = false;
	public GameObject headParent;
	public string nameColliderHit;
	//public bool checkByName;
	//public bool checkByTag;
	public float nextPos;
	
	//public GameObject player = null;

	public void Start() {
		//if (player == null) {
		//	GameObject[] xx = GameObject.FindGameObjectsWithTag ("Player");
		//	Debug.Log (xx.Length);
		//}
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log (col.gameObject.name+ " "+col.gameObject.tag);
		if (col.gameObject.tag == nameColliderHit) {
			//Debug.Log ("next:"+nextPos+" isCollision="+isCollision);
			isCollision = true;	
		}
	}

	/*
	public void Update(){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
			Debug.Log (player.name);
		}
		if(player && player.transform.position.z > 0 && player.transform.position.z >= this.transform.position.z){
			//if(player.transform.position.z >= headParent.transform.position.z){
			Debug.Log (player.transform.position.z+ " > "+ headParent.transform.position.z);
			isCollision = true;	
		}
	}
	*/
}
