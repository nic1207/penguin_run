/// <summary>
/// Shop character.
/// This script use for create shop menu
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopCharacter : MonoBehaviour {

	[System.Serializable]
	public class CharacterData
	{
		public GameObject player;
		public string name;
		public int price;
		public bool isUnLock;
	}

	public CharacterData[] players;
	public float factorSpace;
	public int coin;

	public GameObject DisplayCanvas = null;
	public Text CoinText = null;
	public Text PriceText = null;
	public Button BtnBuy = null;
	public Button BtnPlay = null;
	public Button BtnLeft = null;
	public Button BtnRight = null;

	//public AudioClip sfxButton;

	private int indexSelect;
	private int selecCorrect;
	private List<Vector3> point = new List<Vector3> ();
	private Vector3 getMousePos;

	void ResetData(){
		for (int i = 0; i < players.Length; i++) {
			PlayerPrefs.SetString("Player_"+i, "False");
		}
	}

	void Start(){
		PriceText.enabled = false;
		BtnBuy.gameObject.SetActive (false);
		BtnPlay.gameObject.SetActive (false);
		//LeanTween.scale (BtnPlay.gameObject, new Vector3 (.9f, .9f, .9f), 1.0f).setLoopPingPong ();
        SoundManager.Instance.PlayBGM("menu", true);
		//ResetData ();
		coin = GameData.LoadFish ();
		selecCorrect = PlayerPrefs.GetInt ("SelectPlayer");
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < players.Length; i++) {
			players [i].player.transform.localPosition = new Vector3 (pos.x + (i * factorSpace), 0, 0);
			point.Add (new Vector3 (-1 * (pos.x + (i * factorSpace)) + transform.position.x, transform.position.y, transform.position.z));

			if (i == 0) {
				players [i].isUnLock = true;
			} else {
				//Debug.Log (PlayerPrefs.GetString ("Player_" + i));
				if(PlayerPrefs.GetString ("Player_" + i) == ""){
					players [i].isUnLock = false;
				}else{
					players [i].isUnLock = bool.Parse (PlayerPrefs.GetString ("Player_" + i));
				}

			}
		}

		StartCoroutine (WaitInput ());
	}

	void OnGUI(){
		
		CoinText.text = coin.ToString ();
        if (players [indexSelect].isUnLock == false) 
		{
			PriceText.text = players[indexSelect].price.ToString();
			PriceText.enabled = true;
			BtnBuy.gameObject.SetActive (true);
			BtnPlay.gameObject.SetActive (false);
		} else {
			PriceText.enabled = false;
			BtnBuy.gameObject.SetActive (false);
			BtnPlay.gameObject.SetActive (true);

			//LeanTween.scale (BtnPlay.gameObject, new Vector3 (1.2f, 1.2f, 1.2f), 1.0f).setLoopPingPong ();
		}
		//Debug.Log ("indexSelect="+indexSelect);
		if (indexSelect <= 0) {
			BtnLeft.gameObject.SetActive (false);
		} else {
			BtnLeft.gameObject.SetActive (true);
		}
		if (indexSelect >= players.Length - 1) {
			BtnRight.gameObject.SetActive (false);
		} else {
			BtnRight.gameObject.SetActive (true);
		}
	}

	IEnumerator WaitInput() {
        //Debug.Log("WaitInput()");
        bool input = false;

		while (input == false) {
			if(Input.GetMouseButtonDown(0)){
				getMousePos = Input.mousePosition;
				input = true;
			}
			yield return 0;
		}
		StartCoroutine (SelectDirection ());
	}

	IEnumerator SelectDirection(){
        //Debug.Log("SelectDirection()");
        bool input = false;
		while (input == false) {
			if((Input.mousePosition.x - getMousePos.x) < -40){
				indexSelect++;
				if(indexSelect >= players.Length-1){
					indexSelect = players.Length-1;
				}
				input = true;
			}

			if((Input.mousePosition.x - getMousePos.x) > 40){
				indexSelect--;
				if(indexSelect <= 0){
					indexSelect = 0;
				}
				input = true;
			}

			if(Input.GetMouseButtonUp(0)){
				input = true;
			}
			yield return 0;
		}

		StartCoroutine (MoveToPoint ());
	}

	IEnumerator MoveToPoint(){
        //Debug.Log("MoveToPoint()");
		while (Vector3.Distance(transform.position, point[indexSelect]) > 0.01f) {
			transform.position = Vector3.Lerp(transform.position, point[indexSelect], 10 * Time.deltaTime);
			yield return 0;
		}

		transform.position = point [indexSelect];

		StartCoroutine (WaitInput ());
	}

	public void doPlay() {
		selecCorrect = indexSelect;
		PlayerPrefs.SetInt("SelectPlayer", selecCorrect);
		//SceneManager.Instance.Play (2);
		//SoundManager.Instance.PlaySE (8, false);
		SoundManager.Instance.PlaySE ("button", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("StageSel");
	}

	public void doRight() {
		players [indexSelect].player.transform.position = new Vector3(players [indexSelect].player.transform.position.x, 0, players [indexSelect].player.transform.position.z);
		indexSelect++;
		if(indexSelect >= players.Length-1){
			indexSelect = players.Length-1;
		}
		Animator ani = players [indexSelect].player.GetComponent<Animator> ();
		ani.SetTrigger ("Wave");
		SoundManager.Instance.PlaySE ("button", false);
	}

	public void doLeft() {
		players [indexSelect].player.transform.position = new Vector3(players [indexSelect].player.transform.position.x, 0, players [indexSelect].player.transform.position.z);
		indexSelect--;
		if(indexSelect <= 0){
			indexSelect = 0;
		}
		Animator ani = players [indexSelect].player.GetComponent<Animator> ();
		ani.SetTrigger ("Wave");
		SoundManager.Instance.PlaySE ("button", false);
	}

	public void doBuy() {
		SoundManager.Instance.PlaySE ("button", false);
		if(coin >= players[indexSelect].price)
		{
			coin -= players[indexSelect].price;
			GameData.SaveFish(coin);
			players[indexSelect].isUnLock = true;
			PlayerPrefs.SetString("Player_"+indexSelect, "True");
			PlayerPrefs.SetInt("SelectPlayer", selecCorrect);
			Debug.Log("Buy : "+indexSelect+ " : " + PlayerPrefs.GetString("Player_"+indexSelect));
		}

		//if(sfxButton != null)
		//	AudioSource.PlayClipAtPoint(sfxButton,transform.position);
	}
}
