using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BracketAniControlManager : MonoBehaviour
{
    public static BracketAniControlManager singleton;

    public List<AnimationPlay> animations = new List<AnimationPlay>();

    // Use this for initialization
    void Awake()
    {
        singleton = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < animations.Count; i++)
            animations[i].Update();
    }



    public class AnimationPlay
    {
        public BracketPlay play;
        public string targetName;
        public Vector2 index;
        public List<float> extent = new List<float>();
        public float interval;

        public void Update()
        {
            if (Time.time - play.starttime > interval)
            {
                int p = (int)(play.current + (index.y - index.x) / Mathf.Abs(index.y - index.x));
                if (extent.Count == 0 || p > (int)index.y)
                {
                    BracketAniControlManager.singleton.animations.Remove(this);
                    return;
                }
                extent.RemoveAt(0);
                play = Play(p, extent[0], targetName);
            }
        }

        BracketPlay Play(int index, float extent, string gamename = "BracketAll")
        {
            GameObject father = SOManager.singleton.FindObject("ZhiJia" + index);
            GameObject first = Fuzhi.FindChildAll(father, gamename);
            if (BracketAniControl.PlayBracketAniControl(first, extent, true) != null)
            {
                BracketPlay pl = new BracketPlay(index);
                return pl;
            }

            return null;
        }
    }

    public class BracketPlay
    {
        public int current;
        public float starttime;

        public BracketPlay(int current)
        {
            this.current = current;
            this.starttime = Time.time;
        }
    }

    public void Play(Vector2 index, float interval, List<float> extent, string gamename = "BracketAll")
    {
        GameObject father = SOManager.singleton.FindObject("ZhiJia" + (int)index.x);

        GameObject first = Fuzhi.FindChildAll(father, gamename);

        if (BracketAniControl.PlayBracketAniControl(first, extent[0], true) != null)
        {
            AnimationPlay play = new AnimationPlay();
            play.index = index;
            play.interval = interval;
            play.targetName = gamename;
            play.extent = extent;
            play.play = new BracketPlay((int)index.x);
            BracketAniControlManager.singleton.animations.Add(play);
        }


        //play = new BracketPlay();
        //play.index = index;
        //play.current = (int)index.x;
        //this.extent = extent;
        //this.interval = interval;
    }
}
