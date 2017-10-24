/// <summary>
/// Controller.
/// this script use for control a character
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class Controller : MonoBehaviour {
	
	public enum DirectionInput{
	   Null, Left, Right, Up, Down
	}
	
	public enum Position{
		Middle, Left, Right	
	}

	public Animator anim;
	public CoinRotation coinRotate;
	public GameObject magnet;
	public float speedMove = 5; 
	public float gravity;
	public float jumpValue;
	public bool ishited = false;

	public GameObject Heli;
	public GameObject FlyLine;
	public GameObject WalkSmoke;
	
	//[HideInInspector]
	//public bool isRoll;
	//[HideInInspector]
	//public bool isDoubleJump;
	//[HideInInspector]
	//public bool isMultiply;
	[HideInInspector]
	public bool isSpeedDown = false;
	[HideInInspector]
	public bool isRun = false;


	[HideInInspector]
	public CharacterController characterController;
	
	[HideInInspector]
	public float timeSprint;
	[HideInInspector]
	public float timeMagnet;
	[HideInInspector]
	public float timeMultiply;
	[HideInInspector]
	public float timeJump;

	public float timeSD;
	
	private bool activeInput;
	private bool jumpSecond;
	
	private Vector3 moveDir;
	private Vector2 currentPos;
	
	public bool keyInput;
	public bool touchInput;
	public bool flying = false;
	
	private Position positionStand;
	private DirectionInput directInput;
	//private AnimationManager animationManager;

	private Vector3 direction;
	private Vector3 currectPosCharacter;
	
	public static Controller Instance;

    // Andy adds.
    private IKHandling ikHanlding = null;
    private float flyTimeStamp = 0;
    private float moveDirY = 0;
    private int popIdx = 0;
	private float oldSpeed = 0;
	private GameObject[] mObjects;

    public void WinGame() {
		//stopFly ();
		SoundManager.Instance.PlaySE ("stage_clear", false);
		StartCoroutine (RotateSelf());
		//gameObject.transform.Rotate (new Vector3 (0, 180, 0));
		//anim.SetBool ("Wave", true);
	}

	private IEnumerator RotateSelf() {
		int angel = 0;
		while (angel < 180) {
			gameObject.transform.Rotate (Vector3.up, 10);
			angel += 10;
			yield return new WaitForSeconds (0.01f);
		}
		anim.SetBool ("Wave", true);
	}

	public void walkToStation() {
		isRun = false;
		anim.SetBool ("Run", isRun);
		stopFly ();
		speedMove = 5;
		positionStand = Position.Middle;
		anim.SetBool ("Walk", true);
	}

	//public void reset() {
		//anim.SetBool ("Walk", false);
		//anim.SetBool ("Wave", false);
		//gameObject.transform.Rotate (new Vector3 (0, 180, 0));
	//}

	//Check item collider
	void OnTriggerEnter(Collider col){
		//Debug.Log ("onTriggerEnter()"+col.tag);
		if (col.tag == "Item") {
			if (col.GetComponent<Item> ().useAbsorb) {
				col.GetComponent<Item> ().useAbsorb = false;	
				col.GetComponent<Item> ().StopAllCoroutines ();
			}
			col.GetComponent<Item> ().ItemGet ();
            if (++popIdx >= 6) {
                popIdx = 0;
            } 
			mObjects[popIdx].GetComponent<PopupValue>().playPopupValue(this.gameObject.transform, col.GetComponent<Item>().scoreAdd);
        }
        else if (col.tag == "Enemy" || col.tag == "BigTrap" || col.tag == "SmallTrap")
        {
            if (!ishited)
            {
                float x;
                float trapX = col.transform.position.x;
				if (flying) {
					stopFly ();
					TrapManager.Instance.enableAllTraps(true);
				}
                if (col.tag == "Enemy")
                {
                    Animator ani = col.gameObject.GetComponentInParent<Animator>();
                    if (ani != null)
                        ani.SetTrigger("Angry");
                    SoundManager.Instance.PlaySE("hit_seal", false);
                }
                else {
                    SoundManager.Instance.PlaySE("step_on", false);
                }

                ikHanlding.beHit = true;
                ishited = true;
                // 若企鵝在中線, 則隨機決定向左或向右移動
                if (trapX < 1 && trapX > -1)
                {
                    if (RandomVariable.getRandomValue(0, 1) == 0)
                    {
                        positionStand = Position.Left;
                        ikHanlding.moveLeft = true;
                        x = -0.5f;
                    }
                    else {
                        positionStand = Position.Right;
                        ikHanlding.moveLeft = false;
                        x = 0.5f;
                    }
                }
                else {
                    if (trapX > 1)
                    {
                        ikHanlding.moveLeft = true;
                        x = -0.5f;
                    }
                    else {
                        ikHanlding.moveLeft = false;
                        x = 0.5f;
                    }
                    positionStand = Position.Middle;
                }
                //Vector3 dd = new Vector3(x, -1.1f, -0.5f);
                Vector3 dd = new Vector3(x, 0.5f, col.tag == "SmallTrap" ? -3f : -5f);
                StartCoroutine(Knockback(dd));
            }
        }
    }

    /* 原先撞到物件動作
	IEnumerator Knockback(Vector3 dir){
		//Debug.Log (dir);
		//dir.y -= 0.5f;
		//dir.z -= 0.5f;
		//dir.x += 0.5f;
		dir = new Vector3(0, 1, -1);
		float startTime = Time.time;
		float duration = 1.0f;
		float speed = 10.0f;
		while(Time.time < (startTime + duration)){
			characterController.SimpleMove(dir*speed);
			yield return 0;
		}
	}
    */
    IEnumerator Knockback(Vector3 dir)
    {
        float startTime = Time.time;
        float speed = 1.0f;
        int i = 0;
        while (i < 70)
        {
			if (!GameAttribute.Instance.pause || !GameAttribute.Instance.passed) {
				characterController.SimpleMove (dir * speed);
				i++;
			}
            yield return 0;
        }
    }

    /* 原先撞到物件動作
	void onFinishSkip() {
		//Debug.Log ("onFinishSkip()");
		ishited = false;
		anim.SetBool ("Hit", false);
	}
    */
    void onFinishSkip()
    {
        ishited = false;
    }

    void Start(){
		//Set state character
		Instance = this;
		characterController = this.GetComponent<CharacterController>();
		anim = this.GetComponent<Animator>();
        // 找出IK控制script
        ikHanlding = GetComponent<IKHandling>();

		mObjects = new GameObject[6];
		for (int i = 0; i < 6; i++) {
			mObjects[i] = GameObject.Find ("PopValCtrl[" + i + "]");
		}

        //animationManager = this.GetComponent<AnimationManager>();
        speedMove = GameAttribute.Instance.speed;
		jumpSecond = false;
		magnet.SetActive(false);
		stopFly ();
		TrapManager.Instance.enableAllTraps(true);
		Invoke("WaitStart", 0.2f);
	}

	//Reset state,variable when character die
	public void Reset(){
		transform.position = new Vector3(0, transform.position.y, -5);
		gameObject.transform.Rotate (new Vector3 (0, 180, 0));
		positionStand = Position.Middle;
		//jumpSecond = false;
		//isRoll = false;
		//isDoubleJump = false;
		magnet.SetActive(false);
		anim.SetBool ("Run", true);
		StopAllCoroutines();
		StartCoroutine(UpdateAction());
	}
	
	void WaitStart(){
		StartCoroutine(UpdateAction());
	}	
	
	//Update Loop
	IEnumerator UpdateAction(){
		bool paa = false;
		while(GameAttribute.Instance.life > 0){
			paa = GameAttribute.Instance.pause || GameAttribute.Instance.passed;
			if(!paa && GameAttribute.Instance.isPlaying && PatternSystem.instance.loadingComplete){
				if(keyInput)
					KeyInput();
				
				if(touchInput){
					//TouchInput();
					DirectionAngleInput();
				}
				CheckLane();
				MoveForward();
			}
			yield return 0;	
		}
		//StartCoroutine(MoveBack());
		//animationManager.animationState = animationManager.Dead;
		//yield return new WaitForSeconds (2);
		//GameController.instace.StartCoroutine(GameController.instace.ResetGame());
	}
	
	/*
	IEnumerator MoveBack(){
		float z = transform.position.z-0.5f;
		bool complete = false;
		while(complete == false){
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,transform.position.y,z),2*Time.deltaTime);
			if((transform.position.z - z) < 0.05f){
				complete = true;
			}
			yield return 0;
		}
		
		yield return 0;
	}
	*/
	
	private void MoveForward(){
		if (GameAttribute.Instance.pause)
			return;
		if (GameAttribute.Instance.passed)
			return;
		//speedMove = GameAttribute.gameAttribute.speed;
		
		if(characterController.isGrounded){
			//anim.SetBool ("Jump", false);
			moveDir = Vector3.zero;
			if(directInput == DirectionInput.Up){
				Jump();
				//if(isDoubleJump){
				//	jumpSecond = true;	
				//}
			}
		} else {
			//anim.SetBool ("Jump", true);
			//if(directInput == DirectionInput.Down){
				//QuickGround();
			//}
			if(directInput == DirectionInput.Up && !ishited)
            {
				ishited = false;
				anim.SetBool ("Hit", false);
				//if(jumpSecond){
				//	JumpSeccond();
				//	jumpSecond = false;
				//}
			}
			
			//if(animationManager.animationState != animationManager.Jump
			//	&& animationManager.animationState != animationManager.JumpSecond
			//	&& animationManager.animationState != animationManager.Roll){
			//	animationManager.animationState = animationManager.JumpLoop;
			//}
		}
		moveDir.z = 0;
		moveDir += this.transform.TransformDirection(Vector3.forward*speedMove);
        if (ishited)
            moveDir.y = -0.3f;          // 撞到物件時, 由於會彈跳, 故將y值固定
        else
            moveDir.y -= gravity * Time.deltaTime;


        //CheckSideCollision();
        if (!ishited)
        {
            if (flying)
            {
                moveDir.y = GameObject.Find("FlyTarget").transform.position.y;
            }
            characterController.Move((moveDir + direction) * Time.deltaTime);
            anim.SetBool ("Run", true);
		} else {
			characterController.Move ((moveDir + direction) * 0.1f * Time.deltaTime);
			anim.SetBool ("Run", false);
		}
	}
	
	private bool checkSideCollision;
	private float countDeleyInput;

	private void CheckSideCollision(){
			if (positionStand == Position.Right) {
				if((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides){
					if(transform.position.x <= 1.75f && checkSideCollision == false){
						//Debug.Log("Hit right");
						//ishited = true;
						//anim.SetBool ("Hit", true);
						//CameraFollow.instace.ActiveShake();
						positionStand = Position.Middle;
						checkSideCollision = true;
					}
				}
			}

			if (positionStand == Position.Left) {
				if((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides){
					if(transform.position.x >= -1.75f && checkSideCollision == false){
						//Debug.Log("Hit left");
						//ishited = true;
						//anim.SetBool ("Hit", true);
						//CameraFollow.instace.ActiveShake();
						positionStand = Position.Middle;
						checkSideCollision = true;
					}
				}
			}

			if(positionStand == Position.Middle){
				if((int)characterController.collisionFlags == 5 || characterController.collisionFlags == CollisionFlags.Sides){
					if(transform.position.x <= -0.05f && checkSideCollision == false){
						//Debug.Log("Hit mid");
						//CameraFollow.instace.ActiveShake();
						positionStand = Position.Left;
						//ishited = true;
						//anim.SetBool ("Hit", true);
						checkSideCollision = true;
					}else if(transform.position.x >= 0.05f && checkSideCollision == false){
						//Debug.Log("Hit!!"+positionStand);
						//ishited = true;
						//anim.SetBool ("Hit", true);
						//CameraFollow.instace.ActiveShake();
						positionStand = Position.Right;
						checkSideCollision = true;
					}
				}
			}

		if (checkSideCollision == true) {
			countDeleyInput += Time.deltaTime;
			if(countDeleyInput >= 1f){
				checkSideCollision = false;
				countDeleyInput = 0;
			}
		}
	}
	
	private void QuickGround(){
		moveDir.y -= jumpValue*3;
	}
	
	
	//Jump State
	private void Jump(){
        if (ishited)
            return;

        //Play sfx when jump
        SoundManager.Instance.PlaySE("jump", false);
		anim.SetBool ("Jump", true);
		//animationManager.animationState = animationManager.Jump;
		moveDir.y += jumpValue;
		ishited = false;
		anim.SetBool ("Hit", false);
	}

	public void Fly(){
		speedMove = 20;
		flying = true;
		anim.SetBool ("Fly", true);
        ikHanlding.beFly = true;
        TrapManager.Instance.enableAllTraps(false);
        SoundManager.Instance.PlaySE ("fly", true);
		if (Heli!=null && !Heli.activeSelf) {
			Heli.SetActive (true);
			if(FlyLine!=null)
				FlyLine.SetActive (true);
			if (WalkSmoke != null)
				WalkSmoke.SetActive (false);
		}
	}

	public void stopFly() {
		//speedMove = 10;
		timeSprint = 0;
		flying = false;
		anim.SetBool ("Fly", false);
        ikHanlding.beFly = false;
		speedMove = GameAttribute.Instance.speed;
		//TrapManager.trapManager.enableAllTraps(true);
		SoundManager.Instance.StopSE ();
		if (Heli && Heli.activeSelf) {
			Heli.SetActive (false);
			if(FlyLine!=null)
				FlyLine.SetActive (false);
			if (WalkSmoke != null)
				WalkSmoke.SetActive (true);
		}
	}

	private void JumpSeccond(){
		//animationManager.animationState = animationManager.JumpSecond;
		//moveDir.y += jumpValue*1.15f;
	}
	
	private void CheckLane(){
		if (characterController.isGrounded) {
			anim.SetBool ("Jump", false);
		}

		if(positionStand == Position.Middle){
			if(directInput == DirectionInput.Right){
				if(characterController.isGrounded){
					//anim.SetBool ("Jump", false);
					//GetComponent<Animation>().Stop();
					//animationManager.animationState = animationManager.TurnRight;
				}
				positionStand = Position.Right;	
				//Play sfx when step
				SoundManager.Instance.PlaySE("slide", false);
			}else if(directInput == DirectionInput.Left){
				if(characterController.isGrounded){
					//anim.SetBool ("Jump", false);
					//GetComponent<Animation>().Stop();
					//animationManager.animationState = animationManager.TurnLeft;
				}
				positionStand = Position.Left;	
				//Play sfx when step
				SoundManager.Instance.PlaySE("slide", false);
			}

			//transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y,transform.position.z), 6 * Time.deltaTime);

			if(transform.position.x > 0.15f){
				direction = Vector3.Lerp(direction, Vector3.left*6, Time.deltaTime * 300);
			}else if(transform.position.x <= -0.15f){
				direction = Vector3.Lerp(direction, Vector3.right*6, Time.deltaTime * 300);
			}else{
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			}
			//transform.position = Vector3.Lerp (transform.position, new Vector3 (0, transform.position.y, transform.position.z), 6 * Time.deltaTime);


		}else if(positionStand == Position.Left){
			if(directInput == DirectionInput.Right){
				if(characterController.isGrounded){
					//anim.SetBool ("Jump", false);
					//GetComponent<Animation>().Stop();
					//animationManager.animationState = animationManager.TurnRight;
				}
				positionStand = Position.Middle;	
				//Play sfx when step
				SoundManager.Instance.PlaySE("slide", false);
			}
			//transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			if(transform.position.x >= -1.8f){
				direction = Vector3.Lerp(direction, Vector3.left*6, Time.deltaTime * 1000);
			}else{
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			}
		}else if(positionStand == Position.Right){
			if(directInput == DirectionInput.Left){
				if(characterController.isGrounded){
					//anim.SetBool ("Jump", false);
					//GetComponent<Animation>().Stop();
					//animationManager.animationState = animationManager.TurnLeft;
				}
				positionStand = Position.Middle;
				//Play sfx when step
				SoundManager.Instance.PlaySE("slide", false);
			}
			//transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			if(transform.position.x <= 1.8f){
				direction = Vector3.Lerp(direction, Vector3.right*6, Time.deltaTime * 300);
			}else{
				direction = Vector3.zero;
				checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			}
		}
		
		if(directInput == DirectionInput.Down){
			//GetComponent<Animation>().Stop();
			//animationManager.animationState = animationManager.Roll;
			//Play sfx when roll
			//SoundManager.instance.PlayingSound("Roll");
		}
	}
	
	//Key input method
	private void KeyInput()
	{
        if (ishited)        // 撞到物件時不可操作
            return;

        if (Input.anyKeyDown)
		{
			activeInput = true;
		}
		
		if(activeInput && checkSideCollision == false)
		{
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				directInput = DirectionInput.Left;
				activeInput = false;
			}
			else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				directInput = DirectionInput.Right;
				activeInput = false;
			}
			else if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				directInput = DirectionInput.Up;
				activeInput = false;
			}
			else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				directInput = DirectionInput.Down;
				activeInput = false;
			}
		} else {
			directInput = DirectionInput.Null;	
		}
		
		
	}
	
	//Touch input method
	private void TouchInput(){
        if (ishited)        // 撞到物件時不可操作
            return;

        if (Input.GetMouseButtonDown(0)){
			currentPos = Input.mousePosition;	
			activeInput = true;
		}
		if(Input.GetMouseButton(0) && checkSideCollision == false){
			if(activeInput){
				if((Input.mousePosition.x - currentPos.x) > 40){
					directInput = DirectionInput.Right;
					activeInput = false;
				}else if((Input.mousePosition.x - currentPos.x) < -40){
					directInput = DirectionInput.Left;
					activeInput = false;
				}else if((Input.mousePosition.y - currentPos.y) > 40){
					directInput = DirectionInput.Up;
					activeInput = false;
				}else if((Input.mousePosition.y - currentPos.y) < -40){
					directInput = DirectionInput.Down;
					activeInput = false;
				}
			}else{
				directInput = DirectionInput.Null;
			}
			
		}
		if(Input.GetMouseButtonUp(0)){
			directInput = DirectionInput.Null;	
		}
		currentPos = Input.mousePosition;
	}
	
	private void DirectionAngleInput(){
        if (ishited)        // 撞到物件時不可操作
            return;

        if (Input.GetMouseButtonDown(0)){
			currentPos = Input.mousePosition;
			activeInput = true;
		}
		
		if(Input.GetMouseButton(0) && checkSideCollision == false){
			if(activeInput){
				float ang = CalculateAngle.GetAngle(currentPos, Input.mousePosition);
				if((Input.mousePosition.x - currentPos.x) > 20){
					if(ang < 45 && ang > -45){
						directInput = DirectionInput.Right;
						activeInput = false;
					}else if(ang >= 45){
						directInput = DirectionInput.Up;
						activeInput = false;
					}else if(ang <= -45){
						directInput = DirectionInput.Down;
						activeInput = false;
					}
				}else if((Input.mousePosition.x - currentPos.x) < -20){
					if(ang < 45 && ang > -45){
						directInput = DirectionInput.Left;
						activeInput = false;
					}else if(ang >= 45){
						directInput = DirectionInput.Down;
						activeInput = false;
					}else if(ang <= -45){
						directInput = DirectionInput.Up;
						activeInput = false;
					}
				}else if((Input.mousePosition.y - currentPos.y) > 20){
					if((Input.mousePosition.x - currentPos.x) > 0){
						if(ang > 45 && ang <= 90){
							directInput = DirectionInput.Up;
							activeInput = false;
						}else if(ang <= 45 && ang >= -45){
							directInput = DirectionInput.Right;
							activeInput = false;
						}
					}else if((Input.mousePosition.x - currentPos.x) < 0){
						if(ang < -45 && ang >= -89){
							directInput = DirectionInput.Up;
							activeInput = false;
						}else if(ang >= -45){
							directInput = DirectionInput.Left;
							activeInput = false;
						}
					}
				}else if((Input.mousePosition.y - currentPos.y) < -20){
					if((Input.mousePosition.x - currentPos.x) > 0){
						if(ang > -89 && ang < -45){
							directInput = DirectionInput.Down;
							activeInput = false;
						}else if(ang >= -45){
							directInput = DirectionInput.Right;
							activeInput = false;
						}
					}else if((Input.mousePosition.x - currentPos.x) < 0){
						if(ang > 45 && ang < 89){
							directInput = DirectionInput.Down;
							activeInput = false;
						}else if(ang <= 45){
							directInput = DirectionInput.Left;
							activeInput = false;
						}
					}
					
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0)){
			directInput = DirectionInput.Null;	
			activeInput = false;
		}
		
	}
	
	//Sprint Item
	public void Sprint(float speed, float time){
		StopCoroutine("CancelSprint");
		//GameAttribute.gameAttribute.speed = speed;
		timeSprint = time;
		Fly ();
		StartCoroutine(CancelSprint());
	}
	
	IEnumerator CancelSprint(){
		while(timeSprint > 0){
			if(!GameAttribute.Instance.pause||!GameAttribute.Instance.passed)
				timeSprint -= 1 * Time.deltaTime;
			yield return 0;
		}
		//int i = 0;
		stopFly();
		TrapManager.Instance.enableAllTraps(true);
		//GameAttribute.gameAttribute.speed = GameAttribute.gameAttribute.starterSpeed;
		/*
		while(i < GameController.instace.countAddSpeed+1){
			GameAttribute.gameAttribute.speed += GameController.instace.speedAdd;	
			i++;
		}
		*/
	}
	public void SpeedDown(float speed, float time) {
		if (isSpeedDown) {
			timeSD = time;
			return;
		}
		Debug.Log ("SpeedDown("+speed+")");
		if(oldSpeed!=0)
			speedMove = oldSpeed;
		StopCoroutine("CancelSpeedDown");
		isSpeedDown = true;
		timeSD = time;
		oldSpeed = speedMove;
		speedMove = speedMove - speed;
		StartCoroutine (CancelSpeedDown ());
	}
	IEnumerator CancelSpeedDown(){
		while(timeSD > 0){
			timeSD -= 1 * Time.deltaTime;
			yield return 0;
		}
		isSpeedDown = false;
		speedMove = oldSpeed;
		oldSpeed = 0;
	}

	/*
	//Magnet Item
	public void Magnet(float time){
		StopCoroutine("CancleMagnet");
		magnet.SetActive(true);
		timeMagnet = time;
		StartCoroutine(CancleMagnet());
	}
			
	IEnumerator CancleMagnet(){
		while(timeMagnet > 0){
			timeMagnet -= 1 * Time.deltaTime;
			yield return 0;
		}
		magnet.SetActive(false);
	}
	
	//Double jump Item
	public void JumpDouble(float time){
		StopCoroutine("CancleJumpDouble");
		isDoubleJump = true;
		timeJump = time;
		StartCoroutine(CancleJumpDouble());
	}
	
	IEnumerator CancleJumpDouble(){
		while(timeJump > 0){
			timeJump -= 1 * Time.deltaTime;
			yield return 0;
		}
		isDoubleJump = false;
	}
	
	//Multiply Item
	public void Multiply(float time){
		StopCoroutine("CancleMultiply");
		isMultiply = true;
		timeMultiply = time;
		StartCoroutine(CancleMultiply());
	}
	
	IEnumerator CancleMultiply(){
		while(timeMultiply > 0){
			timeMultiply -= 1 * Time.deltaTime;
			yield return 0;
		}
		isMultiply = false;
	}
	*/
}
