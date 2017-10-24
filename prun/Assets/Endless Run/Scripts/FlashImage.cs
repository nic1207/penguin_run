using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour {
	public Image mainPic = null;

	private float changeImgTime = 0;
	private int imageIdx = 0;
	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlaySE("ice_explosion", false);
		changeImgTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (changeImgTime + 0.5f < Time.time) {
			imageIdx = (imageIdx + 1) % 2;
			changeImgTime = Time.time;
			mainPic.sprite = loadLoadImg("game_home_0" + (imageIdx + 1));
		}
	}

	/// <summary>
	/// 載入圖片, 請將圖片放入Resources目錄中
	/// </summary>
	/// <param name="fileName">檔案名稱, 不含附檔名</param>
	/// <returns></returns>
	private Sprite loadLoadImg(string fileName)
	{
		return Resources.Load<Sprite>(fileName);
	}
}
