using UnityEngine;
using System.Collections;

public class ActionBtn : MonoBehaviour {

	public Texture2D normal;
	public Texture2D actived;
	public string levelName;
	
	public AudioClip sfxButton;

	private bool ready;

	void OnMouseDown(){
		this.GetComponent<GUITexture>().texture = actived;
		
		if(sfxButton != null)
			AudioSource.PlayClipAtPoint(sfxButton,transform.position);
	}


	public void OnMouseUpAsButton(){
		this.GetComponent<GUITexture>().texture = normal;
		//Application.LoadLevel (levelName);
		//SceneManager.Instance.Play(0);
	}
	

	void OnMouseExit(){
		this.GetComponent<GUITexture>().texture = normal;
	}
}
