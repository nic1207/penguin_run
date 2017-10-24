using UnityEngine;
using System.Collections.Generic;

public class StageDisplay : MonoBehaviour {

    /// <summary>
    /// 關卡資料
    /// </summary>
    public class StageData {
        public int Stage = 0;
        public bool Unlock = false;
        public int Stars = 0;

        public StageData(int stage, bool unlock, int stars) {
            Stage = stage;
            Unlock = unlock;
            Stars = stars;
        }
    }

    // For singleton
    public static StageDisplay Instance;

	public GameObject handtap;

    // 要呈現的資料
    private List<StageData> stageDatas = new List<StageData>();     

    private List<StageInfo> stageInfos = new List<StageInfo>();
    private const int MAX_GROUP = 3;
    private const int MAX_STAGE = 12;
    private int selGroup = 1;

	// Use this for initialization
	void Awake () {
		//LevelData.clearAll ();
        Instance = this;
        loadStage();
        loadStageData();
        changeGroup(1);
    }

    // Update is called once per frame
    void Update () {
	
	}

    /// <summary>
    /// 載入關卡資料
    /// </summary>
    private void loadStageData() {        
		int unlocklevel = LevelData.getLevelUnlock ();
		//Debug.Log ("unlocklevel="+ unlocklevel);
		for (int i = 0; i < MAX_GROUP; i++) {
			for (int j = 0; j < MAX_STAGE; j++) {
				//LevelData
				int now = i * MAX_STAGE + j + 1;

				if (unlocklevel < 1)
					unlocklevel = 1;
				if (unlocklevel > GlobalConst.MaxLevel)
					unlocklevel = GlobalConst.MaxLevel;
				
				int star = LevelData.getLevelStar (now);
				bool locked = true;
				if (now > unlocklevel)
					locked = false;
				StageData data = new StageData(now, locked, star);
				stageDatas.Add(data);
			}
		}
		//Debug.Log (stageDatas);
		if (unlocklevel <= 12) {
			float xx, yy;
			xx = unlocklevel % 3 - 1;
			if (xx < 0)
				xx = 2;
			yy = unlocklevel / 3 -1;
			if(xx==2)
				yy = unlocklevel / 3 -2;
			//Debug.Log (xx+" "+yy);
			if (handtap != null) {
				GameObject go = Instantiate (handtap, Vector3.zero, Quaternion.identity) as GameObject;
				go.name = "handtop_"+unlocklevel.ToString();
				go.transform.parent = gameObject.transform;
				//go.transform.localScale = Vector3.one;
				//go.transform.localPosition = Vector3.zero;
				go.transform.localScale = new Vector3(100, 100, 100);
				//go.transform.parent = handtap.transform.parent;
				go.transform.localPosition = new Vector3 ((xx * 190)-150, 130+(yy * -180), 0);

			}
		}
    }

    /// <summary>
    /// 變更單一關卡狀態顯示
    /// </summary>
    /// <param name="data"></param>
    public void changeStageInfo(StageData data) {
        StageInfo info = stageInfos[data.Stage % MAX_STAGE];
        info.Stage = data.Stage;
        info.unlockStage(data.Unlock);
        info.setStars(data.Stars);
    }

    /// <summary>
    /// 變更群組, 表示使用者滑動至其他12關
    /// </summary>
    /// <param name="group"></param>
    public void changeGroup(int group) {
        selGroup = group;

        for (int i = 0; i < MAX_STAGE; i++) {
            StageData data = stageDatas[i + (selGroup - 1) * MAX_STAGE];
            stageInfos[i].Stage = data.Stage;
            stageInfos[i].unlockStage(data.Unlock);
            stageInfos[i].setStars(data.Stars);
        }
    }

    /// <summary>
    /// 載入控制關卡元件
    /// </summary>
    private void loadStage() {
        GameObject obj;
        for (int i = 1; i <= MAX_STAGE; i++) {
            obj = GameObject.Find("StageItem" + i );
			//LeanTween.scale (obj, new Vector3 (0.9f, 0.9f, 0.9f), 1.0f).setLoopPingPong ();
            if (obj != null) {
                StageInfo info = obj.GetComponent<StageInfo>();
                info.Stage = (i);
                info.unlockStage(false);
                info.selectStage = Stage_Click;
                stageInfos.Add(info);
            }
        }
    }

    /// <summary>
    /// 按下回到首頁按鍵
    /// </summary>
    public void HomeButton_Click() {
        Debug.Log("Home button is clicked.");
		SoundManager.Instance.PlaySE ("button", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("SelCharacter");
    }

    /// <summary>
    /// 按下關卡按鍵
    /// </summary>
    /// <param name="stage"></param>
    public void Stage_Click(int stage) {
		//Debug.Log("Stage button " + stage + " is clicked.");
		SoundManager.Instance.PlaySE ("button", false);
		string levelname = "Level";
		switch (stage) {
		case 1:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 2:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 3:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 4:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 5:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 6:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 7:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 8:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 9:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 10:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 11:
			levelname += string.Format ("{0:00}", stage);
			break;
		case 12:
			levelname += string.Format ("{0:00}", stage);
			break;
		}
		//SceneManager.Instance.Play(
		UnityEngine.SceneManagement.SceneManager.LoadScene (levelname);
    }

}
