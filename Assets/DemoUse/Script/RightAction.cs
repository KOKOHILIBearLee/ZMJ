using UnityEngine;
using System.Collections;
using VRTK;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class RightAction : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            //Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripReleased);
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        print("按下角度为：" + e.touchpadAngle + ":" + e.touchpadAxis);
        if (e.touchpadAngle < 45 || e.touchpadAngle >= 315)//up
        {
            OperateBoard.singleton.OnUp();
        }

        if (e.touchpadAngle >= 45 && e.touchpadAngle < 135)//right
        {
            OperateBoard.singleton.OnRight();
        }

        if (e.touchpadAngle >= 135 && e.touchpadAngle < 225)//down
        {
            OperateBoard.singleton.OnDown();
        }

        if (e.touchpadAngle >= 225 && e.touchpadAngle < 315)//left
        {
            OperateBoard.singleton.OnLeft();
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        HandControler.singleton.hold = false;
        print("扳机释放");
        OperateBoard.singleton.OnEvent();
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        HandControler.singleton.hold = true;
        print("扳机释放");
        OperateBoard.singleton.OnEvent();
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        print("Grip按下");
        OperateBoard.singleton.SetVisible(!OperateBoard.singleton.GetVisible());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
