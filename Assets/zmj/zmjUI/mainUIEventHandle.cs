using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TouchPadChangUI))]
public class mainUIEventHandle : MonoBehaviour
{

    public GameObject cameraRig;
    public GameObject jxmyPos;

    public GameObject zncmPos;

    public GameObject sbckPos;
    public GameObject dtPOS;
    public GameObject SLTGameobject;


    public static List<GameObject> showList = new List<GameObject>();
    public static bool IsShow = false;
    TouchPadChangUI m_ui;
    public static mainUIEventHandle sin;
    // Use this for initialization
    GameObject ZNCM_Root=null;
    GameObject root;

    private void Awake()
    {
        sin = this;
        this.gameObject.SetActive(false);
    }

    void Start()
    {

        m_ui = GetComponent<TouchPadChangUI>();
        m_ui.m_eventList.Add(OnZNCM);
        m_ui.m_eventList.Add(OnJXMY);
        m_ui.m_eventList.Add(OnSBCK);
        m_ui.m_eventList.Add(OnXJSYY);
        m_ui.m_eventList.Add(OnTCXT);
        m_ui.m_eventList.Add(ONFHDT);
        m_ui.m_eventList.Add(ONXSSLT);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnZNCM()
    {
        Debug.Log("OnZNCM");

        if (cameraRig && zncmPos)
        {
            //  cameraRig.transform.position = zncmPos.transform.position;
        }
        SetZHDT_XZCFColoder(false);
        //Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/ZNCM.tpvr,0");
        PopUI.bPop = false;
        if (SLTGameobject != null)
            SLTGameobject.SetActive(false);

        SOManager.singleton.currSceneName = "ZNCM_G";
        if (ZNCM_Root != null)
        {
            ZNCM_Root.SetActive(true);
        }
        else
        {
            ZNCM_Root = Resources.Load<GameObject>("Replace");
            ZNCM_Root = Instantiate(ZNCM_Root);
            root = GameObject.Find("SceneRoot");
        }

        for (int i = 0; i < root.transform.childCount; i++)
        {
            root.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void CloseZNCM()
    {
        if (ZNCM_Root!=null&& ZNCM_Root.activeSelf)
        {
            ZNCM_Root.SetActive(false);

            for (int i = 0; i < root.transform.childCount; i++)
            {
                root.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public static void SetZHDT_XZCFColoder(bool val)
    {
        SetColider(GameObject.Find("ZHDT_XZCF"), val);
        SetColider(GameObject.Find("GZM_SLT_CF"), val);
    }

    static void SetColider(GameObject game, bool val)
    {
        if (game == null)
            return;
        Collider[] col = game.GetComponents<Collider>();
        for (int i = 0; i < col.Length; i++)
            col[i].enabled = val;
    }

    public void OnJXMY()
    {
        Debug.Log("OnJXMY");

        if (cameraRig && jxmyPos)
        {
            cameraRig.transform.position = jxmyPos.transform.position;
        }
        SetZHDT_XZCFColoder(false);
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/JXMY.tpvr,0");
        PopUI.bPop = true;
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);

        CloseZNCM();
    }

    public void OnSBCK()
    {
        Debug.Log("OnSBCK");

        if (cameraRig && sbckPos)
        {
            cameraRig.transform.position = sbckPos.transform.position;
        }

        UIMgr.mgr.m_ui = UIMgr.mgr.sbczUI.GetComponent<TouchPadChangUI>();
        UIMgr.mgr.m_ui.gameObject.SetActive(true);
        gameObject.SetActive(false);
        SetZHDT_XZCFColoder(false);
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/Demo001.tpvr,0");
        PopUI.bPop = false;
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);

        CloseZNCM();
    }

    public void OnXJSYY()
    {
        Debug.Log("OnXJSYY");
        UIMgr.mgr.m_ui = UIMgr.mgr.secUI.GetComponent<TouchPadChangUI>();
        UIMgr.mgr.m_ui.gameObject.SetActive(true);
        gameObject.SetActive(false);
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        //  Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/YiJQD.tpvr,0");
        PopUI.bPop = false;

        CloseZNCM();
    }

    public void OnTCXT()
    {
        Debug.Log("OnTCXT");
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
    }

    public void ONFHDT()
    {
        VrCamera(false);
        //VrCamera(false);
        if (cameraRig && zncmPos)
        {
            cameraRig.transform.position = dtPOS.transform.position;
            SetZHDT_XZCFColoder(true);
            Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/ZHDT.tpvr,0");
        }
        //Animation animation = this.transform.parent.gameObject.GetComponent<Animation> ();
        //Animation animation = this.transform.parent.parent.gameObject.GetComponent<Animation> ();
        //animation.Play ();
        Debug.Log("ONFHDT");

        CloseZNCM();
    }

    public static void VrCamera(bool ena)
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayArea");
        if (player == null)
            return;
        VRTK.VRTK_HeightAdjustTeleport heights = player.GetComponent<VRTK.VRTK_HeightAdjustTeleport>();
        VRTK.VRTK_BodyPhysics physicses = player.GetComponent<VRTK.VRTK_BodyPhysics>();
        VRTK.VRTK_BasicTeleport basics = player.GetComponent<VRTK.VRTK_BasicTeleport>();

        if (heights != null)
            heights.enabled = ena;
        if (physicses != null)
            physicses.enabled = ena;
        if (basics != null)
            basics.enabled = !ena;
    }

    void ShowSLT()
    {

    }

    public static void ShowList()
    {
        for (int i = 0; i < showList.Count; i++)
        {
            try
            {
                showList[i].SetActive(true);
            }
            catch (System.Exception e) { }
        }
        showList = new List<GameObject>();
    }

    public static void HideGame()
    {
        GameObject game1 = GameObject.Find("ChuFaBox");
        GameObject game2 = GameObject.Find("CMJ");
        GameObject game3 = GameObject.Find("SL001Pre");
        GameObject game4 = GameObject.Find("RenYuanWeiZhi");
        if (game1)
        {
            game1.SetActive(false);
            showList.Add(game1);
        }
        if (game2)
        {
            game2.SetActive(false);
            showList.Add(game2);
        }
        if (game3)
        {
            game3.SetActive(false);
            showList.Add(game3);
        }
        if (game4)
        {
            game4.SetActive(false);
            showList.Add(game4);
        }
    }

    public void ONXSSLT()
    {
        if (IsShow)
        {
            if (SLTGameobject.activeSelf)
            {
                SLTGameobject.SetActive(false);
                HideGame();
            }
            else
            {
                SLTGameobject.SetActive(true);
                ShowList();
            }
        }
        else
        {
            SLTGameobject.SetActive(true);
            ShowList();
        }
        if (SOManager.singleton.currSceneName != "ZHDT")
            Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/ZHDT.tpvr,0");
        Debug.Log("ONXSSLT");
    }
}
