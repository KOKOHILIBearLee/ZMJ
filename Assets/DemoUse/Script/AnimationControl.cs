using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour
{
    Animation ani;
    string clipname;
    // Use this for initialization

    private void Awake()
    {
        ani = GetComponent<Animation>();
        clipname = ani.clip.name;
        ani[clipname].speed = 0;
        ani[clipname].normalizedTime = 0;
        s = 0;
        ani.Play(clipname);
    }

    float s;
    public void SetOrderPlay()
    {
        ani[clipname].speed = 1;
        RefreshSpeed();
    }

    public void SetReversePlay()
    {
        ani[clipname].speed = -1;
        RefreshSpeed();
    }

    public bool IsPlaying()
    {
        return !(s == 0);
    }

    void RefreshSpeed()
    {
        s = ani[clipname].speed;
    }

    void Update()
    {
        if (s > 0)
        {
            if (!ani.IsPlaying(clipname))
            {
                Debug.LogError("播放完毕");
                ani[clipname].normalizedTime = 1;
                ani[clipname].speed = 0;
                ani.Play(clipname);
                RefreshSpeed();
            }
        }
        if (s < 0)
        {
            if (!ani.IsPlaying(clipname))
            {
                Debug.LogError("播放完毕1");
                ani[clipname].normalizedTime = 0;
                ani[clipname].speed = 0;
                ani.Play(clipname);
                RefreshSpeed();
            }
        }
        //if ()
    }
}
