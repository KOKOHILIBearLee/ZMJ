using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistanceTrigerManager : MonoBehaviour
{
    List<DistanceParas> Triggerset = new List<DistanceParas>();
    public static DistanceTrigerManager singleton = null;
    private void Awake()
    {
        singleton = this;
    }

    void Update()
    {
        for (int i = 0; i < Triggerset.Count; i++)
        {
            Triggerset[i].Update();
        }
    }

    public void Add(DistanceParas pa)
    {
        Triggerset.Add(pa);
    }

    public void Initialize()
    {
        Triggerset = new List<DistanceParas>();
    }

    public class DistanceParas
    {
        public GameObject triggerGame;
        public GameObject triggeredGame;
        bool Into = false;
        bool onceinto = true;
        bool Leave = false;
        bool onceleave = true;
        public float triggerdsitance = 0;

        public List<string> intoFunction;
        public List<string> leaveFunction;


        void Initilize()
        {
            intoFunction = new List<string>();
            leaveFunction = new List<string>();
        }

        public void Update()
        {
            try
            {
                if (((triggeredGame.transform.position - triggerGame.transform.position).magnitude <= triggerdsitance) && onceinto)
                {
                    onceinto = false;
                    onceleave = true;
                    Into = true;
                    Leave = false;
                    string str = "";
                    for (int i = 0; i < intoFunction.Count; i++)
                    {
                        Training.TrainingManager.ExecuteScript(intoFunction[i]);
                        str += i.ToString() + ":" + intoFunction[i] + "     ";
                    }
                    MonoBehaviour.print("主摄像机进入物体" + triggerGame.name + "触发距离" + triggerdsitance + "触发函数:" + str);
                    //"ExecuteFunctionPreTranslate," +
                }

                if (/*Into &&*/ ((triggeredGame.transform.position - triggerGame.transform.position).magnitude > triggerdsitance) && onceleave)
                {
                    onceinto = true;
                    onceleave = false;
                    Into = false;
                    string str = "";
                    for (int i = 0; i < leaveFunction.Count; i++)
                    {
                        Training.TrainingManager.ExecuteScript(leaveFunction[i]);
                        str += i.ToString() + ":" + leaveFunction[i] + "     ";
                    }
                    MonoBehaviour.print("主摄像机离开物体" + triggerGame.name + "触发距离" + triggerdsitance + "触发函数:" + str);
                }
            }
            catch (System.Exception e) { }
        }

        public DistanceParas(GameObject trigger, GameObject triggered, float distance, string intof, string leavef)
        {
            Initilize();
            triggerGame = trigger;
            triggeredGame = triggered;
            triggerdsitance = distance;
            if (!(intof == null || intof == ""))
                intoFunction.Add(intof);
            else
                Debug.LogError("加载距离碰撞函数时函数非法");
            if (!(leavef == null || leavef == ""))
                leaveFunction.Add(leavef);
            else
                Debug.LogError("加载距离碰撞函数时函数非法");
            MonoBehaviour.print("触发创建成功");
        }

        public DistanceParas(GameObject trigger, GameObject triggered, float distance, string[] intof, string[] leavef)
        {
            Initilize();
            triggerGame = trigger;
            triggeredGame = triggered;
            triggerdsitance = distance;
            for (int i = 0; i < intof.Length; i++)
            {
                if (!(intof[i] == null || intof[i] == ""))
                    intoFunction.Add(intof[i]);
                else
                    Debug.LogError("加载距离碰撞函数时函数非法");
            }

            for (int i = 0; i < leavef.Length; i++)
            {
                if (!(leavef[i] == null || leavef[i] == ""))
                    leaveFunction.Add(leavef[i]);
                else
                    Debug.LogError("加载距离碰撞函数时函数非法");
            }
            MonoBehaviour.print("触发创建成功");
        }
    }
}
