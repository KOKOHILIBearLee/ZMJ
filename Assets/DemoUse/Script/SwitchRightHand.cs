using UnityEngine;
using System.Collections;

public class SwitchRightHand : MonoBehaviour
{
    public static SwitchRightHand singlton;
    // Use this for initialization
    public GameObject originalJoystick;
    public GameObject hand;

    public bool joy = false;
    public bool h = false;

    private void Awake()
    {
        singlton = this;
        //SwitchToJoystick();
    }

    public void SwitchToJoystick()
    {
        hand.SetActive(false);
        originalJoystick.SetActive(true);
    }

    public void SwitchToHand()
    {
        hand.SetActive(true);
        originalJoystick.SetActive(false);
    }


    private void Update()
    {
        if (joy)
        {
            joy = false;

            SwitchToJoystick();
        }

        if (h)
        {
            h = false;

            SwitchToHand();
        }
    }
}
