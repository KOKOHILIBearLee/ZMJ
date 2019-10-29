using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour
{
    public Button[] uiBtns;
    // Use this for initialization
    public static DemoUI singleton = null;

    void Awake()
    {
        AddListener();
        singleton = this;
    }

    public void AddListener()
    {
        for (int i = 0; i < uiBtns.Length; i++)
        {
            uiBtns[i].gameObject.AddComponent<ListenMouseDownUp>();
        }
    }

    public bool CreatMapFor3D()
    {
        GameObject modelroot = GameObject.Find("ModelRoot");
        GameObject uiroot = GameObject.Find("2DUIMapRoot");
        if (modelroot == null)
            return false;
        if (uiroot == null || uiroot.transform.parent != modelroot.transform)
            uiroot = new GameObject("2DUIMapRoot");
        uiroot.transform.parent = modelroot.transform;

        for (int i = 0; i < uiBtns.Length; i++)
        {
            bool on = false;
            for (int k = 0; k < uiroot.transform.childCount; k++)
            {
                Transform tranchild = uiroot.transform.GetChild(k);
                if (tranchild.gameObject.name == uiBtns[i].gameObject.name)
                    on = true;
            }
            if (!on)
            {
                GameObject gamechild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gamechild.GetComponent<Renderer>().enabled = false;
                gamechild.GetComponent<BoxCollider>().enabled = false;
                gamechild.gameObject.name = uiBtns[i].gameObject.name;
                gamechild.transform.parent = uiroot.transform;
            }
            else
            {
                //可以删除不应该存在的节点
            }
        }

        return true;
    }
}
