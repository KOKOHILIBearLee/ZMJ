using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditorInternal;
using DG.Tweening;

public class childAnim : MonoBehaviour {

    [HideInInspector] public float curNormalizedTime;
    [HideInInspector] public bool isFold = false;
    bool isStep3 = true;
    bool isStop = false;
    Animator dizuoAnim; //底座移动
    Animator dizuozhouAnim; //调整角度
    Animator qianbiAnim; //伸缩梁
    Animator qianbizhouAnim; //互帮

    AnimatorStateInfo dizuoAnimInfo;
    AnimatorStateInfo qianbiAnimInfo;

    float preNormalizedTime=0f;
    
    // Use this for initialization
    void Awake()
    {
        dizuoAnim = transform.GetComponent<Animator>();
        dizuozhouAnim = transform.Find("dizuozhou_3").GetComponent<Animator>();
        GameObject qianbi = transform.Find("dizuozhou_3").Find("xiabi_1")
            .Find("zhongzhou_1").Find("zhongbi_1").Find("shangzhou_1")
            .Find("shangbi_1").Find("qianbi_1").gameObject;
        qianbiAnim = qianbi.GetComponent<Animator>();
        qianbizhouAnim = qianbi.transform.Find("qianbizhou_2").GetComponent<Animator>();

        dizuoAnimInfo = dizuoAnim.GetCurrentAnimatorStateInfo(0);
        qianbiAnimInfo = qianbiAnim.GetCurrentAnimatorStateInfo(0);

        dizuoAnim.Play("BracketMove");
        qianbiAnim.Play("TelescopicGirder");
    }

    private void Update()
    {
        if (!isStop)
        {
            stopAnim(dizuoAnim, "BracketMove", curNormalizedTime);
            stopAnim(qianbiAnim, "TelescopicGirder", curNormalizedTime);
        }

        if (!isStep3)
        {
            stopAnim(qianbiAnim, "TelescopicGirderH", curNormalizedTime);
        }
    }

    public void InitChild()
    {
        dizuoAnim.Play("New State"); //"BracketMove", 0, 1f
        dizuoAnim.Update(0f);

        qianbiAnim.Play("New State"); //"TelescopicGirder",0,1f
        qianbiAnim.Update(0f);

        dizuozhouAnim.Play("New State"); //"rota1",0,1
        dizuozhouAnim.Update(0f);

        qianbizhouAnim.Play("New State"); //"FaceGuard",0,1
        qianbizhouAnim.Update(0f);

        isStep3 = true;
        isStop = false;
        isFold = false;
        preNormalizedTime = 0f;
        dizuoAnim.Play("BracketMove");
        qianbiAnim.Play("TelescopicGirder");
    }

    public void startPlay()
    {
        if (!qianbiAnim.enabled)
        {
            qianbiAnim.enabled = true;
        }
        qianbiAnim.Play("TelescopicGirderH",0, preNormalizedTime);
        isStep3 = false;
    }

    void stopAnim(Animator anim, string aniName, float endTime)
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName(aniName))
        {
            if (animInfo.normalizedTime > endTime &&endTime<1.0f)
            {
                anim.enabled = false;
                isStop = true;
                if (!isStep3)
                {
                    preNormalizedTime = curNormalizedTime;
                }
                isStep3 = true;
            }
        }
    }

    public void BracketMove2()
    {
        StartCoroutine(truthMove2());
    }

    IEnumerator truthMove2()
    {
        dizuozhouAnim.CrossFade("rota1", 1.0f);
        yield return new WaitForSeconds(1.5f);
        //yield return new WaitForSeconds(0.0f);
        if (!dizuoAnim.enabled)
        {
            dizuoAnim.enabled = true;
        }
        dizuoAnim.CrossFade("BracketMove2", 1f);

        if (!qianbiAnim.enabled)
        {
            qianbiAnim.enabled = true;
        }
        qianbiAnim.CrossFade("TelescopicGirder", 1f);

        yield return new WaitForSeconds(6.0f);
        dizuozhouAnim.CrossFade("rota1 0", 1.0f);
    }

    public void BracketMove()
    {
        StartCoroutine(truthMove());
    }

    IEnumerator truthMove()
    {
        dizuozhouAnim.CrossFade("rota1", 1.0f);

        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(0.0f);
        if (!dizuoAnim.enabled)
        {
            dizuoAnim.enabled = true;
        }
        dizuoAnim.CrossFade("BracketMove", 1f);

        if (!qianbiAnim.enabled)
        {
            qianbiAnim.enabled = true;
        }
        qianbiAnim.CrossFade("TelescopicGirder", 1f);

        yield return new WaitForSeconds(6.0f);
        dizuozhouAnim.CrossFade("rota1 0", 1.0f);
    }

    public void UnfoldFaceGuard()
    {
        if (!qianbizhouAnim.enabled)
        {
            qianbizhouAnim.enabled = true;
        }
        qianbizhouAnim.CrossFade("FaceGuardUF", 1f);
        isFold = false;
    }

    public void FoldFaceGuard()
    {
        if (!qianbizhouAnim.enabled)
        {
            qianbizhouAnim.enabled = true;
        }
        qianbizhouAnim.CrossFade("FaceGuard", 1f);
        isFold = true;
        Invoke("qianbiContraction", 1.5f);
    }

    void qianbiContraction()
    {
        if (!qianbiAnim.enabled)
        {
            qianbiAnim.enabled = true;
        }
        qianbiAnim.CrossFade("TelescopicGirder", 1f);
    }

    public void qianbiStretch()
    {
        if (!qianbiAnim.enabled)
        {
            qianbiAnim.enabled = true;
        }
        qianbiAnim.CrossFade("TelescopicGirderH", 1f);
    }



}



