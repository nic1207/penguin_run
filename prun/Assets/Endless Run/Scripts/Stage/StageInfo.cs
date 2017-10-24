using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageInfo : MonoBehaviour {
    public delegate void SelectStage(int stage);

    public Image mainButton;
    public Text ButtonText;
    public List<Image> starImages = new List<Image>();
    public SelectStage selectStage;

    private int mStage = 0;             // 關卡號碼
    private int mStars = 0;             // 星星數
    private bool mUnlock = false;       // 是否解鎖

    /// <summary>
    /// 顯示關卡號碼
    /// </summary>
    private void showStageNum() {
        ButtonText.text = mUnlock && mStage > 0 ? mStage.ToString() : "";
    }

    /// <summary>
    /// 設定關卡號碼
    /// </summary>
    public int Stage {
        set {
            mStage = value;
            showStageNum();
        }
    }

    /// <summary>
    /// 解開關卡
    /// </summary>
    /// <param name="unlock"></param>
    public void unlockStage(bool unlock) {
		//Debug.Log("unlockStage("+unlock+")");
        foreach (Image img in starImages) {
            img.enabled = unlock;
        }
		if (unlock) {
			float tt = 0.3f * (mStage % 15);
			//LeanTween.scale (mainButton.gameObject, new Vector3 (0.0f, 0.0f, 0.0f), 0);
			//LeanTweensetDelay(1f);
			//LeanTween.scale (mainButton.gameObject, new Vector3 (1.0f, 1.0f, 1.0f), 1.5f).setEase(LeanTweenType.easeOutElastic).setDelay(tt);
			//.setLoopType (LeanTweenType.easeInOutElastic);
		}
        mainButton.sprite = loadLoadImg(unlock ? "game_lev_on" : "game_lev_off");
        mUnlock = unlock;
        showStageNum();
    }

    /// <summary>
    /// 設定關卡星星數
    /// </summary>
    /// <param name="num"></param>
    public void setStars(int num) {
        if (num > 3) {
            num = 3;
        }
        mStars = num;

        for (int i = 0; i < starImages.Count; i++) {
            starImages[i].sprite = loadLoadImg(i < num ? "lev_star_on" : "lev_star_off");
        }
    }

    /// <summary>
    /// 載入圖片, 請將圖片放入Resources目錄中
    /// </summary>
    /// <param name="fileName">檔案名稱, 不含附檔名</param>
    /// <returns></returns>
    Sprite loadLoadImg(string fileName)
    {
        return Resources.Load<Sprite>(fileName);
    }

    /// <summary>
    /// 點選關卡
    /// </summary>
    public void Stage_Click() {
        if (selectStage != null && mUnlock) {
            selectStage(mStage);
        }
    }

}
