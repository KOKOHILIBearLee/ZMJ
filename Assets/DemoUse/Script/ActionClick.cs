using UnityEngine;
using System.Collections;
using VRTK;
using Training;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class ActionClick : MonoBehaviour {

	private GameObject m_selectObj = null;
	private RaycastHit objHit;
	private Ray _ray;

	// Use this for initialization
	void Start () {
		if (GetComponent<VRTK_ControllerEvents>() == null)
		{
			Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
			return;
		}

		//Setup controller event listeners
		GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
		GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

		//GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
		//GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

		GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }
	
	// Update is called once per frame
	void Update () {

		m_selectObj = null;
		_ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (_ray, out objHit, 1000f)) {
			m_selectObj = objHit.collider.gameObject;
			if (m_selectObj)
				DrawLine.bTarget = true;
			else
				DrawLine.bTarget = false;
		} 
	
	}

	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		//Debug.Log ("tr press");
		DrawLine.bDraw = true;
	}

	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
	{
		

		if (m_selectObj) {
			TrainingManager.ExecuteScript("TriggerFire,MouseLeftDown," + m_selectObj.name);
           // MonoBehaviour.print(m_selectObj.name);
		}
		DrawLine.bDraw = false;
	}

	private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
	{
		//Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
		//	+ " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
	}

	private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
	{
		//DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e);
	}

	private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
	{
		//DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
	}

	private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
	{
		//DebugLogger(e.controllerIndex, "TOUCHPAD", "axis changed", e);
	}


}
