using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class SettingMaterial : MonoBehaviour {

	private static bool mchanged = false;
	public Vector4 offset;
	public float brightness = 2.78f;
	public float distance = 100;

	public Material[] materials;

	public static SettingMaterial Instance;
	void Start() {
		Instance = this;
	}
	 //#if UNITY_EDITOR

	//void Update () {
	//}

	public void ChangeAll() {
		for(int i = 0; i < materials.Length; i++){
			//materials[i].shader = Shader.Find("Custom/Curved3");;
			if (materials [i] != null) {
				materials [i].SetVector ("_QOffset", offset);
				//materials [i].SetFloat ("_Brightness", brightness);
				//materials [i].SetFloat ("_Dist", distance);
			}
		}
	}
	//#endif

	public static void ChangeX(int x) {
		if (mchanged)
			return;
		Instance.offset.x = x;
		Instance.ChangeAll ();
		mchanged = true;
	}
	public static void ChangeY(int y) {
		if (mchanged)
			return;
		Instance.offset.y = y;
		Instance.ChangeAll ();
		mchanged = true;
	}
	public static void ChangeZ(int z) {
		if (mchanged)
			return;
		Instance.offset.z = z;
		Instance.ChangeAll ();
		mchanged = true;
	}

	public static void needChange() {
		mchanged = false;
	}
}
