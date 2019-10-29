using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutMineAnimation : MonoBehaviour
{
    AnimationMotor animation = null;
    GameObject templetGameobject;
    static CutMineAnimation single;
    public string cutterName;
    public string father;
    public string child;
    public Vector2 way1;
    public Vector2 way2;
    public Vector2 way3;
    public Vector2 way4;
    public Vector2 way5;
    public Vector4 pauseTime;
    public bool bway1 = false;
    public bool bway2 = false;
    public bool bway3 = false;
    public bool bway4 = false;

    public float speed;
    public List<KEY> keys;

    [System.Serializable]
    public struct KEY
    {
        // [SerializeField]
        public List<GameObject> keyGame;
    }

    public static CutMineAnimation singleton
    {
        get
        {
            return single;
        }
    }

    private void Awake()
    {
        single = this;
    }

    private void Update()
    {
        try
        {
            if (animation != null)
                animation.Update();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("播放CutterAnimation动画失败");
        }
    }

    public void Stop()
    {
        animation = null;
    }

    public void PlayCutterAnimation(List<GameObject> keys, GameObject cutter, int direction, float speed, float height = 0,System.Action action = null, System.Action<int> indexchange = null)
    {
        animation = new AnimationMotor();
        animation.keyFrameGameobjects = keys;
        animation.playMode = AnimationMotor.PlayMode.SingleOnce;
        if (templetGameobject == null)
            templetGameobject = new GameObject("CutterAnimationTEMPLET");
        Transform cchild = templetGameobject.transform.Find(cutterName);
        if (cchild != null)
            cutter.transform.parent = templetGameobject.transform.parent;
        templetGameobject.transform.parent = cutter.transform.parent;
        animation.templetGameobject = templetGameobject;
        animation.InitializeTemplet();
        animation.speed = speed;
        cutter.transform.parent = templetGameobject.transform;
        cutter.transform.localPosition = new Vector3(0, height, 0);
        cutter.transform.localEulerAngles = new Vector3(0, 90 * (direction > 0 ? 1 : -1), 0);
        animation.playing = transform;
        animation.finishiAction = action;
        animation.indexChange = indexchange;
    }

    public void PlayCutterAnimation(int index, GameObject cutter,int direciotn, float speed, float height = 0)
    {
        List<GameObject> keyanimation;
        try
        {
            keyanimation = keys[index].keyGame;
        }
        catch (System.Exception e)
        {
            Debug.LogError("不能存在索引" + index);
            return;
        }

        PlayCutterAnimation(keyanimation, cutter, direciotn,speed, height);
    }

    public void PlayCutterAnimation(Vector2 range, GameObject cutter, int direction, float speed, float height = 0, System.Action action = null, System.Action<int> indexchange = null)
    {
        if (father == null || father == "" || child == null || child == "")
        {
            Debug.LogError("CutAnimation未发现合法的物体名称");
            return;
        }
        List<GameObject> results = new List<GameObject>();
        int start = (int)range.x;
        int end = (int)range.y;
        bool order = end - start > 0 ? true : false;
        end = order ? end + 1 : end - 1;
        while (start != end)
        {
            GameObject f = GameObject.Find(father + start);
            GameObject c = FindChildAll(f, child);
            if (c != null)
                results.Add(c);
            start = order ? start + 1 : start - 1;
        }
        if (results.Count < 2)
        {
            Debug.LogError("关键帧节点不足");
            return;
        }
        PlayCutterAnimation(results, cutter, direction, speed, height, action, indexchange);
    }
    
    public static GameObject FindChildAll(GameObject father, string childname)
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
