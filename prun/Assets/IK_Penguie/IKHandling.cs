using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour {

    private Transform locTarget;             // 身體碰撞時彈跳的參考對像
    private Transform flyTarget;
    public int weight = 1;

    public Transform leftHandTarget;        // The target for left hand
    public Transform rightHandTarget;       // The target for right hand

    public Transform leftElbowTarget;       // The target for left elbow
    public Transform rightElbowTarget;      // // The target for right elbow

    public bool beHit = false;              // 撞到物件
    public bool beFly = false;              // 手上下擺動, 用於飛行

    private Animator anim;
    private Animator locAnim = null;

    // Fly paramaters
    private float myY = 1;
    private bool toUp = true;
    private bool toLeft = true;
    private bool beSetHiy = false;

    public bool moveLeft {
        set {
            toLeft = value;
        }
    }

    /*
    private void SetHandPosition() {
        Vector3 lefHandPos = transform.position + new Vector3(-4, 0, 0);
        Vector3 rightHandPos = transform.position + new Vector3(4, 0, 0);

        if (toLeft)
        {
            leftHandTarget.position = lefHandPos;
            leftElbowTarget.position = lefHandPos;
            rightHandTarget.position = rightHandPos;
            rightElbowTarget.position = rightHandPos;
        }
        else {
            leftHandTarget.position = rightHandPos;
            leftElbowTarget.position = rightHandPos;
            rightHandTarget.position = lefHandPos;
            rightElbowTarget.position = lefHandPos;
        }
    }
    */

    private void SetHandPosition() {
        Vector3 lefHandPos = transform.position + new Vector3(-4, toLeft ? -0.2f : 0.2f, 0);
        Vector3 rightHandPos = transform.position + new Vector3(4, toLeft ? 0.2f : -0.2f, 0);

        leftHandTarget.position = lefHandPos;
        leftElbowTarget.position = lefHandPos;
        rightHandTarget.position = rightHandPos;
        rightElbowTarget.position = rightHandPos;
    }
    

    // Use this for initialization
    void Start () {
        if (locTarget == null) {
            locTarget = GameObject.Find("JumpTarget").transform;
        }
        if (flyTarget == null) {
            flyTarget = GameObject.Find("FlyTarget").transform;
        }
        anim = GetComponent<Animator>();
        locAnim = locTarget.GetComponent<Animator>();
        SetHandPosition();
    }

	// Update is called once per frame
	void Update () {
        // Active Hit Animation of the Sphere.
		if (GameAttribute.Instance!=null && GameAttribute.Instance.pause)
			return;
		if (GameAttribute.Instance!=null && GameAttribute.Instance.passed)
			return;
        if (beHit)
        {
            locAnim.SetBool("Hit", true);
            beSetHiy = true;
			beHit = false;
        }

        #region HIT
        if (locAnim.GetBool("Hit"))
        {
            //SetHandPosition();
            transform.rotation = Quaternion.Euler(-20f, 0f, toLeft ? 20f : -20f);
            //transform.rotation = Quaternion.Euler(45f, 0f, 0f);
            transform.position = new Vector3(transform.position.x, locTarget.position.y, transform.position.z);
        }
        else if (beSetHiy) {
            beSetHiy = false;
            //beHit = false;
            if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
        #endregion

        #region FLY
        if (beFly || locAnim.GetBool("Hit"))
        {
			if(GameAttribute.Instance.pause)
				return;
			if(GameAttribute.Instance.passed)
				return;
			if (toUp)
            {
                myY += 0.5f;
            }
            else {
                myY -= 0.5f;
            }
            if (myY > 3)
            {
                toUp = false;
            }
            else if (myY < -1)
            {
                toUp = true;
            }
            leftHandTarget.localPosition = new Vector3(-4, myY, 0);
            rightHandTarget.localPosition = new Vector3(4, myY, 0);
        }
        else {
            if (myY != 1)
            {
                myY = 1;
                toUp = true;
                leftHandTarget.localPosition = new Vector3(-4, myY, 0);
                rightHandTarget.localPosition = new Vector3(4, myY, 0);
            }
        }	
        #endregion
    }

    void OnAnimatorIK() {
        if (locAnim == null)
            return;
		if (GameAttribute.Instance && GameAttribute.Instance.pause)
			return;
		if (GameAttribute.Instance && GameAttribute.Instance.passed)
			return;
		
        if (locAnim.GetBool("Hit") || beFly)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, weight);
            anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, weight);

            anim.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTarget.position);
            anim.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTarget.position);

            anim.SetIKRotationWeight(toLeft ? AvatarIKGoal.LeftFoot : AvatarIKGoal.RightFoot, weight);

            if (locAnim.GetBool("Hit"))
                anim.SetIKRotation(toLeft ? AvatarIKGoal.LeftFoot : AvatarIKGoal.RightFoot, Quaternion.Euler(0, 0, 0));
        }
    }
}
