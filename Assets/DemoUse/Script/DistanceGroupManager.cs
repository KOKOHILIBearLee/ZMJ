using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistanceGroupManager : DistanceTrigerManager
{
    public List<GameObject> triggerGame;
    public List<string> function;
    public float distance;
    public bool once = false;
    List<DistanceParas> distancepars = new List<DistanceParas>();

    public static DistanceGroupManager sin;

    private void Awake()
    {
        sin = this;
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if (once)
        {
            once = false;
            Ini();
        }

        for (int i = 0; i < distancepars.Count; i++)
            distancepars[i].triggeredGame = Camera.main.gameObject;
    }

    void Ini()
    {
        distancepars = new List<DistanceParas>();
        for (int i = 0; i < triggerGame.Count; i++)
        {
            List<string> functiont = new List<string>();
            functiont.Add("ShowModelZMJ," + triggerGame[i].name);
            try
            {
                string[] strs = function[i].Split('/');
                for (int ii = 0; ii < strs.Length; ii++)
                    functiont.Add(strs[ii]);
            }
            catch (System.Exception e)
            {
                Debug.LogError("函数数量不匹配");
            }
            distancepars.Add(new DistanceParas(triggerGame[i], Camera.main.gameObject, distance, functiont.ToArray(), new string[] { "HideModelZMJ," + triggerGame[i].name }));
        }
        for (int i = 0; i < distancepars.Count; i++)
        {
            DistanceTrigerManager.singleton.Add(distancepars[i]);
        }
    }
}
