using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingProgress : MonoBehaviour {

    public static LoadingProgress Instance = null;

    public GameObject owner;
    public Scrollbar progress;


    // Use this for initialization
    void Start () {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public float percentage {
        get {
            if (progress != null)
            {
                return progress.size * 100;
            }

            return -1;
        }

        set {
            if (progress != null)
            {
                progress.size = value / 100;
            }
        }
    }

    public void SetActive(bool active) {
        if (owner != null)
        {
            owner.SetActive(active);
        }
    }

}
