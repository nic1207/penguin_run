using UnityEngine;
using System.Collections;

public class FlashColor : MonoBehaviour {

	public Color fcolor = Color.red;
	private Color scolor = Color.white;
	private MeshRenderer _renderer = null;
	private bool _isfc = false;
	// Use this for initialization
	void Start () {
		_renderer = gameObject.GetComponent<MeshRenderer> ();
		//scolor = _renderer.sharedMaterial.color;
		StartCoroutine (flashcolor());
	}
	
	// Update is called once per frame
	void Update () {
		//_renderer.sharedMaterial.color = fcolor;
	}

	private IEnumerator flashcolor() {
		while (_renderer != null) {
			if (GameAttribute.Instance != null 
				&& (!GameAttribute.Instance.pause||!GameAttribute.Instance.passed) ) {
				//if()
				if (_isfc)
					_renderer.sharedMaterial.color = fcolor;
				else
					_renderer.sharedMaterial.color = scolor;
				_isfc = !_isfc;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}
}
