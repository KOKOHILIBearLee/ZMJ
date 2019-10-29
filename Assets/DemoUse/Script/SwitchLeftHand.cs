using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchLeftHand : MonoBehaviour
{
    public GameObject leftjoy;
    public GameObject cmjpla;
    public GameObject zjpla;
    public static SwitchLeftHand singlton;

    private void Awake()
    {
        singlton = this;
        ShowLeftJoy();
    }


    void HideAll()
    {
        leftjoy.SetActive(false);

        SetGameObjectVisible(cmjpla.transform, false);
        SetGameObjectVisible(zjpla.transform, false);
    }

    public static void SetGameObjectVisible(Transform tran, bool val)
    {
        if (tran == null)
            return;
        Renderer render = tran.GetComponent<Renderer>();
        Collider collider = tran.GetComponent<Collider>();
        if (render != null)
            render.enabled = val;
        if (collider != null)
            collider.enabled = val;

        foreach (Transform child in tran)
        {
            SetGameObjectVisible(child, val);
        }
    }

    public void ShowLeftJoy()
    {
        HideAll();
        leftjoy.SetActive(true);
    }

    public void ShowCMJPla()
    {
        HideAll();
        SetGameObjectVisible(cmjpla.transform, true);
    }

    public void ShowZJPla()
    {
        HideAll();
        SetGameObjectVisible(zjpla.transform, true);
    }
}
