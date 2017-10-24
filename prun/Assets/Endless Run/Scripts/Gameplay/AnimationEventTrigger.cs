using UnityEngine;
using System.Collections;

public class AnimationEventTrigger : MonoBehaviour {

	public GameObject angryline = null;
	public GameObject hitEffect = null;

	public Texture ttangry;
	public Texture ttnormal;

	// Use this for initialization
	void Start () {
		ttangry = Resources.Load ("seajurgar4") as Texture;
		ttnormal = Resources.Load ("seajurgarVer2") as Texture;
		if (angryline) {
			MeshRenderer mr = angryline.GetComponent<MeshRenderer> ();
			mr.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnAngry() {
		//Debug.Log ("Angry...");
		if (hitEffect != null)
			initEffect (hitEffect);
		StartCoroutine (flashMaterial(gameObject));
	}

	private void initEffect(GameObject prefab){
		//Debug.Log (prefab);
		if (!prefab)
			return;
		Vector3 v3 = new Vector3 (gameObject.transform.position.x-0.5f, gameObject.transform.position.y+1.0f, gameObject.transform.position.z-0.5f); 
		GameObject go = (GameObject) Instantiate(prefab, v3, Quaternion.identity);
		//go.transform.parent = gameObject.transform;
		//go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, go.transform.localPosition.z);	
	}

	IEnumerator flashMaterial(GameObject go){
		SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer> ();
		for (int i = 0; i < renderers.Length; i++) {
			renderers [i].material.mainTexture = ttangry;
		}
		MeshRenderer mr = null;
		if (angryline) {
			mr = angryline.GetComponent<MeshRenderer> ();
			mr.enabled = true;
		}
		yield return new WaitForSeconds (2);
		for (int i = 0; i < renderers.Length; i++) {
			renderers [i].material.mainTexture = ttnormal;
		}
		if (mr) {
			mr.enabled = false;
		}
	}
}