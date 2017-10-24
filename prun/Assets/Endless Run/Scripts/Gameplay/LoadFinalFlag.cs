using UnityEngine;
using System.Collections;

public class LoadFinalFlag : MonoBehaviour {


	// Use this for initialization
	void Start () {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.sprite = Resources.LoadAll<Sprite>("StageClearFlag")[GameAttribute.Instance.level - 1];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
