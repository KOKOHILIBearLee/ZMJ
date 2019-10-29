using UnityEngine;
using System.Collections;

public class FlowMachine : MonoBehaviour {

    public GameObject root;
    public string objName = "钻头底座004";

    static public bool bFlow = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (bFlow)
        {
            if (root)
            {
                GameObject fl = GameObject.Find(objName);
                if (fl )
                {
                    Vector3 pos = fl.transform.position;
                    pos.x = fl.transform.position.x;
                    pos.y = fl.transform.position.y;

                    transform.position = pos;
                }
                else
                {
                    Debug.Log("跟随错误！！  " + objName);
                }
            }
        }
    }
}
