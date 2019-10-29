using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    Animator dizuoAnim;
    Animator dizuozhouAnim;
    public Animator qianbiAnim;
    public Animator qianbizhouAnim;
    // Use this for initialization
    void Start () {
        dizuoAnim = transform.Find("dizuo").GetComponent<Animator>();
        dizuozhouAnim = transform.Find("dizuo").Find("dizuozhou_3").GetComponent<Animator>();
        //qianbiAnim = transform.Find("qianbi_1").GetComponent<Animator>();
        //qianbizhouAnim = transform.Find("qianbi_1").Find("qianbizhou_2").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(dizuoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (Input.GetKey(KeyCode.Alpha1))
        {
            qianbizhouAnim.CrossFade("FaceGuard", 1f);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            qianbizhouAnim.CrossFade("FaceGuardUF", 1f);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            dizuoAnim.Play("BracketMove");
            qianbiAnim.Play("TelescopicGirder");
            //dizuoAnim.CrossFade("BracketMove", 1f);
            //qianbiAnim.CrossFade("TelescopicGirder", 1f);
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            dizuoAnim.CrossFade("BracketMove2", 1f);
            qianbiAnim.CrossFade("TelescopicGirder", 1f);
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            if (!qianbiAnim.enabled)
            {
                qianbiAnim.enabled = true;
            }
            qianbiAnim.CrossFade("TelescopicGirder", 1f);
        }

        if (Input.GetKey(KeyCode.Alpha6))
        {
            if (!qianbiAnim.enabled)
            {
                qianbiAnim.enabled = true;
            }
            qianbiAnim.CrossFade("TelescopicGirderH", 0f);
        }

        if (Input.GetKey(KeyCode.Alpha7))
        {
            dizuozhouAnim.CrossFade("Rota",1.0f);
        }

        if (Input.GetKey(KeyCode.Alpha8))
        {
            dizuozhouAnim.CrossFade("Rota1", 1.0f);
        }

        if (!isStop)
        {
            stopAnim(dizuoAnim, "BracketMove", 0.5f);
            stopAnim(qianbiAnim, "TelescopicGirder", 0.5f);
        }

    }

    bool isStop = false;
    void stopAnim(Animator anim,string aniName,float endTime)
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName(aniName))
        {
            if (animInfo.normalizedTime > endTime)
            {
                anim.enabled = false;
                isStop = true;
            }
        }
    }
}
