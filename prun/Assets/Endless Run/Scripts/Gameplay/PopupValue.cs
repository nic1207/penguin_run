using UnityEngine;
using System.Collections;

public class PopupValue : MonoBehaviour {

    private GameObject textObj;
    public int popIdx = 0;
    //銷毀時間
    public float FreeTime = 1.5F;

    //傷害數值
    private int Value = 0;
    //目標位置
    private Vector3 mTarget;
    //屏幕坐標
    private Vector3 mScreen;
    //文本寬度
    //public float ContentWidth = 100;
    //文本高度
    //public float ContentHeight = 50;

    //GUI坐標
    //private Vector2 mPoint;

    //public GUIStyle style;
    private Transform targetObj;
    private RectTransform rectTrans;
    private float deltaY = 0;
    private float delayTime = 0;        // 顯示間隔
    private float MAX_TOP = 160 * Screen.height / 640;
    private bool endDiaply = false;
    private float DELTA_TOP = 5 * Screen.height / 640;

    private void showVal() {
        if ((Value <= 0) || endDiaply)
            return;

        if (deltaY < MAX_TOP && ((delayTime + 0.01) < Time.fixedTime))
        {
            deltaY += DELTA_TOP;
            delayTime = Time.fixedTime;
        }
        //text.transform.position = mScreen + new Vector3(0, deltaY);
        rectTrans.localPosition = mScreen + new Vector3(0, deltaY);
        //if (mScreen.x < 50 || mScreen.x > Screen.width - 50)
        if ((mScreen.x < (-Screen.width / 2 + 50)) || (mScreen.x > (Screen.width / 2 - 50)))
        {
            endDiaply = true;
            //text.transform.position = new Vector3(Screen.width / 2, -2000);
            rectTrans.localPosition = new Vector3(Screen.width / 2, -2000);
        }
    }

    private void initDeltaY() {
        if (popIdx % 2 == 0)
        {
            deltaY = 10 * Screen.height / 640;
        }
        else {
            deltaY = 0;
        }
    }

    void Start()
    {
		textObj = GameObject.Find ("PopVal["+popIdx+"]");
		if(textObj)
			rectTrans = textObj.GetComponent<RectTransform>();
        // 當同時顯示時會有高低的差別
        initDeltaY();
		//DontDestroyOnLoad (this.transform.parent.gameObject);
    }

    public void playPopupValue(Transform target, int val) {
        targetObj = target;
        endDiaply = false;
        transform.position = target.transform.position;
        Value = val;
        //獲取目標位置
        mTarget = transform.position;
        textObj.GetComponent<UnityEngine.UI.Text>().text = "+" + val;
        //獲取屏幕坐標
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        mScreen.x -= Screen.width / 2;
        mScreen.y -= 7 * Screen.height / 16;
        //Debug.Log("mScreen = " + mScreen);
        //將屏幕坐標轉化为GUI坐標
        //mPoint = new Vector2(mScreen.x - 10, Screen.height - mScreen.y);
        showVal();
        //開启自動銷毀線程
        StartCoroutine("Free");
    }

    void Update()
    {
        if ((Value <= 0) || endDiaply)
            return;

        //使文本在垂直方向山產生一個偏移
        //transform.Translate(Vector3.up * 0.5F * Time.deltaTime);
        //重新計算坐標
        mTarget = transform.position;
        //獲取屏幕坐標
        mScreen.x = Camera.main.WorldToScreenPoint(mTarget).x;
        mScreen.x -= Screen.width / 2;
        //將屏幕坐標轉化为GUI坐標
        //mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        // 顯示文字
        showVal();
    }

    void OnGUI()
    {
        /*
        //保證目標在攝像機前方
        if (mScreen.z > 0 && Value > 0)
        {
            if (y > 370 && ((delayTime + 0.01) < Time.fixedTime))
            {
                y -= 5;
                delayTime = Time.fixedTime;
            }
            Rect rect = new Rect(mScreen.x - 20, y, ContentWidth, ContentHeight);
            //內部使用GUI坐標進行繪制
            GUI.Label(rect, "+" + Value, style);
        }
        */
    }

    IEnumerator Free()
    {
        yield return new WaitForSeconds(FreeTime);
        //text.transform.position = new Vector3(Screen.width / 2, -2000);
        rectTrans.localPosition = new Vector3(Screen.width / 2, -2000);
        endDiaply = true;
        //Destroy(this.gameObject);
        initDeltaY();
    }
}
