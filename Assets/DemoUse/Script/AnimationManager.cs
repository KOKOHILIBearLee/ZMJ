using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void PauseAction();
public class AnimationManager : MonoBehaviour
{
    public Vector2 targetIndex;

    #region DT_YJQD提示框
    // public GameObject DT_YJQDUI;
    MeshRenderer DT_YJQDrender;
    TextMesh DT_YJQDmesh;

    void InitilizeDT_YJQD()
    {
        GameObject DT_YJQD = GameObject.Find("DT_YJQD");
        DT_YJQDrender = DT_YJQD.GetComponent<MeshRenderer>();
        DT_YJQDmesh = DT_YJQD.GetComponent<TextMesh>();
    }

    public void SetDT_YJQDTextContent(string content)
    {
        DT_YJQDmesh.text = content;
        SetDT_YJQDVisiblity(true);
    }

    public void SetDT_YJQDVisiblity(bool val)
    {
        DT_YJQDrender.enabled = val;
    }
    #endregion
    public int width;
    public float actiontime;
    GameObject mineWall = null;
    PauseAction pauseAction = null;
    public float pauseTime;
    float pauseStartTime;

    Vector3 penwuposition = new Vector3(0, -0.0008f, 0);
    Vector3 penwurotation = new Vector3(270, 0, 0);
    Vector3 penwuscale = new Vector3(0.0004f, 0.0004f, 0.0004f);

    Vector2 currentIndex;
    bool order;

    public bool playing = false;

    Vector3 time;
    Vector3 step;
    Vector2 distance;
    Queue<AnimationStateForZMJ> animationQueue = new Queue<AnimationStateForZMJ>();
    AnimationStateForZMJ[] rember;
    public static int identification;
    float startfirstflattime;
    float distancetime = 15;

    bool call1;
    bool call2;
    public float fiveBrackall = 1f;
    AnimationStateForZMJ zmj;
    public static AnimationManager singion;
    // Use this for initialization
    List<GameObject> penwuParticleGroup1=new List<GameObject>();
    List<GameObject> penwuParticleGroup2=new List<GameObject>();

    private void Awake()
    {
        singion = this;
        InitilizeDT_YJQD();

        GameObject p = Resources.Load<GameObject>("PenWu");
        p.SetActive(false);

        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));

        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));
    }

    public void IntilizeZJParticle()
    {
        for (int i = 0; i < penwuParticleGroup1.Count; i++)
            penwuParticleGroup1[i].SetActive(false);
        for (int i = 0; i < penwuParticleGroup2.Count; i++)
            penwuParticleGroup2[i].SetActive(false);
    }

    void SetGroup1Particle(int startindex)
    {
        for (int i = 0; i < penwuParticleGroup1.Count; i++)
        {
            GameObject fa = GameObject.Find("ZhiJia" + (i + startindex));
            if (fa != null)
            {
                GameObject g = Fuzhi.FindChildAll(fa, "UpArm");

                penwuParticleGroup1[i].SetActive(true);
                penwuParticleGroup1[i].transform.parent = g.transform;
                penwuParticleGroup1[i].transform.localPosition = penwuposition;
                penwuParticleGroup1[i].transform.localEulerAngles = penwurotation;
                penwuParticleGroup1[i].transform.localScale = penwuscale;
            }
            else
                penwuParticleGroup1[i].SetActive(false);
        }
    }

    void SetGroup2Particle(int startindex)
    {
        for (int i = 0; i < penwuParticleGroup2.Count; i++)
        {
            GameObject fa = GameObject.Find("ZhiJia" + (i + startindex));
            if (fa != null)
            {
                GameObject g = Fuzhi.FindChildAll(fa, "UpArm");

                penwuParticleGroup2[i].SetActive(true);
                penwuParticleGroup2[i].transform.parent = g.transform;
                penwuParticleGroup2[i].transform.localPosition = penwuposition;
                penwuParticleGroup2[i].transform.localEulerAngles = penwurotation;
                penwuParticleGroup2[i].transform.localScale = penwuscale;
            }
            else
                penwuParticleGroup2[i].SetActive(false);
        }
    }


    //Play(Vector3 time, Vector3 step, Vector3 distance)
    void Play(Vector3 time, Vector3 step, Vector2 distance, Vector2 targetindex, int width, float actionPertime)
    {
        call1 = false;
        call2 = false;
        this.time = time;
        this.step = step;
        this.distance = distance;
        this.targetIndex = targetindex;
        this.width = width;
        this.actiontime = Staure(actionPertime);
        if (this.targetIndex.x < this.targetIndex.y)
            order = true;
        else
            order = false;

        currentIndex = new Vector2(this.targetIndex.x, this.targetIndex.x + (order ? width : -width));
        playing = true;

        //if (order)
        //{
        //    PlayMec(new Vector2(currentIndex.y, currentIndex.x));
        //}
        //else
        PlayMec(new Vector2(currentIndex.y, currentIndex.x));
    }
    bool minewallorder = true;
    float minewalltime = -1;
    public void Play(AnimationStateForZMJ[] states)
    {
        //BracketAniControlManager.singleton.Play(new Vector2(20, 30), 4f, new List<float>(new float[] { 0.51f, 0.55f, 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f }));
        //BracketAniControlManager.singleton.Play(new Vector2(20, 30), 4f, new List<float>(new float[] { 0.51f, 0.55f, 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f }), "UpArm");
        // AddParticle();
        DestroyWall.singleton.control1 = false;
        DestroyWall.singleton.control2 = false;
        DestroyWall.singleton.height = 0.9f;
        CutMineAnimation.singleton.speed = 0.52f;
        playin = true;
        firstplay = true;
        firstplaytime = 20;
        firstplaysatrttime = Time.time;
        firstcutterplay = true;
        firstcutterplaytime = 5;
        firstcutterplaystarttime = Time.time;
        rember = states;
        identification = 0;
        SlanimationFirst = true;
        allAnimationOvew = false;
        zmjZhijiaend = false;
        result = false;
        allstop1 = false;
        pauseAction = null;
        call1 = false;
        call2 = false;
        animationQueue = new Queue<AnimationStateForZMJ>();
        for (int i = 0; i < states.Length; i++)
            animationQueue.Enqueue(states[i]);
        float[] sketor = new float[54];
        float[] bracket = new float[54];
        float[] uparm = new float[54];
        for (int i = 0; i < sketor.Length; i++)
        {
            sketor[i] = 0.5f;
            bracket[i] = 0.5f;
            uparm[i] = 1;
        }
        ZhengScripts.InitilizeBracketAll(new Vector2(0, 53), bracket);
        ZhengScripts.InitilizeUpArm(new Vector2(0, 53), uparm);
        ZhengScripts.InitilizeSketator(new Vector2(0, 53), sketor);

        ZhengScripts.InitilizeBracketAll(new Vector2(0, 11), new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.2f, 0.3f, 0.4f });
        ZhengScripts.InitilizeUpArm(new Vector2(0, 11), new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0.2f, 0.4f, 0.6f, 0.8f });
        ZhengScripts.InitilizeSketator(new Vector2(0, 16), new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.5f, 0.5f });
        ZhuAnimation.PlayAnimation(new Vector2(0, 16), 0.1f, new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0.05f, 0.1f, 0.15f, 0.2f, 0.3f, 0.4f, 0.5f, 0.5f, 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode3);


        //        if (identification == 1)
        //      {

        //    }
    }

    public void IniMineWallParas()
    {
        ClearMineWall();
        minewallorder = true;
        minewalltime = -1;
    }

    void ClearMineWall()
    {
        if (mineWall != null)
        {
            Destroy(mineWall);
            mineWall = null;
        }
    }

    void IniMineWall()
    {
        if (mineWall == null)
        {
            mineWall = Resources.Load<GameObject>("Minewall");
            if (mineWall != null)
            {
                mineWall = GameObject.Instantiate<GameObject>(mineWall);
            }
        }
        else
        {
            ClearMineWall();
            IniMineWall();
        }

        if (mineWall != null)
        {
            DestroyWall.singleton.father = mineWall;
            DestroyWall.singleton.IniFather();
        }
    }

    void IniCutterBra()
    {
        GameObject bra1 = GameObject.Find("钻头001");
        GameObject bra2 = GameObject.Find("钻头002");

        DestroyWall.singleton.cutter = new List<GameObject>();
        DestroyWall.singleton.cutter.Add(bra1);
        DestroyWall.singleton.cutter.Add(bra2);
    }

    bool allAnimationOvew = false;
    float allstarttime;
    bool allstop1 = false;
    float allstope1timme;

    void PlayCutterArm(int mode = 0)
    {
        GameObject coalcutter = GameObject.Find("CMJ");
        if (coalcutter != null)
        {
            GameObject JixieBi1 = Fuzhi.FindChildAll(coalcutter, "钻头机械臂001");
            GameObject JixieBi2 = Fuzhi.FindChildAll(coalcutter, "钻头机械臂002");
            Animation jixieBiani1 = JixieBi1.GetComponent<Animation>();
            Animation jixieBiani2 = JixieBi2.GetComponent<Animation>();

            Debug.LogError("播放割煤机臂动画序号为：" + mode);

            if (jixieBiani1 != null)
            {
                if (mode == 0)
                {
                    jixieBiani1[jixieBiani1.clip.name].normalizedTime = 1;
                    jixieBiani1[jixieBiani1.clip.name].speed = 0;
                    jixieBiani1.Play();
                    CastTransfrom.singleton.InitilizeAnimation();
                    
                    //CastTransfrom.singleton.ToLeft();
                }
                if (mode == 1)
                {
                    jixieBiani1[jixieBiani1.clip.name].normalizedTime = 1;
                    jixieBiani1[jixieBiani1.clip.name].speed = -1;
                    jixieBiani1.Play();
                    CastTransfrom.singleton.LeftArmDown();
                    CastTransfrom.singleton.RightArmUp();
                    //CastTransfrom.singleton.ToRight();
                }
                if (mode == 2)
                {
                    //jixieBiani1[jixieBiani1.clip.name].normalizedTime = 1;
                    jixieBiani1[jixieBiani1.clip.name].speed = 1;
                    jixieBiani1.Play();
                    CastTransfrom.singleton.LeftArmUp();
                    CastTransfrom.singleton.RightArmDown();
                    //CastTransfrom.singleton.ToLeft();
                }
                if (mode == 3)
                {
                    jixieBiani1[jixieBiani1.clip.name].normalizedTime = 1;
                    jixieBiani1[jixieBiani1.clip.name].speed = -1;
                    jixieBiani1.Play();
                    zmjLastArmAnimation = Time.time;
                    CutMineAnimation.singleton.bway3 = true;
                    CastTransfrom.singleton.LeftArmDown();
                    //CastTransfrom.singleton.ToRight();
                }
            }
            if (jixieBiani2 != null)
            {
                if (mode == 0)
                {
                    jixieBiani2[jixieBiani2.clip.name].normalizedTime = 1;
                    jixieBiani2[jixieBiani2.clip.name].speed = 0;
                    jixieBiani2.Play();
                }
                if (mode == 1)
                {
                    jixieBiani2[jixieBiani2.clip.name].normalizedTime = 1;
                    jixieBiani2[jixieBiani2.clip.name].speed = -1;
                    jixieBiani2.Play();
                }
                if (mode == 2)
                {
                    //jixieBiani2[jixieBiani2.clip.name].normalizedTime = 1;
                    jixieBiani2[jixieBiani2.clip.name].speed = 1;
                    jixieBiani2.Play();
                }
                if (mode == 4)
                {
                    jixieBiani2[jixieBiani2.clip.name].normalizedTime = 1;
                    jixieBiani2[jixieBiani2.clip.name].speed = -1;
                    jixieBiani2.Play();
                    CastTransfrom.singleton.RightArmUp();
                    //CastTransfrom.singleton.ToLeft();
                }
            }
        }
    }

    void Play()
    {
        try
        {
            zmj = animationQueue.Dequeue();
            identification++;
            Play(zmj.time, zmj.step, zmj.distance, zmj.targetIndex, zmj.width, zmj.actionPertime);
            // zmj = new AnimationStateForZMJ(zmj.time, zmj.step, zmj.distance, zmj.targetIndex, zmj.width, zmj.actionPertime, zmj.stopTime);
            zmj.startTime = Time.time;
            //CutMineAnimation.singleton.PlayCutterAnimation();

            if (identification == 5)
            {
                BracketAniControlManager.singleton.Play(new Vector2(37, 46), 2f, new List<float>(new float[] { 1, 1, 1, 1, 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }));
                BracketAniControlManager.singleton.Play(new Vector2(37, 46), 2f, new List<float>(new float[] { 1, 1, 1, 1, 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }), "UpArm");
            }

        }
        catch (System.Exception e)
        {
            MonoBehaviour.print("AnimationManager Over " + e.StackTrace);
            //for (int i = 0; i < Fuzhi.mounts; i++)
            //{
            //    ZhengScripts.Initilize(i);
            //}
            //
            Machine.singleton.Inistint();
            ZhengScripts.Binding();
            AnimationManager.singion.playing = true;
            allstope1timme = Time.time;
            allAnimationOvew = true;
            allstop1 = true;
            Cutter2DAnimation.singleton.SwitchScenceInitilize();
            // CastTransfrom.singleton.InitilizeAnimation();
            // Play(rember);
        }
    }

    public float Staure(float number)
    {
        if (number > 1)
            return 1;
        if (number < 0)
            return 0;
        return number;
    }

    public bool PlayMec(Vector2 playindex)
    {
        TestAnimation t = GetAnimation((int)playindex.x);
        if (t != null)
        {
            t.isPositive = true;
            t.Play(this.time, this.step, this.distance);
            t.SetAction(this.actiontime * t.GetAlltime(), delegate
            {
                call1 = true;
            });
        }
        else
            return false;


        //can add animation before play inverse animation
        pauseStartTime = Time.time;
        pauseAction = delegate
        {
            pauseAction = null;
            TestAnimation tt = GetAnimation((int)playindex.y);
            if (tt != null)
            {
                tt.isPositive = false;
                Vector3 newTime = new Vector3(this.time.z, this.time.y, this.time.x);
                Vector3 newStep = new Vector3(this.step.z, this.step.y, this.step.x);
                Vector2 newDistance = new Vector2(this.distance.y, this.distance.x);
                tt.Play(newTime, -newStep, newDistance);
                tt.SetAction(this.actiontime * tt.GetAlltime(), delegate
                {
                    call2 = true;
                });
            }
        };
        return true;
    }

    TestAnimation GetAnimation(int inde)
    {
        GameObject game = GameObject.Find("ZhiJia" + inde);
        if (game == null)
            return null;
        return game.GetComponent<TestAnimation>();
    }
    bool SlanimationFirst = true;
    TestAnimation ani1;
    TestAnimation ani2;
    bool result = false;
    // Update is called once per frame

    float zmjZhijiaendStart;
    float zmjLastArmAnimation;
    float zmjFiveCutterSart;
    bool zmjZhijiaend = false;

    void StartTimerForCutterAnimation()
    {
        zmjZhijiaend = true;
        if (identification == 1)
            CutMineAnimation.singleton.bway1 = true;
        if (identification == 2)
            CutMineAnimation.singleton.bway2 = true;
        //if (identification == 3)
        //    CutMineAnimation.singleton.bway3 = true;
        zmjZhijiaendStart = Time.time;


        // ZhuAnimation.PlayAnimation(new Vector2(0, 16), 0.1f, new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.5f, 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode3);
    }

    void UpdateCutterAniamtion()
    {
        if (Time.time - zmjLastArmAnimation > CutMineAnimation.singleton.pauseTime.z && CutMineAnimation.singleton.bway3)
        {
            CutMineAnimation.singleton.bway3 = false;
            GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
            Debug.LogError("第四次播放割煤动画");
            //SetDT_YJQDTextContent("割底煤");
            if (Cutter2DAnimation.singleton.playing)
            {
                CastTransfrom.singleton.CutDown();
                CastTransfrom.singleton.ToRight();
            }
            CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way4, cutter, -1, CutMineAnimation.singleton.speed, 0, () =>
             {
                 Debug.LogError("第四次播放割煤动画结束");
                 //SetDT_YJQDVisiblity(false);
                 if (Cutter2DAnimation.singleton.playing)
                 {
                     CastTransfrom.singleton.MiddleVisible(false);
                     CastTransfrom.singleton.HintVisible(false);
                 }
                 CutMineAnimation.singleton.bway4 = true;
                 zmjFiveCutterSart = Time.time;
                 //动画播放完毕时执行的函数
             }, index =>
             {
                 SetGroup1Particle(-index + 47 + 5 + 2);
                 SetGroup2Particle(-index + 47 + 5 - 8+1);
             });
        }

        if (Time.time - zmjFiveCutterSart > CutMineAnimation.singleton.pauseTime.w && CutMineAnimation.singleton.bway4)
        {
            CutMineAnimation.singleton.bway4 = false;
            GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
            Debug.LogError("第五次播放割煤动画");
            if (Cutter2DAnimation.singleton.playing)
                CastTransfrom.singleton.ToLeft();
            CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way5, cutter, 1, CutMineAnimation.singleton.speed, 0, () =>
             {
                 PlayCutterArm(4);
                 Debug.LogError("第五次播放割煤动画结束");
                 //SetDT_YJQDVisiblity(false);
                 if (Cutter2DAnimation.singleton.playing)
                 {
                     CastTransfrom.singleton.MiddleVisible(false);
                     CastTransfrom.singleton.HintVisible(false);
                 }
                 //动画播放完毕时执行的函数
             }, index =>
             {
                 SetGroup1Particle(index - 3 + 38);
                 SetGroup2Particle(index - 3 + 39 + 8);
             });
        }

        if (zmjZhijiaend)
        {
            if (Time.time - zmjZhijiaendStart > CutMineAnimation.singleton.pauseTime.x && CutMineAnimation.singleton.bway1)
            {
                CutMineAnimation.singleton.bway1 = false;
                zmjZhijiaend = false;
                GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
                Debug.LogError("第二次播放割煤动画");
                //SetDT_YJQDTextContent("切三角煤");
                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.CutTri();
                    CastTransfrom.singleton.ToRight();
                }
                CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way2, cutter, -1, CutMineAnimation.singleton.speed, 0, () =>
                 {
                     //  if (identification == 1)
                     Debug.LogError("第二次播放割煤动画结束");
                     //SetDT_YJQDVisiblity(false);
        
                     PlayCutterArm(2);
                     CutMineAnimation.singleton.speed = 0.54f;
                     if (Cutter2DAnimation.singleton.playing)
                     {
                         CastTransfrom.singleton.MiddleVisible(false);
                         CastTransfrom.singleton.HintVisible(false);
                         Cutter2DAnimation.singleton.StopCutter2ThirdStageAnimation();
                     }
                     //动画播放完毕时执行的函数
                 }, index =>
                 {
                     SetGroup1Particle(-index + 16 + 6 + 1);
                     SetGroup2Particle(-index + 16 + 6 - 8);
                     Debug.LogError("第二段动画序列号：" + index);
                     if (index == 4)
                     {
                         if (Cutter2DAnimation.singleton.playing)
                             Cutter2DAnimation.singleton.StartCutter2ThirdStageAnimation();
                         //Cutter2DAnimation.singleton.StartCutter1ThirdStageAnimation();
                     }

                 });
            }
            if (Time.time - zmjZhijiaendStart > CutMineAnimation.singleton.pauseTime.y && CutMineAnimation.singleton.bway2)
            {
                CutMineAnimation.singleton.bway2 = false;
                zmjZhijiaend = false;
                Debug.LogError("第三次播放割煤动画");
                //SetDT_YJQDTextContent("割底煤");
                GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
                if (Cutter2DAnimation.singleton.playing)
                {
                    Cutter2DAnimation.singleton.ShenSuoBi1SecondStage();
                    Cutter2DAnimation.singleton.TrackSecondStage();
                    CastTransfrom.singleton.ToLeft();
                    CastTransfrom.singleton.CutDown();
                }
                CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way3, cutter, 1, CutMineAnimation.singleton.speed, 0, () =>
                 {
                     Debug.LogError("第三次播放割煤动画结束");
                     //SetDT_YJQDVisiblity(false);
                     if (Cutter2DAnimation.singleton.playing)
                     {
                         CastTransfrom.singleton.MiddleVisible(false);
                         CastTransfrom.singleton.HintVisible(false);
                         Cutter2DAnimation.singleton.StopCutter1ThirdStageAnimation();
                     }
                     PlayCutterArm(3);
                     //动画播放完毕时执行的函数
                 }, index =>
                 {
                     SetGroup1Particle(index - 3 - 1);
                     SetGroup2Particle(index - 3 + 8);
                     Debug.LogError("第三次播放割煤动画索引：" + index);
                     if (index == 17)
                     {
                         //SetDT_YJQDTextContent("中部跟机");
                         if (Cutter2DAnimation.singleton.playing)
                         {
                             CastTransfrom.singleton.FollowMiddle();
                             Cutter2DAnimation.singleton.StartCutter1ThirdStageAnimation();
                         }
                     }

                     if (index == 41 && AnimationManager.identification == 3)
                     {
                         TestAnimation.PlayAnimationWhenMax(36, true);
                     }
                 });
            }

            //if (/*Time.time - zmjZhijiaendStart > CutMineAnimation.singleton.pauseTime.z &&*/ CutMineAnimation.singleton.bway3)
            //{
            //    CutMineAnimation.singleton.bway3 = false;
            //    zmjZhijiaend = false;
            //    GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
            //    CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way4, cutter, CutMineAnimation.singleton.speed, 0, () =>
            //    {

            //        //动画播放完毕时执行的函数
            //    });
            //}
        }
    }
    //                GameObject coalcutter = GameObject.Find("CMJ");
    //                if (coalcutter != null)
    //                {
    //                    MonoBehaviour.print("Animation Over");
    //                    GameObject JixieBi1 = Fuzhi.FindChildAll(coalcutter, "钻头机械臂001");
    //                    GameObject JixieBi2 = Fuzhi.FindChildAll(coalcutter, "钻头机械臂002");
    //                    Animation ani = coalcutter.GetComponent<Animation>();
    //                    Animation jixieBiani1 = JixieBi1.GetComponent<Animation>();
    //                    Animation jixieBiani2 = JixieBi2.GetComponent<Animation>();
    //                    if (ani != null)
    //                    {
    //                        ani.Play();
    //                        MonoBehaviour.print("Animation Over Play");
    //                    }

    //                    if (jixieBiani1 != null)
    //                    {
    //                        jixieBiani1.Play();
    //                    }
    //                    if (jixieBiani2 != null)
    //                    {
    //                        jixieBiani2.Play();
    //                    }
    //                }

    bool firstplay = false;
    bool firstcutterplay = false;
    float firstcutterplaytime;
    float firstcutterplaystarttime;
    float firstplaytime;
    float firstplaysatrttime;

    bool playin = true;

    public void Stop()
    {
        playin = false;
    }

    UnityEngine.UI.Image image;
    void Update()
    {
        if (SOManager.singleton.currSceneName != "ZNCM_G")
        {
            try
            {
                if (image == null)
                    image = Cutter2DAnimation.singleton.mineWallsAll.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                    image.enabled = false;
            }
            catch { }
        }

        if (!playin)
            return;

        if (firstcutterplay)
        {
            if (Time.time - firstcutterplaystarttime > firstcutterplaytime)
            {
                if (SOManager.singleton.currSceneName == "ZNCM")
                    Training.TrainingManager.ExecuteScript("SkipVRCamera,ZNCM_TZ");

                firstcutterplay = false;
                GameObject cutter = GameObject.Find(CutMineAnimation.singleton.cutterName);
                try
                {
                    PlayCutterArm();
                    if (Cutter2DAnimation.singleton != null)
                        Cutter2DAnimation.singleton.InitilizeAll(GameObject.Find("CMJ_ZNCM"));
                    Debug.LogError("第一次播放割煤动画");
                    //SetDT_YJQDTextContent("斜切进刀");
                    if (Cutter2DAnimation.singleton.playing)
                    {
                        CastTransfrom.singleton.CutKnife();
                        CastTransfrom.singleton.ToLeft();
                    }
                    CutMineAnimation.singleton.PlayCutterAnimation(CutMineAnimation.singleton.way1, cutter, 1, CutMineAnimation.singleton.speed, 0, () =>
                     {
                         Debug.LogError("第一次播放割煤动画结束");
                        //SetDT_YJQDVisiblity(false);
                        if (Cutter2DAnimation.singleton.playing)
                         {
                             CastTransfrom.singleton.MiddleVisible(false);
                             CastTransfrom.singleton.HintVisible(false);
                             Cutter2DAnimation.singleton.StopCutter1SecondStageAnimation();
                             Cutter2DAnimation.singleton.StopCutter2SecondStageAnimation();
                         }
                         ZhuAnimation.PlayAnimation(new Vector2(16, 0), 0.5f, new float[] { 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode2);
                         PlayCutterArm(1);
                        //动画播放完毕时执行的函数
                    }, index =>
                    {
                        Debug.LogError("序列号：" + index);

                        SetGroup1Particle(index - 3 - 1);
                        SetGroup2Particle(index - 3 + 8);



                        if (index == 5)
                        {

                            if (Cutter2DAnimation.singleton.playing)
                                Cutter2DAnimation.singleton.StartCutter1FirstStageAnimation();
                            //Cutter2DAnimation.singleton.control1.AbandonRight();
                            DestroyWall.singleton.control1 = true;
                            if (Cutter2DAnimation.singleton != null)
                                Cutter2DAnimation.singleton.SetDescending(true, -2.0f);
                            //if (Cutter2DAnimation.singleton != null)
                            //    Cutter2DAnimation.singleton.SetCutter1(true);
                            //AddParticle(0);
                        }
                        if (index == 8)
                        {
                            Cutter2DAnimation.singleton.StartCutter1SecondStageAnimation();
                        }
                        if (index == 9)
                        {
                            if (Cutter2DAnimation.singleton.playing)
                                Cutter2DAnimation.singleton.StartCutter2FirstStageAnimation();

                            DestroyWall.singleton.control2 = true;
                            if (Cutter2DAnimation.singleton != null)
                            {
                                Cutter2DAnimation.singleton.SetDescending(true, 1.3f);
                                Cutter2DAnimation.singleton.SetCutter1(true);
                            }
                            //AddParticle(1);
                        }

                        if (index == 9)
                        {
                            Cutter2DAnimation.singleton.scalecutterrotation = 1f;
                            //Cutter2DAnimation.singleton.control1.AbandonLeft();

                        }

                        if (index == 13)
                        {
                            //Cutter2DAnimation.singleton.control1.AbandonRight1();
                        }

                        if (index == 13)
                        {
                            if (Cutter2DAnimation.singleton.playing)
                                Cutter2DAnimation.singleton.StartCutter2SecondStageAnimation();
                            if (Cutter2DAnimation.singleton != null)
                                Cutter2DAnimation.singleton.SetCutter2(true);
                        }

                        if (index == 14)
                        {
                            DestroyWall.singleton.height = 10;
                        }
                        if (index == 16)
                        {
                            SetGroup2Particle(index - 3 + 9);
                            //if (Cutter2DAnimation.singleton != null)
                            //    Cutter2DAnimation.singleton.SetCutter2(true);
                        }
                    });
                }
                catch (System.Exception e)
                {
                    Debug.LogError("播放采煤机动画时出现错误：" + e.StackTrace);
                }

                if (SOManager.singleton.minewall != null)
                    SOManager.singleton.minewall.SetActive(false);
                if (minewallorder)
                {
                    minewallorder = false;
                    IniMineWall();
                    IniCutterBra();
                }
            }
        }

        if (firstplay)
        {
            if (Time.time - firstplaysatrttime > firstplaytime)
            {
                firstplay = false;
                Play();
            }
        }
        if (playing)
        {
            UpdateCutterAniamtion();
            if (minewalltime > 0 && Time.time - minewalltime > 2)
            {
                minewalltime = -1;
                if (!minewallorder)
                {
                    IniMineWall();
                    IniCutterBra();
                }
            }
            if (allAnimationOvew)
            {
                if (allstop1 && Time.time - allstope1timme > 3)
                {
                    allstop1 = false;
                    Training.TrainingManager.ExecuteScript("SetLenght,0:16,-0.4:0.6:0.01");
                    allstope1timme = Time.time;
                }
                if (!allstop1 && Time.time - allstarttime > 3)
                {
                    allAnimationOvew = false;
                    Play(rember);
                    minewalltime = Time.time;
                }
            }
            if (call1 && call2)
            {
                call1 = false;
                call2 = false;

                if (order)
                {
                    if (currentIndex.y >= targetIndex.y)
                    {
                        result = true;
                        ani1 = GetAnimation((int)currentIndex.x);
                        ani2 = GetAnimation((int)currentIndex.y);
                        startfirstflattime = Time.time;
                        StartTimerForCutterAnimation();
                        return;
                    }
                    currentIndex = new Vector2(currentIndex.x + 1, currentIndex.y + 1);
                }
                else
                {
                    if (currentIndex.y <= targetIndex.y)
                    {
                        result = true;
                        ani1 = GetAnimation((int)currentIndex.x);
                        ani2 = GetAnimation((int)currentIndex.y);
                        startfirstflattime = Time.time;
                        StartTimerForCutterAnimation();
                        return;
                    }
                    currentIndex = new Vector2(currentIndex.x - 1, currentIndex.y - 1);
                }

                if (identification == 3)
                {
                    if (currentIndex.y == 33)
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(0, 12), 0.1f, new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9f, 0.8f, 0.7f, 0.6f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                        MonoBehaviour.print("SLFirst");
                    }
                    if (currentIndex.y == 47)
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(10, 25), 0.1f, new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                        MonoBehaviour.print("SLSecond");
                    }
                }

                if (identification == 5)
                {
                    if (currentIndex.y == 53)
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(21, 44), 0.1f, new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                        MonoBehaviour.print("SLTirth");
                    }
                }
                //if (currentIndex.x == 6)
                //{
                //    DestroyWall.singleton.control1 = true;
                //}
                PlayMec(new Vector2(currentIndex.y, currentIndex.x));
            }

            if (result)
            {
                if (ani1 != null && ani2 != null)
                {
                    if (!ani1.playing && !ani2.playing)
                    {
                        if (SlanimationFirst && Time.time - startfirstflattime > distancetime)
                        {
                            SlanimationFirst = false;

                        }
                        //  MonoBehaviour.print("pppppppppppppppp");
                        if (Time.time - zmj.startTime > zmj.stopTime)
                        {
                            result = false;
                            // print(new Vector2(targetIndex.y, targetIndex.x));
                            Play();
                            //Play(time, step, distance, new Vector2(targetIndex.y, targetIndex.x), width, actiontime);
                        }
                    }
                }
            }

            if (pauseAction != null)
            {
                if (Time.time - pauseStartTime > pauseTime)
                {
                    pauseAction();
                }
            }
        }
    }

    public struct AnimationStateForZMJ
    {
        public Vector3 time;
        public Vector3 step;
        public Vector2 distance;
        public Vector2 targetIndex;
        public int width;
        public float actionPertime;
        public float stopTime;
        public float startTime;

        public AnimationStateForZMJ(Vector3 time, Vector3 step, Vector2 distance, Vector2 targetIndex, int width, float actionPertime, float stoptime)
        {
            this.time = time;
            this.step = step;
            this.distance = distance;
            this.targetIndex = targetIndex;
            this.width = width;
            this.actionPertime = actionPertime;
            this.stopTime = stoptime;
            startTime = 0;
        }
    }
}
