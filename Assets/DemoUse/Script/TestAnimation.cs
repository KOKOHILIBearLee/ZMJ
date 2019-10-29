using UnityEngine;
using System.Collections;
using System;

public class TestAnimation : MonoBehaviour
{
    //public static Vector3 time;
    //public static Vector3 step;
    //public static Vector3 pertime;
    //public static Vector2 index;
    //public static Vector2 inverseIndex;
    //public static int triggerInverse;
    public bool isPositive = true;

    Vector3 operaTime;
    Vector3 step;
    Vector2 distanceTime;

    float alltime;

    int index;

    bool onceSmallest = true;
    float startTimeS;
    bool onceSSmallest = true;
    float startTimeSS;
    bool onceMax = true;
    float startTimeM;
    public bool playing;

    public Action action;
    float time;

    //public static bool isplaying;
    // Use this for initialization
    void Start()
    {
        string name = this.gameObject.name.Replace("ZhiJia", "");
        try
        {
            index = int.Parse(name);
        }
        catch (Exception e)
        {
            Destroy(this);
        }
    }

    public int GetIndex()
    {
        return index;
    }

    public TestAnimation Play(Vector3 time, Vector3 step, Vector2 distance)
    {
        this.operaTime = time;
        this.step = step;
        this.distanceTime = distance;
        this.onceSmallest = true;
        this.onceSSmallest = true;
        this.onceMax = true;
        this.onceinverMax = true;
        playing = true;
        maxmecbing = null;
        remberinverseTime = true;
        if (isPositive)
        {
            alltime = time.x + time.y;//Mathf.Max(time.x, distance.x + time.y, 2 * distance.x + distance.y + time.z + maxinverTime);
                                      // MonoBehaviour.print(alltime);
        }
        else
            alltime = time.x + time.y + time.z + maxinverTime + time.x; //Mathf.Max(time.x, distance.x + time.y);
        return this;
    }

    public float GetAlltime()
    {
        return alltime;
    }

    public void SetAction(float time, Action action)
    {
        this.action = action;
        this.time = time;
    }
    // Update is called once per frame
    public void PlayPositive()
    {
        if (onceSmallest)
        {
            onceSmallest = false;
            Machine.singleton.MecBindings["Smallest3" + index + "_" + "Smallest2" + index].PlayAnimatin(operaTime.x, step.x);
            try
            {
                Animation ani = GameObject.Find("HuBang").transform.GetChild(index).GetComponent<Animation>();
                ani[ani.clip.name].speed = 1;
                ani[ani.clip.name].normalizedTime = 0;
                ani.Play();
            }
            catch
            {

            }
            startTimeS = Time.time;
        }


        if (!onceSmallest)
        {
            if ((Time.time - startTimeS > distanceTime.x) && onceSSmallest)
            {
                onceSSmallest = false;
                Machine.singleton.MecBindings["SSmallest3" + index + "_" + "SSmallest2" + index].PlayAnimatin(operaTime.y, step.y);
                startTimeSS = Time.time;
            }
        }
        if (!onceSSmallest)
        {
            if ((Time.time - startTimeSS > distanceTime.y) && onceMax)
            {
                onceMax = false;
                //	Machine.singleton.MecBindings["Max3" + index + "_" + "Max2" + index].PlayAnimatin(operaTime.z, step.z);
            }
        }
    }
    bool onceinverMax = true;
    MechanMetaComponentBinding maxmecbing = null;
    float maxinverTime = 2;
    float maxinverStartTime;
    public bool remberinverseTime;

    public static void PlayAnimationWhenMax(int indexx, bool only = false)
    {
        int aniindx = indexx;
        if (aniindx < 0)
            return;

        int groupindex = aniindx % 4;

        int targetindex;

        if (groupindex == 0)
            targetindex = aniindx + 2;
        else if (groupindex == 2)
        {
            aniindx = aniindx + 1;
            targetindex = aniindx - 2;
        }
        else
            return;

        GameObject game = Fuzhi.FindChildAll(GameObject.Find("ZhiJia" + aniindx), "BracketAll");
        GameObject game2 = Fuzhi.FindChildAll(GameObject.Find("ZhiJia" + aniindx), "UpArm");
        GameObject game3 = Fuzhi.FindChildAll(GameObject.Find("ZhiJia" + targetindex), "BracketAll");
        GameObject game4 = Fuzhi.FindChildAll(GameObject.Find("ZhiJia" + targetindex), "UpArm");

        if (game == null || game2 == null || game3 == null || game4 == null)
            return;

        Animation ani = game.GetComponent<Animation>();
        Animation ani2 = game2.GetComponent<Animation>();
        Animation ani3 = game3.GetComponent<Animation>();
        Animation ani4 = game4.GetComponent<Animation>();

        if (ani == null || ani2 == null || ani3 == null || ani4 == null)
            return;


        if (AnimationManager.identification == 1)
        {


            ani2[ani2.clip.name].speed = 1.5f;
            ani[ani.clip.name].speed = 1.5f;
            ani.gameObject.AddComponent<BracketAniControl>().target = 0.5f;

            if (!only)
            {

                ani3[ani3.clip.name].speed = 1.5f;
                ani4[ani4.clip.name].speed = 1.5f;
                ani3.gameObject.AddComponent<BracketAniControl>().target = 0.5f;
            }
        }

        if (AnimationManager.identification == 3)
        {
            ani2[ani2.clip.name].speed = 1.5f;
            ani[ani.clip.name].speed = 1.5f;
            if (!only)
            {
                ani3[ani.clip.name].speed = 1.5f;
                ani4[ani4.clip.name].speed = 1.5f;
            }
        }

        //if (AnimationManager.identification == 5 /*|| AnimationManager.identification == 3*/)
        //{

        //    Animation ani = game.GetComponent<Animation>();
        //    if (ani[ani.clip.name].normalizedTime >= 1)
        //        return;
        //    float extent = Mathf.Max(0, AnimationManager.singion.fiveBrackall = -0.1f);
        //    if (ani != null && ani.isPlaying)
        //    {
        //        if (game2 != null)
        //        {
        //            Animation ani2 = game2.GetComponent<Animation>();
        //            if (ani2 != null)
        //            {
        //                try
        //                {
        //                    ani2[ani2.clip.name].speed = 1;
        //                    game2.AddComponent<BracketAniControl>().target = extent;
        //                    //ani2[ani2.clip.name].normalizedTime = 0;
        //                }
        //                catch (System.Exception e)
        //                {
        //                    Debug.LogError("尝试播放默认动画片段为Null的动画");
        //                }
        //            }
        //        }
        //        try
        //        {
        //            ani[ani.clip.name].speed = 1;
        //            game.AddComponent<BracketAniControl>().target = extent;
        //            // ani[ani.clip.name].normalizedTime = 0;
        //        }
        //        catch (System.Exception e)
        //        {
        //            Debug.LogError("尝试播放默认动画片段为Null的动画");
        //        }
        //    }
        //}
    }

    public void PlayAnimationWhenMax()
    {
        if (AnimationManager.identification == 1)
        {
            PlayAnimationWhenMax(index - 2);
            if (index == 11)
                PlayAnimationWhenMax(10);
        }

        if (AnimationManager.identification == 3)
        {
            PlayAnimationWhenMax(index - 2);
        }
    }

    public void PlayInverse()
    {
        if (onceSmallest)
        {
            onceSmallest = false;
            maxmecbing = Machine.singleton.MecBindings["Max3" + index + "_" + "Max2" + index].PlayAnimatin(operaTime.x, step.x);
            startTimeS = Time.time;
            maxinverStartTime = float.MaxValue;
            PlayAnimationWhenMax();
            //可以播放向前移动的动画
        }

        if (!onceSmallest)
        {
            if (maxmecbing != null && !maxmecbing.playing && remberinverseTime)
            {
                remberinverseTime = false;
                maxinverStartTime = Time.time;
                //PlayAnimationWhenMax();//可以播放动画暂停时间
            }
        }
        // if (AnimationManager.identification == 3)//臨時判斷
        //    return;
        if (!onceSmallest)
        {
            if (Time.time - maxinverStartTime > maxinverTime && onceinverMax)
            {
                onceinverMax = false;
                if (maxmecbing != null)
                {
                    maxmecbing.PlayAnimatin(operaTime.x, -step.x);
                    maxmecbing = null;
                }
            }
        }

        if (!onceinverMax)
        {
            if ((Time.time - startTimeS > distanceTime.x) && onceSSmallest)
            {
                onceSSmallest = false;
                Machine.singleton.MecBindings["SSmallest3" + index + "_" + "SSmallest2" + index].PlayAnimatin(operaTime.y, step.y);
                startTimeSS = Time.time;
            }
        }
        if (!onceSSmallest)
        {
            if ((Time.time - startTimeSS > distanceTime.y) && onceMax)
            {
                onceMax = false;
                Machine.singleton.MecBindings["Smallest3" + index + "_" + "Smallest2" + index].PlayAnimatin(operaTime.z, step.z);

                try
                {
                    Animation ani = GameObject.Find("HuBang").transform.GetChild(index).GetComponent<Animation>();
                    ani[ani.clip.name].speed = -1;
                    ani[ani.clip.name].normalizedTime = 1;
                    ani.Play();
                }
                catch
                {

                }
            }
        }
    }

    void Update()
    {
        try
        {
            if (playing)
            {
                if (isPositive)
                    PlayPositive();
                else
                    PlayInverse();
                if (!onceSmallest)
                {
                    if (Time.time - startTimeS > time)
                    {
                        if (action != null)
                        {
                            action();
                            action = null;
                        }
                    }
                    if (Time.time - startTimeS > alltime)
                    {
                        if (!isPositive)
                        {
                            //  print("00000000000fdsgfdsgfd   "+alltime);
                            // if (AnimationManager.identification != 4)
                            ZhengScripts.Initilize(index);
                        }
                        //PlayAnimationWhenMax();
                        playing = false;
                    }
                }
            }
        }
        catch (Exception e)
        {
            // Debug.LogError("播放单个支架动画时报错：" + e.StackTrace);
        }
    }

    public static void IniAllTestAnimation()
    {
        TestAnimation[] testanis = GameObject.FindObjectsOfType<TestAnimation>();
        for (int i = 0; i < testanis.Length; i++)
        {
            testanis[i].playing = false;
            testanis[i].action = null;
        }
    }

    //public MechanMetaComponentBinding PlaySmallest(int indexo, bool direction = true)
    //{
    //    return Machine.singleton.MecBindings["Smallest3" + indexo + "_" + "Smallest2" + indexo].PlayAnimatin(time.x, direction ? step.x : -step.x);
    //}

    //public MechanMetaComponentBinding PlaySSmallest(int indexo, bool direction = true)
    //{
    //    return Machine.singleton.MecBindings["SSmallest3" + indexo + "_" + "SSmallest2" + indexo].PlayAnimatin(time.y, direction ? step.y : -step.y);
    //}

    //public MechanMetaComponentBinding PlayMax(int indexo, bool direction = true)
    //{
    //    return Machine.singleton.MecBindings["Max3" + indexo + "_" + "Max2" + indexo].PlayAnimatin(time.z, direction ? step.z : -step.z);
    //}
}
