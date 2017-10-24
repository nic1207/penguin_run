using UnityEngine;
using System.Collections;

public class SharkMovement : MonoBehaviour
{
	private float m_moveSpeed = 5.0f;
	private float n_moveSpeed = 30f;
	private float moveSpeed = 0.5f;
	
	void Start()
	{
		
	}
	
	void Update ()
	{
		
		if (Input.GetKey("i"))
		{
			moveFast();
		}
		else if (Input.GetKey("j"))
		{
			moveLeft();
		}
		else if (Input.GetKey("l"))
		{
			moveRight();
		}
		else if (Input.GetKey("o"))
		{
			moveUp();
		}
		else if (Input.GetKey("p"))
		{
			moveDown();
		}
		else if (Input.GetKey("k"))
		{
			moveAttack1();
		}
		else if (Input.GetKey("m"))
		{
			moveAttack2();
		}
		else if (Input.GetKey("n"))
		{
			moveAttack3();
		}
		else 
		{
			moveSlow();
		}
		
	}

	public void moveFast()
	{
		transform.Translate(0.0f, 0.0f, +m_moveSpeed * Time.deltaTime);
		{
			GetComponent<Animation>().CrossFade("FastSwim", 0.2f);
		}
	}
	public void moveSlow()
	{
		transform.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);
		{
			GetComponent<Animation>().CrossFade("SlowSwim", 0.2f);
		}
	}
	public void moveRight()
	{
		transform.RotateAround(transform.position+(transform.right*2), Vector3.up, Time.deltaTime * n_moveSpeed);
		{
			GetComponent<Animation>().CrossFade("RT", 0.2f);
		}
	}
	public void moveLeft()
	{
		transform.RotateAround(transform.position-(transform.right*2), Vector3.down, Time.deltaTime * n_moveSpeed);
		{
			GetComponent<Animation>().CrossFade("LT", 0.2f);
		}
	}
	public void moveUp()
	{
		transform.Rotate (Vector3.right * Time.deltaTime * -m_moveSpeed);
		{
			GetComponent<Animation>().CrossFade("DiveOut", 0.2f);
		}
	}
	public void moveDown()
	{
		transform.Rotate (Vector3.left * Time.deltaTime * -m_moveSpeed);
		{
			GetComponent<Animation>().CrossFade("DiveIn", 0.2f);
		}
	}
	public void moveAttack1()
	{
		transform.Translate(0.0f, 0.0f, +m_moveSpeed * Time.deltaTime);
		{
			GetComponent<Animation>().CrossFade("Attack1", 0.2f);
		}
	}
	public void moveAttack2()
	{
		transform.Translate(0.0f, 0.0f, +m_moveSpeed * Time.deltaTime);
		{
			GetComponent<Animation>().CrossFade("Attack2", 0.2f);
		}
	}
	public void moveAttack3()
	{
		transform.Translate(0.0f, 0.0f, +m_moveSpeed * Time.deltaTime);
		{
			GetComponent<Animation>().CrossFade("Attack3", 0.2f);
		}
	}
	
}
