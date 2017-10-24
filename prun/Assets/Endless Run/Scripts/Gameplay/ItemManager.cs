using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

	public List<GameObject> Items = new List<GameObject>();
	private static ItemManager _instance;

	public static ItemManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = (ItemManager) FindObjectOfType(typeof(ItemManager));

				if (_instance == null)
				{
					Debug.LogError("ItemManager Instance Error");
				}
			}
			return _instance;
		}
	}

	void Awake (){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("ItemManager");
		if( obj.Length > 1 ){
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	public void Add(GameObject item) {
		Items.Add(item);
	}

	public void Remove(GameObject item) {
		Items.Remove(item);
	}

	public void RemoveAt(int index) {
		Items.RemoveAt(index);
	}

	public void HideAll() {
		for(int i=0;i<Items.Count ;i++) {
			if(Items[i]!=null)
				Items[i].SetActive(false);
		}
	}

	public void ShowAll() {
		for(int i=0;i<Items.Count ;i++) {
			Items[i].SetActive(true);
		}
	}

	public void ClearAll() {
		Items.Clear();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
