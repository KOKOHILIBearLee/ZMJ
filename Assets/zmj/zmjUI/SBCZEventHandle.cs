using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SBCZEventHandle : MonoBehaviour
{

    TouchPadChangUI m_ui;

    public Vector3 ro;
    public Vector3 offset = new Vector3();  //偏置值

    //井下初始位置

    public float v;

    Transform zj_ui;
    Transform cmj_ui;
    GameObject cam_eye;
    // Use this for initialization
    void Start()
    {
        m_ui = GetComponent<TouchPadChangUI>();
        //m_ui.m_eventList.Add(OnFHZCD);
        m_ui.m_eventList.Add(OnFHDT);
        m_ui.m_eventList.Add(OnZJ);
        m_ui.m_eventList.Add(OnCMJ);
        cam_eye = GameObject.Find("Camera (eye)");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCMJ()
    {
        Debug.Log("OnCMJ");

        SwitchLeftHand.singlton.ShowCMJPla();
        //zj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("ZJ_UI");
        //cmj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("CMJ_UI");


        //if (cmj_ui)
        //{
        //    //cmj_ui.transform.position = (cam_eye.transform.position + cam_eye.transform.forward * 0.3f + offset) + cam_eye.transform.forward * v;
        //   // Vector3 eu = cam_eye.transform.eulerAngles;

        //    //cmj_ui.transform.eulerAngles = eu;
        //   // cmj_ui.transform.Rotate(ro);
        //    cmj_ui.gameObject.SetActive(true);
        //    zj_ui.gameObject.SetActive(false);
        //}

    }

    void OnZJ()
    {
        Debug.Log("OnZJ");
        SwitchLeftHand.singlton.ShowZJPla();
        //zj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("ZJ_UI");
        //cmj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("CMJ_UI");


        //if (zj_ui)
        //{

        //    //zj_ui.transform.position = cam_eye.transform.position + cam_eye.transform.forward * 0.3f + offset + cam_eye.transform.forward * v;
        //   // Vector3 eu = cam_eye.transform.eulerAngles;

        //    //zj_ui.transform.eulerAngles = eu;
        //    //zj_ui.transform.Rotate(ro);
        //    cmj_ui.gameObject.SetActive(false);
        //    zj_ui.gameObject.SetActive(true);
        //}
    }

    void OnFHZCD()
    {
        Debug.Log("OnFHZCD");

        SwitchLeftHand.singlton.ShowLeftJoy();
        //zj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("ZJ_UI");
        //cmj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("CMJ_UI");


        //cmj_ui.gameObject.SetActive(false);
        //zj_ui.gameObject.SetActive(false);


        UIMgr.mgr.m_ui = UIMgr.mgr.mainUI.GetComponent<TouchPadChangUI>();
        UIMgr.mgr.m_ui.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    //SOManager.singleton.LoadScene(filePath);
    void OnFHDT()
    {
        SwitchLeftHand.singlton.ShowLeftJoy();

        //zj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("ZJ_UI");
        //cmj_ui = GameObject.Find("[CameraRig]/Controller (left)/SBCK").transform.Find("CMJ_UI");


        //cmj_ui.gameObject.SetActive(false);
        //zj_ui.gameObject.SetActive(false);

        mainUIEventHandle.VrCamera(false);
        //VrCamera(false);
        if (mainUIEventHandle.sin.cameraRig && mainUIEventHandle.sin.zncmPos)
        {
            mainUIEventHandle.sin.cameraRig.transform.position = mainUIEventHandle.sin.dtPOS.transform.position;
            mainUIEventHandle.SetZHDT_XZCFColoder(true);
            Training.TrainingManager.ExecuteScript("LoadScene,./课程/科目1/场景/ZHDT.tpvr,0");
        }
    }
}
