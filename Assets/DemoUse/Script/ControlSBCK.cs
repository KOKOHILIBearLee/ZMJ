using UnityEngine;
using System.Collections;

public class ControlSBCK : MonoBehaviour
{
    public SteamVR_TrackedObject Hand;
    SteamVR_Controller.Device device;
    public static ControlSBCK singleton;
    public static AnimationControl leftarm;
    public static AnimationControl rightarm;
    public AnimationControl animation;

    public static void AddAnimationControl()
    {
        try
        {
            leftarm = GameObject.Find("SceneRoot").transform.Find("ModelRoot/CMJ1/CMJ/钻头机械臂001").gameObject.AddComponent<AnimationControl>();
            rightarm = GameObject.Find("SceneRoot").transform.Find("ModelRoot/CMJ1/CMJ/钻头机械臂002").gameObject.AddComponent<AnimationControl>();
        }
        catch
        {
            Debug.LogError("添加AnimationControl时报错");
        }
    }

    private void Awake()
    {
        singleton = this;
    }

    float starttime = 0;
    bool shake;

    private void Update()
    {
        if (shake)
        {
            if ((starttime += Time.deltaTime) <= 1)
            {
                if (device == null)
                    device = SteamVR_Controller.Input((int)Hand.index);
                if (device != null)
                    device.TriggerHapticPulse(1000);
            }
            else
            {
                shake = false;
                starttime = 0;
            }
        }
    }

    void Shake()
    {
        shake = true;
        starttime = 0;
        //Move_LT.MouseLeftDown


    }

    private void OnTriggerEnter(Collider other)
    {
        OnOffContorl target = other.GetComponent<OnOffContorl>();
        if (HandControler.singleton.hold)
            switch (other.gameObject.name)
            {
                case "CMJ_Switch_L":
                    TriCMJ_Switch_L(target);
                    break;
                case "CMJ_Switch_R":
                    TriCMJ_Switch_R(target);
                    break;
                case "CMJ_Switch_Move":
                    TriCMJ_Switch_Move(target);
                    break;
                case "ZJ_Switch_1H":
                    TriZJ_Switch_1H(target);
                    break;
                case "ZJ_Switch_2H":
                    TriZJ_Switch_2H(target);
                    break;
                case "ZJ_Switch_SSL":
                    TriZJ_Switch_SSL(target);
                    break;
                case "ZJ_Switch_ZZ":
                    TriZJ_Switch_ZZ(target);
                    break;
                case "CMJ_ZQ":
                    TriCMJ_ZQ();
                    break;
                case "CMJ_ZT":
                    TriCMJ_ZT();
                    break;

                case "SQ_KZZX":
                    YiJQD_TriSQ_KZZX(other.gameObject);
                    break;

                case "YJQD_KZZX":
                    YiJQD_TriYJQD_KZZX(other.gameObject);
                    break;

                case "SQ_GZM":
                    YiJQD_TriSQ_GZM(other.gameObject);
                    break;

                case "HanH_KZZX":
                    YiJQD_TriHanH_KZZX(other.gameObject);
                    break;

                case "HanH_GZM":
                    YiJQD_TriHanH_GZM(other.gameObject);
                    break;
            }
    }

    /// <summary>
    /// 采煤机左臂
    /// </summary>
    /// <param name="target"></param>
    void TriCMJ_Switch_L(OnOffContorl target)
    {
        if (target == null)
            return;
        Debug.LogError("点击：" + "CMJ_Switch_L");
        target.IsUp = !target.IsUp;
        if (leftarm == null)
            return;

        if (target.IsUp)
            leftarm.SetReversePlay();
        else
            leftarm.SetOrderPlay();

        Shake();
    }

    /// <summary>
    /// 采煤机右臂
    /// </summary>
    /// <param name="target"></param>
    void TriCMJ_Switch_R(OnOffContorl target)
    {
        if (target == null)
            return;
        Debug.LogError("点击：" + "CMJ_Switch_R");
        target.IsUp = !target.IsUp;

        if (rightarm == null)
            return;

        if (!target.IsUp)
            rightarm.SetReversePlay();
        else
            rightarm.SetOrderPlay();

        Shake();
    }

    /// <summary>
    /// 采煤机移动
    /// </summary>
    /// <param name="target"></param>
    void TriCMJ_Switch_Move(OnOffContorl target)
    {
        if (target == null)
            return;
        Debug.LogError("点击：" + "CMJ_Switch_Move:" + target.IsUp);
        target.IsUp = !target.IsUp;
        if (!target.IsUp)
        {
            Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,Move_LT.MouseLeftDown");
        }
        else
        {
            Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,Move_RT.MouseLeftDown");
        }
        Shake();
    }

    bool SSmallestOver = true;
    bool SSmallest_Small = true;
    int SSmallCurrentState = 1;
    int SSmallest_SmallCurrentState = -1;
    /// <summary>
    /// 中小护板
    /// </summary>
    /// <param name="target"></param>
    void TriZJ_Switch_1H(OnOffContorl target)
    {
        if (target == null)
            return;
        //ZJ_1Hup.MouseLeftDown  ZJ_1Hdn.MouseLeftDown
        Debug.LogError("点击：" + "ZJ_Switch_1H");

        if (SmallestCurrentState == 1)
            return;

        if (!SmallestOver || !SSmallestOver || !MaxOver || !SSmallest_Small)
            return;
        SSmallestOver = false;
        SSmallest_Small = false;

        target.IsUp = !target.IsUp;

        if (target.IsUp)
        {
            SSmallCurrentState = 1;
            Debug.LogError("SSmall:1");
            Machine.singleton.MecBindings["SSmallest3_SSmallest2"].PlayAnimatin(3, 0.2f).AnimationAction(() =>
            {
                SSmallestOver = true;
            });  //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_1Hup.MouseLeftDown");


            SSmallest_SmallCurrentState = -1;
            Machine.singleton.MecBindings["Smallest3_Smallest2"].PlayAnimatin(2, -0.2f).AnimationAction(() =>
            {
                SSmallest_Small = true;
            });  //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_2Hdn.MouseLeftDown");

        }
        else
        {
            SSmallCurrentState = -1;
            Machine.singleton.MecBindings["SSmallest3_SSmallest2"].PlayAnimatin(3, -0.2f).AnimationAction(() =>
            {
                SSmallestOver = true;
            }); //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_1Hdn.MouseLeftDown");

            SSmallest_SmallCurrentState = 1;

            Machine.singleton.MecBindings["Smallest3_Smallest2"].PlayAnimatin(2, 0.2f).AnimationAction(() =>
            {
                SSmallest_Small = true;
            }); //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_2Hup.MouseLeftDown");
        }
        Shake();
    }

    bool SmallestOver = true;
    int SmallestCurrentState = -1;
    /// <summary>
    /// 最小护板
    /// </summary>
    /// <param name="target"></param>
    void TriZJ_Switch_2H(OnOffContorl target)
    {
        if (target == null)
            return;
        //ZJ_2Hup.MouseLeftDown  ZJ_2Hdn.MouseLeftDown
        Debug.LogError("点击：" + "ZJ_Switch_2H");

        if (!SmallestOver || !SSmallestOver || !MaxOver || !SSmallest_Small)
            return;

        if (SSmallCurrentState == 1)
            return;

        SmallestOver = false;
        target.IsUp = !target.IsUp;

        if (target.IsUp)
        {
            SmallestCurrentState = 1;
            Debug.LogError("Small:1");
            Machine.singleton.MecBindings["Smallest3_Smallest2"].PlayAnimatin(2, 0.3f).AnimationAction(() =>
            {
                SmallestOver = true;
            }); //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_2Hup.MouseLeftDown");
        }
        else
        {
            SmallestCurrentState = -1;
            Machine.singleton.MecBindings["Smallest3_Smallest2"].PlayAnimatin(2, -0.3f).AnimationAction(() =>
            {
                SmallestOver = true;
            });  //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_2Hdn.MouseLeftDown");
        }
        Shake();
    }

    /// <summary>
    /// 伸缩梁
    /// </summary>
    /// <param name="target"></param>
    void TriZJ_Switch_SSL(OnOffContorl target)
    {
        //ZJ_SSdn.MouseLeftDown  ZJ_SSup.MouseLeftDown
        Debug.LogError("点击：" + "ZJ_Switch_SSL");

        if (animation == null || animation.IsPlaying())
            return;

        target.IsUp = !target.IsUp;
        if (target.IsUp)
            animation.SetOrderPlay();//Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_SSdn.MouseLeftDown");
        else
            animation.SetReversePlay();//Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,ZJ_SSup.MouseLeftDown");

        Shake();
    }

    bool MaxOver = true;
    int MaxCurrentState = -1;
    /// <summary>
    /// 最大护板
    /// </summary>
    /// <param name="target"></param>
    void TriZJ_Switch_ZZ(OnOffContorl target)
    {
        if (target == null)
            return;
        // HB_Shen.MouseLeftDown HB_Shou.MouseLeftDown
        Debug.LogError("点击：" + "ZJ_Switch_ZZ");

        if (!SmallestOver || !SSmallestOver || !MaxOver || !SSmallest_Small)
            return;
        MaxOver = false;

        target.IsUp = !target.IsUp;

        if (target.IsUp)
        {
            Debug.LogError("Max:1");
            MaxCurrentState = 1;
            Machine.singleton.MecBindings["Max3_Max2"].PlayAnimatin(2, 0.2f).AnimationAction(() =>
            {
                MaxOver = true;
            }); //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,HB_Shen.MouseLeftDown");
        }
        else
        {
            MaxCurrentState = -1;
            Machine.singleton.MecBindings["Max3_Max2"].PlayAnimatin(2, -0.2f).AnimationAction(() =>
            {
                MaxOver = true;
            }); //Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,HB_Shou.MouseLeftDown");
        }
        Shake();
    }

    /*
    Machine.singleton.MecBindings["Max3_Max2"].PlayAnimatin(time / 1000, step);
    Machine.singleton.MecBindings["SSmallest3_SSmallest2"].PlayAnimatin(time / 1000, step);
    Machine.singleton.MecBindings["Smallest3_Smallest2"].PlayAnimatin(time / 1000, step);      
    */

    void TriCMJ_ZQ()
    {
        Debug.LogError("点击：TriCMJ_ZQ");
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,CMJ_ZQ.MouseLeftDown");
        Shake();
    }

    void TriCMJ_ZT()
    {
        Debug.LogError("点击：TriCMJ_ZT");
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,CMJ_ZT.MouseLeftDown");
        Shake();
    }

    void YiJQD_TriSQ_KZZX(GameObject target)
    {
        Debug.LogError("点击：SQ_KZZX");
        OffModelEffect();
        Training.TrainingManager.ExecuteScript("HideGameByOrder,Y");
        SOManager.singleton.RecoveryCameraRig();
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,SQ_KZZX.MouseLeftDown");
        Shake();
    }

    void YiJQD_TriYJQD_KZZX(GameObject target)
    {
        Debug.LogError("点击：YJQD_KZZX");
        OffModelEffect();
        Training.TrainingManager.ExecuteScript("HideGameByOrder,Y");
        SOManager.singleton.EnlargeCameraRig();
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,YJQD_KZZX.MouseLeftDown");

        GameObject sce = GameObject.Find("SceneRoot");
        GameObject cam = GameObject.Find("[CameraRig]");
        if (sce != null)
        {
            Transform tran = sce.transform.Find("ModelRoot/XJTZ_ZHDT");
            cam.transform.position = tran.position;
        }

        Shake();
    }

    void YiJQD_TriSQ_GZM(GameObject target)
    {
        Debug.LogError("点击：SQ_GZM");
        OffModelEffect();
        Training.TrainingManager.ExecuteScript("HideGameByOrder,Y");
        SOManager.singleton.EnlargeCameraRig();
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,SQ_GZM.MouseLeftDown");
        Shake();
    }

    void YiJQD_TriHanH_KZZX(GameObject target)
    {
        Debug.LogError("点击：HanH_KZZX");
        OffModelEffect();
        Training.TrainingManager.ExecuteScript("HideGameByOrder,Y");
        SOManager.singleton.RecoveryCameraRig();
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,HanH_KZZX.MouseLeftDown");
        Shake();
    }

    void YiJQD_TriHanH_GZM(GameObject target)
    {
        Debug.LogError("点击：HanH_GZM");
        OffModelEffect();
        Training.TrainingManager.ExecuteScript("HideGameByOrder,Y");
        SOManager.singleton.EnlargeCameraRig();
        Training.TrainingManager.ExecuteScript("ExecuteFunctionPreTranslate,HanH_GZM.MouseLeftDown");
        Shake();
    }

    public static void OffModelEffect()
    {
        Training.TrainingManager.ExecuteScript("ModelEffect,SQ_KZZX,1,2,1,10000");
        Training.TrainingManager.ExecuteScript("ModelEffect,YJQD_KZZX,1,2,1,10000");
        Training.TrainingManager.ExecuteScript("ModelEffect,SQ_GZM,1,2,1,10000");
        Training.TrainingManager.ExecuteScript("ModelEffect,HanH_KZZX,1,2,1,10000");
        Training.TrainingManager.ExecuteScript("ModelEffect,HanH_GZM,1,2,1,10000");
    }

    static void InitilizeAnimationState()
    {
        if (singleton == null)
            return;

        singleton.MaxOver = true;
        singleton.MaxCurrentState = -1;
        singleton.SSmallest_SmallCurrentState = -1;
        singleton.SmallestOver = true;
        singleton.SmallestCurrentState = -1;
        singleton.SSmallest_Small = true;
        singleton.SSmallestOver = true;
        singleton.SSmallCurrentState = 1;
    }

    void Test()
    {
        //Machine.singleton.MecBindings["SSmallest3_SSmallest2"].PlayAnimatin(time / 1000, step);
    }

    public static void InitilizeAllOnoff()
    {
        InitilizeAnimationState();
        OnOffContorl[] contorls = GameObject.FindObjectsOfType<OnOffContorl>();
        Debug.LogError("初始化OnOff动画");
        for (int i = 0; i < contorls.Length; i++)
        {
            switch (contorls[i].gameObject.name)
            {
                case "CMJ_Switch_L":
                    contorls[i].IsUp = true;
                    break;
                case "CMJ_Switch_R":
                    contorls[i].IsUp = false;
                    break;
                case "CMJ_Switch_Move":
                    contorls[i].IsUp = false;
                    //Debug.LogError("初始化CMJ_Switch_Move动画");
                    break;
                case "ZJ_Switch_1H":
                    contorls[i].IsUp = true;
                    break;
                case "ZJ_Switch_2H":
                    contorls[i].IsUp = false;
                    break;
                case "ZJ_Switch_SSL":
                    contorls[i].IsUp = false;
                    break;
                case "ZJ_Switch_ZZ":
                    contorls[i].IsUp = false;
                    break;
            }
        }
    }
}
