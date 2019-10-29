using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class secUIEventHandle : MonoBehaviour
{

    public List<GameObject> yjqdList = new List<GameObject>();

    public GameObject cameraRig;
    public GameObject ZTDZgameobject;
    public GameObject RYDWPosition;
    public GameObject FHDTPosition;
    public GameObject SLTGameobject;

    public static secUIEventHandle sin;
    TouchPadChangUI m_ui;

    // Use this for initialization
    void Awake()
    {
        sin = this;
        gameObject.SetActive(false);
        m_ui = GetComponent<TouchPadChangUI>();
        m_ui.m_eventList.Add(OnYDGJ);
        m_ui.m_eventList.Add(OnRYDW);
        m_ui.m_eventList.Add(OnFHZCD);
        m_ui.m_eventList.Add(OnZDTZ);
        m_ui.m_eventList.Add(OnJYGM);
        m_ui.m_eventList.Add(OnYJQD);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnYDGJ()
    {
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        Debug.Log("OnYDGJ");
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/YDGJ.tpvr,0");
    }

    public void OnRYDW()
    {
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        FlowMachine.bFlow = false;
        if (cameraRig != null && RYDWPosition != null)
        {
            cameraRig.transform.position = RYDWPosition.transform.position;
        }
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/RYDW.tpvr,0");
        Debug.Log("OnRYDW");
    }

    public void OnFHZCD()
    {
        Debug.Log("OnFHZCD");
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        UIMgr.mgr.m_ui = UIMgr.mgr.mainUI.GetComponent<TouchPadChangUI>();
        UIMgr.mgr.m_ui.gameObject.SetActive(true);
        gameObject.SetActive(false);
        FlowMachine.bFlow = false;

    }

    public void OnZDTZ()
    {
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        Debug.Log("OnZDTZ");
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/ZNCM.tpvr,0");
        if (cameraRig != null && ZTDZgameobject != null)
            cameraRig.transform.position = ZTDZgameobject.transform.position;
        mainUIEventHandle.SetZHDT_XZCFColoder(false);
    }

    public void OnJYGM()
    {
        mainUIEventHandle.VrCamera(false);
        if (SLTGameobject != null)
            SLTGameobject.SetActive(true);
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/JiYGM.tpvr,0");
        FlowMachine f = Object.FindObjectOfType<FlowMachine>();
        if (f != null)
        {
            f.objName = "JiYGM_XJ";
            FlowMachine.bFlow = true;
        }

        Debug.Log("OnJYGM");
    }



    public void OnYJQD()
    {
        if (mainUIEventHandle.IsShow)
        {
            if (SLTGameobject != null)
                SLTGameobject.SetActive(true);
        }
        mainUIEventHandle.SetZHDT_XZCFColoder(true);
        Debug.Log("OnYJQD");
        Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/YiJQD.tpvr,0");
        StartCoroutine(Drump());
    }

    IEnumerator Drump()
    {
        foreach (GameObject ob in yjqdList)
        {
            if (cameraRig)
            {
                //  cameraRig.transform.position = ob.transform.position;
                yield return new WaitForSeconds(4);
            }
        }
        Debug.Log("This message appears after 3 seconds!");
        yield return null;

    }
}
