using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PluginEvent;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class reset : MonoBehaviour {
    public string child;
    public GameObject CMJGo;

    public float CMJSpeed = 0.5f;
    List<GameObject> BanList = new List<GameObject>();
    List<GameObject> BanChildList = new List<GameObject>();
    int maxNUM = 54;
    GameObject mineWall = null;

    Animator leftArmAni;
    Animator rightArmAim;
    MakeAnimation SLFather;

    Vector3 penwuposition = new Vector3(0, -0.0008f, 0);
    Vector3 penwurotation = new Vector3(270, 0, 0); 
    Vector3 penwuscale = new Vector3(0.01016f, 0.01016f, 0.01016f); //(0.0004f, 0.0004f, 0.0004f)
    Vector3 CMJGoPos = new Vector3(47.3f, -2.8f, -54.8f);

    List<GameObject> penwuParticleGroup1 = new List<GameObject>();
    List<GameObject> penwuParticleGroup2 = new List<GameObject>();
    // Use this for initialization
    void Start () {
        GameObject p = Resources.Load<GameObject>("PenWu");
        p.SetActive(false);

        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup1.Add(GameObject.Instantiate<GameObject>(p));

        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));
        penwuParticleGroup2.Add(GameObject.Instantiate<GameObject>(p));

        SLFather = GameObject.Find("SLFather").GetComponent<MakeAnimation>();
        SLFather.state = MakeAnimation.AnimationState.play;
        
        leftArmAni = GameObject.Find("钻头机械臂001").GetComponent<Animator>();
        rightArmAim = GameObject.Find("钻头机械臂002").GetComponent<Animator>();

        for (int i = 0; i < maxNUM; i++)
        {
            GameObject go = GameObject.Find("Ban" + i);

            if (go != null)
            {
                BanList.Add(go);
                GameObject childGo = FindChildAll(go, child);
                if (childGo != null)
                {
                    BanChildList.Add(childGo);
                }
            }
        }

        InitZNCM();
        MyEventSystem.AddListenter("ZNCMrestart",Restart);
        
    }

    void Restart(object obj)
    {
        DestroyWall.singleton.control1 = false;
        DestroyWall.singleton.control2 = false;
        leftArmAni.Play("New State");
        leftArmAni.Update(0);
        rightArmAim.Play("New State");
        rightArmAim.Update(0);

        isFirst = true;
        InitZNCM();
        IntilizeZJParticle();

        for (int i = 0; i < maxNUM; i++)
        {
            GameObject go= GameObject.Find("Sketator" + i);
            ZhuAnimation zhu = go.GetComponent<ZhuAnimation>();
            if (zhu!=null)
            {
                Destroy(zhu);
            }
        }
    }

    void InitZNCM()
    {
        m_Time = 0;
        IniMineWall();
        float[] sketor = new float[54];
        for (int i = 0; i < sketor.Length; i++)
        {
            sketor[i] = 0.5f;
        }
        ZhengScripts.InitilizeSketator(new Vector2(0, 53), sketor);
        ZhengScripts.InitilizeSketator(new Vector2(0, 17), new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.5f, 0.5f ,0.5f});
        ZhuAnimation.PlayAnimation(new Vector2(0, 17), 0.1f,
            new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f ,0.5f}, ZhuAnimation.ZhuAnimationMode.Mode3);

        curStep = typeofStep.None;
        CMJGo.transform.position = CMJGoPos;//BanChildList[2].transform.position
        curIndex = 2;

        KeyFrameAnimation.SSLLJ = true;
        if (Cutter2DAnimation.singleton != null)
        {
            Cutter2DAnimation.singleton.InitilizeAll(GameObject.Find("CMJ"));
        }

        //斜切入刀
        CastTransfrom.singleton.InitilizeAnimation();
        if (Cutter2DAnimation.singleton.playing)
        {
            CastTransfrom.singleton.CutKnife();
            CastTransfrom.singleton.ToLeft();
        }
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
            GameObject fa = GameObject.Find("Bracket" + (i + startindex));
            if (fa != null)
            {
                GameObject g = Fuzhi.FindChildAll(fa, "qianbi_1");

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
            GameObject fa = GameObject.Find("Bracket" + (i + startindex));
            if (fa != null)
            {
                GameObject g = Fuzhi.FindChildAll(fa, "qianbi_1");

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

    // Update is called once per frame
    void Update () {

        reflashRota();
    }

    private void FixedUpdate()
    {
        CMJMove();
    }

    public static int curIndex = 2;
    float distance;
    enum typeofStep
    {
        None,
        step1,
        step2,
        step3,
        step4,
        step5,
    }

    int count = 10;
    typeofStep curStep;
    bool isFirst = true;
    float m_Time = 0;
    void CMJMove()
    {
        m_Time += Time.deltaTime;
        if (isFirst && m_Time>=3f)
        {
            leftArmAni.CrossFade("LeftArm_DH", 0f);
            rightArmAim.CrossFade("RightArm_DH", 0f);
            isFirst = false;
            m_Time = 0f;
            curStep = typeofStep.step1;
        }

        if (curStep == typeofStep.step1 && m_Time >= 4.2f) //第一次割煤
        {
            //yield return new WaitForSeconds(4.2f);
            if (curIndex <= 18)
            {
                distance = (CMJGo.transform.position - BanChildList[curIndex].transform.position).magnitude;
                CMJGo.transform.forward = BanChildList[curIndex].transform.position - BanChildList[curIndex - 1].transform.position;
                //Vector3 Dir = BanChildList[curIndex].transform.position - BanChildList[curIndex - 1].transform.position;
                //CMJGo.transform.forward = Vector3.Lerp(CMJGo.transform.forward, Dir, Time.deltaTime);
                CMJGo.transform.Translate(CMJGo.transform.forward * CMJSpeed * Time.deltaTime, Space.World);

                if (distance <= 0.4f)
                {
                    Cutter2D(curIndex);
                    curIndex++;
                    MyEventSystem.DispatchEvent("BeginMove", curIndex);
                }
            }
            else
            {
                CastTransfrom.singleton.LeftArmDown();
                CastTransfrom.singleton.RightArmUp();
                curStep = typeofStep.step2;
                m_Time = 0f;
                //割三角煤
                ZhuAnimation.PlayAnimation(new Vector2(16, 0), 0.5f, new float[] { 0.5f }, ZhuAnimation.ZhuAnimationMode.Mode2);
                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.MiddleVisible(false);
                    CastTransfrom.singleton.HintVisible(false);
                    Cutter2DAnimation.singleton.StopCutter1SecondStageAnimation();
                    Cutter2DAnimation.singleton.StopCutter2SecondStageAnimation();
                }
                leftArmAni.CrossFade("LeftArm_DHR", 0f);
                rightArmAim.CrossFade("RightArm_DHR", 0f);

                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.CutTri();
                    CastTransfrom.singleton.ToRight();
                }
            }
        }
        else if (curStep == typeofStep.step2 && m_Time >= 4.2f) //第二次割煤--割三角煤
        {
            //yield return new WaitForSeconds(4.2f);

            if (curIndex >= 3)
            {
                distance = (CMJGo.transform.position - BanChildList[curIndex - 1].transform.position).magnitude;
                CMJGo.transform.Translate(-CMJGo.transform.forward * CMJSpeed * Time.deltaTime, Space.World);

                if (curIndex == 15)
                {
                    if (Cutter2DAnimation.singleton.playing)
                        Cutter2DAnimation.singleton.StartCutter2ThirdStageAnimation();
                }

                //Debug.Log(distance);
                if (distance <= 0.4f)
                {
                    SetGroup1Particle(curIndex + 5);
                    SetGroup2Particle(curIndex - 4);
                    curIndex--;
                    MyEventSystem.DispatchEvent("Step2", curIndex);
                }
            }
            else
            {
                CastTransfrom.singleton.LeftArmUp();
                CastTransfrom.singleton.RightArmDown();
                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.MiddleVisible(false);
                    CastTransfrom.singleton.HintVisible(false);
                    Cutter2DAnimation.singleton.StopCutter2ThirdStageAnimation();
                }
                //割底煤
                leftArmAni.CrossFade("LeftArm_DH", 0f);
                rightArmAim.CrossFade("RightArm_DH", 0f);

                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.ToLeft();
                    CastTransfrom.singleton.CutDown();
                }
                curStep = typeofStep.step3;
                m_Time = 0f;
            }
        }
        else if (curStep == typeofStep.step3 && m_Time >= 4.2f)//第三次割煤--割底煤--中部跟机
        {
            //yield return new WaitForSeconds(4.2f);

            if (curIndex < BanChildList.Count - 1)
            {
                distance = (CMJGo.transform.position - BanChildList[curIndex].transform.position).magnitude;
                CMJGo.transform.Translate(CMJGo.transform.forward * CMJSpeed * Time.deltaTime, Space.World);

                //Debug.Log(distance);
                if (distance <= 0.4f)
                {
                    SetGroup1Particle(curIndex - 3);
                    SetGroup2Particle(curIndex - 3 + 9);

                    curIndex++;

                    //中部跟机
                    if (curIndex == 19)
                    {
                        //SetDT_YJQDTextContent("中部跟机");
                        if (Cutter2DAnimation.singleton.playing)
                        {
                            CastTransfrom.singleton.FollowMiddle();
                            Cutter2DAnimation.singleton.StartCutter1ThirdStageAnimation();
                        }
                    }
                    if (curIndex == 20 )
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(curIndex - 20, curIndex - 11), 0.1f,
                            new float[] { 1.0f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                    }

                    if (curIndex > 20 && curIndex % 10 == 0)
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(curIndex - 29, curIndex - 11), 0.1f,
                            new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f }, ZhuAnimation.ZhuAnimationMode.Mode3); //, 1.0f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f
                        //ZhuAnimation.PlayAnimation(new Vector2(curIndex - 20, curIndex - 11), 0.1f,
                        //    new float[] { 1.0f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                    }

                    MyEventSystem.DispatchEvent("Step3", curIndex);
                }
            }
            else
            {
                CastTransfrom.singleton.LeftArmDown();
                leftArmAni.CrossFade("LeftArm_DHR", 0f);

                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.MiddleVisible(false);
                    CastTransfrom.singleton.HintVisible(false);
                    Cutter2DAnimation.singleton.StopCutter1ThirdStageAnimation();
                }
                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.CutDown();
                    CastTransfrom.singleton.ToRight();
                }
                curStep = typeofStep.step4;
                m_Time = 0f;
            }
        }
        else if (curStep == typeofStep.step4 && m_Time >= 4.2f) //第四次割煤--割底煤
        {

            if (curIndex > 42)
            {
                distance = (CMJGo.transform.position - BanChildList[curIndex - 1].transform.position).magnitude;
                CMJGo.transform.Translate(-CMJGo.transform.forward * CMJSpeed * Time.deltaTime, Space.World);

                if (distance <= 0.4f)
                {
                    SetGroup1Particle(curIndex + 5);
                    SetGroup2Particle(curIndex - 4);

                    curIndex--;
                    MyEventSystem.DispatchEvent("Step4");
                }
            }
            else
            {
                if (Cutter2DAnimation.singleton.playing)
                {
                    CastTransfrom.singleton.MiddleVisible(false);
                    CastTransfrom.singleton.HintVisible(false);
                }
                    curStep = typeofStep.step5;
                m_Time = 0f;
            }
        }
        else if (curStep == typeofStep.step5 && m_Time >= 0.5f) //第五次割煤--回到待机位
        {
            //yield return new WaitForSeconds(0.5f);

            if (curIndex < BanChildList.Count - 1)
            {
                distance = (CMJGo.transform.position - BanChildList[curIndex].transform.position).magnitude;
                CMJGo.transform.Translate(CMJGo.transform.forward * CMJSpeed * Time.deltaTime, Space.World);


                //Debug.Log(distance);
                if (distance <= 0.4f)
                {
                    if (curIndex == 47)
                    {
                        ZhuAnimation.PlayAnimation(new Vector2(31, 45), 0.1f,
                               new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.95f, 0.9f, 0.85f, 0.8f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f }, ZhuAnimation.ZhuAnimationMode.Mode3);
                    }
                    SetGroup1Particle(curIndex - 3);
                    SetGroup2Particle(curIndex - 3 + 9);

                    curIndex++;
                    MyEventSystem.DispatchEvent("Step5",curIndex);
                }
            }
            else
            {
                m_Time = 0f;
                MyEventSystem.DispatchEvent("ZNCMrestart");
            }
        }

    }

    void Cutter2D(int index)
    {
        SetGroup1Particle(index - 3 );
        SetGroup2Particle(index - 3 + 9);

        if (index == 5)
        {

            if (Cutter2DAnimation.singleton.playing)
                Cutter2DAnimation.singleton.StartCutter1FirstStageAnimation();
            //Cutter2DAnimation.singleton.control1.AbandonRight();
            DestroyWall.singleton.control1 = true;
            if (Cutter2DAnimation.singleton != null)
                Cutter2DAnimation.singleton.SetDescending(true, -2f);
            //if (Cutter2DAnimation.singleton != null)
            //    Cutter2DAnimation.singleton.SetCutter1(true);
            //AddParticle(0);
        }
        if (index == 9)
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
        }

        if (index == 9)
        {
            Cutter2DAnimation.singleton.scalecutterrotation = 1f;
        }


        if (index == 14)
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
    }

    void moveToLeft()
    {
        leftArmAni.SetFloat("Arm", -1);
        if (leftArmAni.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            leftArmAni.Play("LeftArm_DH", 0, 1);
        }

        rightArmAim.SetFloat("Arm", -1);
        if (rightArmAim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            rightArmAim.Play("RightArm_DH", 0, 1);
        }


        CastTransfrom.singleton.LeftArmDown();
        CastTransfrom.singleton.RightArmUp();
    }

    void moveRight()
    {
        leftArmAni.SetFloat("Arm", 1);
        if (leftArmAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0)
        {
            leftArmAni.Play("LeftArm_DH", 0, 0);
        }

        rightArmAim.SetFloat("Arm", 1);
        if (rightArmAim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0)
        {
            rightArmAim.Play("RightArm_DH", 0, 0);
        }

        CastTransfrom.singleton.LeftArmUp();
        CastTransfrom.singleton.RightArmDown();
    }

    void reflashRota()
    {
        for (int i = BanList.Count - 2; i >=1; i--)
        {
            Vector3 For = BanList[i + 1].transform.position - BanList[i].transform.position;
            BanList[i].transform.forward = Vector3.Cross(Vector3.up, For);
        }
    }

    GameObject FindChildAll(GameObject father, string childname)
    {
        if (father == null)
            return null;
        Transform resulttrans = father.transform.Find(childname);
        GameObject result = null;
        if (resulttrans != null)
            result = resulttrans.gameObject;
        if (father.transform.childCount == 0 || result != null)
            return result;
        foreach (Transform trans in father.transform)
        {
            result = FindChildAll(trans.gameObject, childname);
            if (result != null)
                return result;
        }
        return null;
    }


}
