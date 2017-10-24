using UnityEngine;
using System.Collections;

public class WC15PRO : MonoBehaviour
{

	public float wavesHeight = 0.3f;
	public float speed = 0.015f;
	public float wavesMode = 0.2f;
	public GameObject splashPref = null;
	private GameObject splash = null;

	float buf;
	float iterator;
	int meshCounter;
	Vector3[] lastDy;

	// Use this for initialization
	void Start () {
		buf = 0;
		iterator = 0;
		meshCounter = 0;

		int i = 0;
		lastDy = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];
		while (i < GetComponent<MeshFilter>().mesh.vertices.Length) {
			lastDy[i] = new Vector3();
			i++;
		}
		if (splashPref != null) {
			//splash = Instantiate (splashPref) as GameObject;
			//splash.transform.position = new Vector3 (-100, -100, -100);
			//splash.SetActive (false);
		}
	}



	// Update is called once per frame
	void Update () {
		if (GameAttribute.Instance && GameAttribute.Instance.pause)
			return;
		if (GameAttribute.Instance && GameAttribute.Instance.passed)
			return;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3 dy = new Vector3();

		while (meshCounter < vertices.Length) {
			buf = meshCounter;

			if(meshCounter % 11 != 0 && (120 - meshCounter) % 11 != 0)
			{
				if(meshCounter > 10 && meshCounter <110)
				{
					dy = new Vector3(0,Mathf.Sin((Time.deltaTime + buf + iterator)) * wavesHeight * Mathf.Cos((Time.deltaTime + buf + iterator) * wavesMode));
					vertices[meshCounter] += dy - lastDy[meshCounter];	
				}
				else
				{
					dy = new Vector3();
					vertices[meshCounter]+= dy;
				}
			}
			else
			{
				dy = new Vector3();
				vertices[meshCounter]+= dy;

			}
			lastDy[meshCounter] = dy;

			meshCounter++;
		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();	

		meshCounter = 0;

		iterator += speed;
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log ("OnTriggerEnter("+col.name+")");
		if (col.tag == "Player") {
			//GUIManager.Instance.ShowFailedMenu ();
			StartCoroutine (jumpWater(col.transform.position));
		}
	}

	private IEnumerator jumpWater(Vector3 pos) {
		if (splashPref != null) {
			splash = Instantiate (splashPref) as GameObject;
			splash.transform.position = pos;
			//splash.SetActive (true);
		}
		GameController.Instance.cameraFol.Target = null;
		yield return new WaitForSeconds (1.0f);
		GUIManager.Instance.ShowFailedMenu ();
		yield return null;
	}
}
