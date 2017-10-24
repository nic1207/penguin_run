using UnityEngine;
using System.Collections;

public class t_change : MonoBehaviour {
	//private Texture[] front;
	//private Object[] fronto;
	//private Texture[] up;
	//private Object[] upo;
	//private Texture[] left;
	//private Object[] lefto;
	//private Texture[] right;
	//private Object[] righto;
	//private Texture[] back;
	//private Object[] backo;
	//private Texture[] down;
	//private Object[] downo;
	//string cur_name;	
	//private float sec=0;
	//public float day = 48f; // Duration of the day in seconds
	//float next_change;
	//float prev_time=0;
	//int current_hour=0;	
	//float cur_time=0;
	//int next_hour;
	//float blend=0;	
	//float moon_alpha;
	//float light_var;
	//private Skybox sk;
	public float cloudspeed = 0.7f;
	public float rot = 0;
	//public Quaternion rot = Quaternion.Euler (0f, 0f, 0f);

	public Skybox sky;
	void Start() {
		sky = GetComponent<Skybox> ();
	}
	void Update () {
		//rot = Quaternion.Euler (60f, 0f, 0f);
		//Matrix4x4 m = Matrix4x4.TRS (Vector3.zero, rot, new Vector3(1,1,1) );
		//sky.material.SetMatrix ("_Rotation", m);
		rot += cloudspeed * Time.deltaTime;
		rot %= 360;
		sky.material.SetFloat ("_Rotation", rot);
	}
}
