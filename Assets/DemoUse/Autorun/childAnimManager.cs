using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class childAnimManager : MonoBehaviour {

    List<GameObject> bracketList = new List<GameObject>();
    int maxNum = 55;

    // Use this for initialization
    void Start () {
        for (int i = 1; i <= maxNum; i++)
        {
            GameObject go = GameObject.Find("Bracket" + i);
            bracketList.Add(go);
        }

        InitZNCM();

        MyEventSystem.AddListenter("BeginMove", FaceGuardIn);
        MyEventSystem.AddListenter("Step2", Step2);
        MyEventSystem.AddListenter("Step3", Step3);
        MyEventSystem.AddListenter("Step4", Step4);
        MyEventSystem.AddListenter("ZNCMrestart", Restart);
    }

    private void OnDestroy()
    {
        MyEventSystem.RemoveListener("BeginMove", FaceGuardIn);
        MyEventSystem.RemoveListener("Step2", Step2);
        MyEventSystem.RemoveListener("Step3", Step3);
        MyEventSystem.RemoveListener("Step4", Step4);
        MyEventSystem.RemoveListener("ZNCMrestart", Restart);
    }

    void Restart(object obj)
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject go = bracketList[i].transform.Find("dizuo").gameObject;

            childAnim childScript = go.GetComponent<childAnim>();
            childScript.InitChild();
        }
        InitZNCM();
    }

    void InitZNCM()
    {
        InitHolder(new Vector2(0, 16), new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.6f, 0.8f, 1.0f, 1.0f, 1.0f });

        float[] holderArray = new float[maxNum - 16];
        for (int i = 0; i < maxNum - 16; i++)
        {
            holderArray[i] = 1.0f;
        }

        StartCoroutine(Delay());
        InitHolder(new Vector2(16, maxNum), holderArray);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
        InitFaceGuard(new Vector2(6, 12));
    }

    int m = 48;
    int n = 43;
    void Step4(object obj)
    {
        //if (m+4<=56)
        //{
        //    PlayTele(new Vector2(m, m + 4), new float[] { 1.0f, 0.8f, 0.6f, 0.4f, 0.2f });
        //    m++;
        //}
        //else
        //{
        //    PlayTele(new Vector2(52, 56), new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f });
        //}

        if (m<=54)
        {
            PlayTele(m);
            m++;
        }

        FaceGuardController(n);
        n++;
    }

    void Step3(object obj)
    {
        int curIndex = 2;
        if (obj != null)
        {
            curIndex = (int)obj;
        }

        if (curIndex + 10<= maxNum)
        {
            InitFaceGuard(new Vector2(curIndex + 4, curIndex + 10));
        }

        if (curIndex - 3>=0)
        {
            //PlayTele(new Vector2(curIndex + 1, curIndex - 3), new float[] { 1.0f, 0.8f, 0.6f, 0.4f, 0.2f });
            PlayTele(curIndex - 3);
        }
        
        if (curIndex - 8> 0)
        {
            FaceGuardController(curIndex - 8);
        }

        if (curIndex - 11 > 0&& (curIndex - 10)%2==0&&curIndex<=46)
        {
            if (curIndex % 4 == 0)
            {
                fourBracket2(curIndex - 11);
                fourBracket2(curIndex - 9);
            }
            else /*if ((curIndex - 1) % 4 == 0)*/
            {
                fourBracket2(curIndex - 12);
                fourBracket2(curIndex - 10);
            }
        }

    }
    void Step2(object obj)
    {
        int curIndex = 2;
        if (obj != null)
        {
            curIndex = (int)obj;
        }

        if (curIndex - 6>=0)
        {
            InitFaceGuard(new Vector2(curIndex , curIndex - 6));
        }
    }
    void FaceGuardIn(object obj)
    {
        int curIndex=2;
        if (obj!=null)
        {
            curIndex = (int)obj;
        }

        if (curIndex - 4>=0&&(curIndex-4)%2==0)
        {
            if (curIndex%4==0)
            {
                fourBracket(curIndex - 3);
                fourBracket(curIndex - 1);
            }
            else /*if ((curIndex-1) % 4 == 0)*/
            {
                fourBracket(curIndex - 4);
                fourBracket(curIndex-2);
            }
        }

        InitFaceGuard(new Vector2(curIndex+4, curIndex+10));
    }



    void fourBracket2(int index)
    {
        GameObject child = bracketList[index - 1].transform.Find("dizuo").gameObject;
        childAnim childScript = child.GetComponent<childAnim>();
        if (childScript == null)
        {
            childScript = child.AddComponent<childAnim>();
        }

        childScript.BracketMove2();
    }
    void fourBracket(int index)
    {
        GameObject child = bracketList[index-1].transform.Find("dizuo").gameObject;
        childAnim childScript = child.GetComponent<childAnim>();
        if (childScript == null)
        {
            childScript = child.AddComponent<childAnim>();
        }

        childScript.BracketMove();
    }

    //互帮
    void FaceGuardController(int index)
    {
        if (index-1 >= 0)
        {
            GameObject child = bracketList[index - 1].transform.Find("dizuo").gameObject;
            childAnim childScript = child.GetComponent<childAnim>();
            if (childScript == null)
            {
                childScript = child.AddComponent<childAnim>();
            }

            childScript.UnfoldFaceGuard();
            if (index - 1 < 54)
            {
                Cutter2DHuBan(index - 1, true);
            }
        }
    }


    void InitFaceGuard(Vector2 targetindex)
    {
        int start = (int)Mathf.Min(targetindex.x, targetindex.y);
        int end = (int)Mathf.Max(targetindex.x, targetindex.y);
        for (int i = start; i < end; i++)
        {
            GameObject child = bracketList[i].transform.Find("dizuo").gameObject;
            childAnim childScript = child.GetComponent<childAnim>();
            if (childScript == null)
            {
                childScript = child.AddComponent<childAnim>();
            }
            if (!childScript.isFold)
            {
                childScript.FoldFaceGuard();
                if (i<54)
                {
                    Cutter2DHuBan(i, false);
                }
                
            }

        }
    }

    void Cutter2DHuBan(int index, bool isFold)
    {
        GameObject f = GameObject.Find("HuBang").transform.GetChild(index).gameObject;
        Animation ani = f.GetComponent<Animation>();
        if (!isFold)
        {
            ani[ani.clip.name].speed = 1;
            ani[ani.clip.name].normalizedTime = 0;
            ani.Play();
        }
        else
        {
            ani[ani.clip.name].speed = -1;
            ani[ani.clip.name].normalizedTime = 1;
            ani.Play();
        }
    }

    void InitHolder(Vector2 targetindex, float[] extent)
    {
        int start = (int)Mathf.Min(targetindex.x, targetindex.y);
        int end = (int)Mathf.Max(targetindex.x, targetindex.y);

        int k = 0;
        for (int i = start; i < end; i++)
        {
            GameObject child = bracketList[i].transform.Find("dizuo").gameObject;
            childAnim childScript = child.GetComponent<childAnim>();
            if (childScript==null)
            {
                childScript = child.AddComponent<childAnim>();
            }

            if (k <= extent.Length)
            {
                childScript.curNormalizedTime = extent[k];
                k++;
            }
            else
            {
                childScript.curNormalizedTime = 0;
            }
        }
    }

    //伸缩臂向前伸
    void PlayTele(Vector2 targetindex, float[] extent)
    {
        int start = (int)Mathf.Min(targetindex.x, targetindex.y);
        int end = (int)Mathf.Max(targetindex.x, targetindex.y);

        int k = 0;
        for (int i = start; i < end; i++)
        {
            GameObject child = bracketList[i-1].transform.Find("dizuo").gameObject;
            childAnim childScript = child.GetComponent<childAnim>();
            if (childScript == null)
            {
                childScript = child.AddComponent<childAnim>();
            }

            if (k <= extent.Length)
            {
                childScript.startPlay();
                childScript.curNormalizedTime = extent[k];
                k++;
            }
            else
            {
                childScript.curNormalizedTime = 0;
            }
        }
    }

    void PlayTele(int index)
    {
        GameObject child = bracketList[index].transform.Find("dizuo").gameObject;
        childAnim childScript = child.GetComponent<childAnim>();
        if (childScript == null)
        {
            childScript = child.AddComponent<childAnim>();
        }

        childScript.qianbiStretch();
    }
}
