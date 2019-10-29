using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PopUI : MonoBehaviour {

	public List<string> collisionedObjList = new List<string> ();
	public List<GameObject> popUIList = new List<GameObject> ();
	public int popTime = 2000;

	public GameObject root;
	public float dis = 0;
	public Vector3 ro = new Vector3 ();

    static public bool bPop = false;
	//计时器
	Timer popTimer = new Timer();
	int pop = 0;
	// Use this for initialization


	void Start () {		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll)
	{
        if(bPop)
        {
            for (int i = 0; i < collisionedObjList.Count; i++)
            {

                popUIList[i].SetActive(false);
                if (collisionedObjList[i] == coll.collider.gameObject.name)
                {
                    popUIList[i].SetActive(true);

                    //popUIList[i].transform.position = (transform.position + transform.forward * 0.3f) + transform.forward * dis;
                    //Vector3 eu = transform.eulerAngles;

                    //popUIList[i].transform.eulerAngles = eu;
                    //popUIList[i].transform.Rotate(ro);                    
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (bPop)
        {
            for (int i = 0; i < collisionedObjList.Count; i++)
            {

                popUIList[i].SetActive(false);
                if (collisionedObjList[i] == collision.collider.gameObject.name)
                {
                    popUIList[i].SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bPop)
        {
            for (int i = 0; i < collisionedObjList.Count; i++)
            {

                popUIList[i].SetActive(false);
                if (collisionedObjList[i] == other.gameObject.name)
                {
                    popUIList[i].SetActive(true);

                    //popUIList[i].transform.position = (transform.position + transform.forward * 0.3f) + transform.forward * dis;
                    //Vector3 eu = transform.eulerAngles;

                    //popUIList[i].transform.eulerAngles = eu;
                    //popUIList[i].transform.Rotate(ro);                    
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (bPop)
        {
            for (int i = 0; i < collisionedObjList.Count; i++)
            {

                popUIList[i].SetActive(false);
                if (collisionedObjList[i] == other.gameObject.name)
                {
                    popUIList[i].SetActive(false);
                }
            }
        }
    }
}
