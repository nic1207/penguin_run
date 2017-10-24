using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishTrigger : MonoBehaviour {
	//public GameObject fishObj;
	public Transform tr;
    // Use this for initialization

    //private const int RANDOM_RATE = 1;       // 魚產生的機率, 4表示1/4, 1表示1/1
    //private static System.Random randomFish = new System.Random();
    //private static int fishIdx = 0;    
	//private static int colorIdx = 0

    void Start () {
        //Debug.Log ("aaaa");
        //randomFish = new System.Random();
    }

    // Update is called once per frame
    void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log ("xxx "+col.tag);
		if (col.tag == "Player") {
            if (!col.GetComponent<Controller>().flying && (RandomVariable.getRandomFish() == 0))
            {
				//GameObject fish = GameObject.Find("Fish[" + colorIdx + "][" + fishIdx + "]");
                //fishIdx = (fishIdx + 1) % FishPool.POOL_SIZE;
				GameObject fish  = FishPool.Instance.getFish();
                float dir = RandomVariable.getRandomValue(0, 2);
                fish.transform.position = tr.position;
                Rigidbody rb = fish.GetComponent<Rigidbody>();
                if (tr.position.x < -1)     // 洞在左側
                {
                    if (dir == 1)
                        dir = 8;
                    else if (dir == 2)
                        dir = 18f;
                    else
                        dir = 0;
                }
                else if (tr.position.x > 1) // 洞在右側
                {
                    if (dir == 1)
                        dir = -8;
                    else if (dir == 2)
                        dir = 0;
                    else
                        dir = -18f;
                }
                else                        // 洞在中間
                {
                    if (dir == 1)
                        dir = 0;
                    else if (dir == 2)
                        dir = 8;
                    else
                        dir = -8;
                }
                rb.AddForce(new Vector3(dir, 26, -15), ForceMode.Impulse);
                rb.isKinematic = false;
                rb.useGravity = true;

                StartCoroutine(DestroySelf(rb, dir));

            }
        }
	}

    IEnumerator DestroySelf(Rigidbody rb, float x)
    {
        //play your sound
        yield return new WaitForSeconds(3); //waits 3 seconds

        rb.AddForce(new Vector3(-x, +25, 15), ForceMode.Impulse);
        rb.useGravity = false;
        rb.isKinematic = true;
        StartCoroutine(DisableKinematic(rb));
    }

    IEnumerator DisableKinematic(Rigidbody rb) {
        yield return new WaitForSeconds(0.1f); //waits 3 seconds
        rb.isKinematic = false; 
    }
}
