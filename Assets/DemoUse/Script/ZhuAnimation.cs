using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZhuAnimation : MonoBehaviour
{
    public static bool order = true;
    public static int currentIndex = 1;
    public static int endIndex = 0;
    public static float triggerTime = 1f;
    public static bool offAll = false;
    public static List<float> normalTime = new List<float>();

    public static float targetAAL;
    // bool once = true;
    Animation animation;
    public float time = float.MaxValue;
    float startTime;

    public float toTarget = 2;
    float NormalTimeTarget = float.MaxValue;
    public ZhuAnimationMode mode;

    bool targetRem = true;
    bool attachRem = true;

    bool forDestroy = false;
    // Animator animator;
    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animation>();
        if (animation == null)
            return;
        //AnimationEvent aniamtionEvent = new AnimationEvent();
        //aniamtionEvent.functionName = "Attach";
        //aniamtionEvent.time = triggerTime;
        //animation.clip.AddEvent(aniamtionEvent);
        //animation.Play();
        animation[animation.clip.name].speed = 1;
        if (toTarget < 1)
        {
            NormalTimeTarget = animation[animation.clip.name].normalizedTime + toTarget;
        }
        else
        {
            if (NormalTimeTarget != float.MaxValue)
            {
                toTarget = 1;
            }
        }

        if (mode == ZhuAnimationMode.Mode3)
        {
            if (normalTime.Count > 0)
            {
                NormalTimeTarget = normalTime[0];
                normalTime.RemoveAt(0);
                toTarget = 0;
            }
        }
        startTime = Time.time;
    }

    private void Update()
    {
        if (offAll)
        {
            animation[animation.clip.name].speed = 0;
            animation[animation.clip.name].time = 0;
            Destroy(this);
        }

        if (animation != null)
        {
            if (animation[animation.clip.name].normalizedTime > NormalTimeTarget && targetRem)
            {
                targetRem = false;
                animation[animation.clip.name].normalizedTime = NormalTimeTarget;
                animation[animation.clip.name].speed = 0;
                if (time == float.MaxValue)
                {
                  //  PlayNext();
                    Destroy(this);
                }
                else
                {
                    if (forDestroy == true)
                    {
                      //  PlayNext();
                        Destroy(this);
                    }
                    else
                        forDestroy = true;
                }
            }
        }
        Attach();
    }

    void PlayNext()
    {
        try
        {
            if (order)
                currentIndex++;
            else
                currentIndex--;
            if (currentIndex <= Fuzhi.mounts && currentIndex >= 0)
            {
                if (order ? currentIndex <= endIndex : currentIndex >= endIndex)
                {
                    ZhuAnimation zhu = GameObject.Find("Sketator" + currentIndex).GetComponent<ZhuAnimation>();
                    if (zhu==null)
                    {
                        zhu = GameObject.Find("Sketator" + currentIndex).AddComponent<ZhuAnimation>();
                    }
                    zhu.time = time;
                    zhu.mode = mode;
                    if (mode == ZhuAnimationMode.Mode1)
                    {
                        zhu.toTarget = toTarget + targetAAL;
                        print(zhu.toTarget);
                    }
                    if (mode == ZhuAnimationMode.Mode2)
                    {
                        zhu.NormalTimeTarget = NormalTimeTarget;
                    }
                }
            }
        }
        catch (System.Exception e) { print(currentIndex + "  kk"); }
    }

    public void Attach()
    {
        if ((Time.time - startTime > time)&&attachRem)
        {
            attachRem = false;
            animation.clip.events = null;
            PlayNext();
            if (toTarget > 1)
            {
                Destroy(this);
            }
            else
            {
                if (forDestroy == true)
                { 
                    Destroy(this);
                }
                else
                    forDestroy = true;
            }
        }
        //if (once)
        //{
        //    once = false;
        //    try
        //    {
        //        if (currentIndex < Fuzhi.mounts && currentIndex > 0)
        //        {
        //            if (order)
        //                currentIndex++;
        //            else
        //                currentIndex--;
        //            if (order ? currentIndex <= endIndex : currentIndex >= endIndex)
        //                GameObject.Find("Sketator" + currentIndex).AddComponent<ZhuAnimation>();
        //            else
        //                print(currentIndex + "   ff");
        //        }
        //    }
        //    catch (System.Exception e) { print(currentIndex+"  kk"); }
        //    animation.clip.events = null;
        //    Destroy(this);
        //}
    }

    public static void PlayAnimation(Vector2 index, float time, float[] target, ZhuAnimationMode mode =ZhuAnimationMode.Mode1)
    {
        GameObject game = GameObject.Find("Sketator" + (int)index.x);
        if (game == null)
            return;
        if (index.y > index.x)
            order = true;
        else
            order = false;
        currentIndex = (int)index.x;
        endIndex = (int)index.y;
        ZhuAnimation zhu = game.GetComponent<ZhuAnimation>();
        if (zhu==null)
        {
            zhu = game.AddComponent<ZhuAnimation>();
        }
        zhu.time = time;
        zhu.mode = mode;
        if (mode==ZhuAnimationMode.Mode1)
        {
            zhu.toTarget = target[0];
            targetAAL = target[0];
        }
        if (mode == ZhuAnimationMode.Mode2)
        {
            zhu.NormalTimeTarget = target[0];
        }
        if (mode == ZhuAnimationMode.Mode3)
        {
            normalTime = new List<float>(target);
        }
            
    }

    public enum ZhuAnimationMode
    {
        Mode1,
        Mode2,
        Mode3,
    }
}
