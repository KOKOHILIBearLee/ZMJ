using UnityEngine;
using System.Collections;
using VRTK;
//using Training;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class LeftAction : MonoBehaviour
{

    private GameObject m_selectObj = null;
    private RaycastHit objHit;
    private Ray _ray;

    Canvas m_cavans;

    //一级菜单
    TouchPadChangUI m_UI;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }

        //Setup controller event listeners
        //GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        //GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripReleased);
        m_cavans = Transform.FindObjectOfType<Canvas>();
        //		if (m_cavans) {
        //			RectTransform rec = m_cavans.gameObject.GetComponent<RectTransform> ();
        //			rec.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
        //			
        //		}


    }

    public bool NewVersion = true;

    // Update is called once per frame
    void Update()
    {
        m_UI = UIMgr.mgr.m_ui;
    }



    void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("tr press");

    }

    void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (m_selectObj)
        {
            //TrainingManager.ExecuteScript("TriggerFire,MouseLeftDown," + m_selectObj.name);
        }

    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {

    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        if (NewVersion)
        {
            if (UIMgr.mgr.sbczUI)
                UIMgr.mgr.sbczUI.SetActive(true);
        }
        else
        {
            if (m_UI)
            {
                m_UI.transform.gameObject.SetActive(true);
            }
        }
        Training.TrainingManager.ExecuteScript("HideModelZMJ,CMJ_YKQ");
        Training.TrainingManager.ExecuteScript("HideColider,CMJ_YKQ");
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        //		DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
        if (NewVersion)
        {
            if (UIMgr.mgr.sbczUI)
                UIMgr.mgr.sbczUI.SetActive(false);
        }
        else
        {
            if (m_UI)
                m_UI.transform.gameObject.SetActive(false);
        }
    }
    public static bool uncontrol = true;
    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {

        if (SOManager.singleton.currSceneName == "ZNCM" && !SOManager.singleton.isLoading)
        {
            Cutter2DAnimation.singleton.Visible(!Cutter2DAnimation.singleton.GetVisible(), null, false);
        }
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        if (NewVersion)
        {
            if (UIMgr.mgr.sbczUI)
                UIMgr.mgr.sbczUI.GetComponent<TouchPadChangUI>().ChangeUI((uint)e.touchpadAngle);
        }
        else
        {
            if (m_UI)
            {
                try
                {
                    m_UI.ChangeUI((uint)e.touchpadAngle);
                }
                catch (System.Exception r) { }
            }
        }
    }


    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (NewVersion)
        {
            if (UIMgr.mgr.sbczUI)
                UIMgr.mgr.sbczUI.GetComponent<TouchPadChangUI>().OnTouchPadPress((uint)e.touchpadAngle);
        }
        else
        {

            if (m_UI)
                m_UI.OnTouchPadPress((uint)e.touchpadAngle);
        }
    }

}
