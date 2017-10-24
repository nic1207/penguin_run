using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingPage : MonoBehaviour {

    public Image progressBar;
    public Image mainPic;

    private const float defaultDelayTime = 0.04f;
    private float previousTime = 0;
    private float percentage = 0;
    //private float changeImgTime = 0;
    //private int imageIdx = 0;

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBGM("start", false);
        progressBar.fillAmount = percentage;
        //changeImgTime = Time.time;
		//LeanTween.scale (mainPic.gameObject, new Vector3 (.9f, .9f, .9f), 1.0f).setLoopPingPong ();
    }
	
	// Update is called once per frame
	void Update () {
        if (percentage < 1f && previousTime + defaultDelayTime < Time.time) {
            previousTime = Time.time;
            percentage += 0.01f;
            progressBar.fillAmount = percentage;
        }
        //Debug.Log("percentage = " + percentage);
        if (percentage > 1f) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("SelCharacter");
        }
		/*
        if (changeImgTime + 0.5f < Time.time) {
            imageIdx = (imageIdx + 1) % 2;
            changeImgTime = Time.time;
            //mainPic.sprite = loadLoadImg("game_home_0" + (imageIdx + 1));
        }
        */
	}

    /// <summary>
    /// 載入圖片, 請將圖片放入Resources目錄中
    /// </summary>
    /// <param name="fileName">檔案名稱, 不含附檔名</param>
    /// <returns></returns>
    //Sprite loadLoadImg(string fileName)
    //{
    //    return Resources.Load<Sprite>(fileName);
    //}

}
