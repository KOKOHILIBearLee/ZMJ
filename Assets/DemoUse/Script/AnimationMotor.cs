using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimationMotor
{
   // [SerializeField]
    public List<GameObject> keyFrameGameobjects = new List<GameObject>();
    public float distance;
    public float speed;
    public float creatSpeed;
    public PlayMode playMode;
    public GameObject templetGameobject;
    public bool playing;
    public System.Action finishiAction;
    public Action<int> indexChange;

    MotorUnit singleGame = null;
    List<MotorUnit> motorUnits = new List<MotorUnit>();
   
    float creatTime;

    public AnimationMotor()
    {
        //CreatSingle();
    }

    public void InitializeTemplet()
    {
        singleGame = CreatMotorUnit(templetGameobject, 1);
    }

    MotorUnit CreatMotorUnit(GameObject game, int index)
    {
        MotorUnit mu = new MotorUnit();
        try
        {
            mu.id = index;
            mu.moveGameobject = game;
            mu.start = keyFrameGameobjects[mu.id - 1];
            mu.end = keyFrameGameobjects[mu.id];
            mu.CurrentDistance = 0;
            return mu;
        }
        catch (System.Exception e)
        {
            return null;
        }
    }

    void CreatSingle(int index = 1)
    {
        CreatMotorUnit(templetGameobject, index);
    }

    public class MotorUnit
    {
        public GameObject moveGameobject;
        public GameObject start;
        public GameObject end;
        float allDistance { get { return (start.transform.position - end.transform.position).magnitude; } }
        public float AllDistance { get { return allDistance; } }
        float currentDistance;
        public float CurrentDistance
        {
            get { return currentDistance; }
            set
            {
                currentDistance = value;
                if (value < 0)
                    currentDistance = 0;
                if (value > allDistance)
                    currentDistance = allDistance;
                Refresh();
            }
        }

        public int id;

        void Refresh()
        {
            Ray ray = new Ray(start.transform.position, end.transform.position - start.transform.position);
            moveGameobject.transform.position = ray.GetPoint(currentDistance);
            moveGameobject.transform.forward = end.transform.position - start.transform.position;
        }
    }

    public void Update()
    {
        if (playing)
        {
            if (speed < 0)
                speed = 0;
            if (playMode == PlayMode.FixedCreatSpeed || playMode == PlayMode.FixedDistance)
            {
                if (keyFrameGameobjects.Count >= 2)
                {
                    CreatElement();
                    if (playMode == PlayMode.FixedCreatSpeed)
                        UpdateMoveFixedCreatSpeed();
                    if (playMode == PlayMode.FixedDistance)
                        UpdateMoveFixedDistance();
                }
            }
            if (playMode == PlayMode.SingleLoop || playMode == PlayMode.SingleOnce || playMode == PlayMode.SinglePingPong)
                UpdateMoveSingle();
        }
    }

    void DestroyGameobject()
    {
        int rem = 0;
        for (int i = 0; i < motorUnits.Count; i++)
        {
            MotorUnit m = motorUnits[i];
            float currentdis = m.CurrentDistance;
            currentdis += speed * Time.deltaTime;
            if (currentdis > m.AllDistance && m.id >= keyFrameGameobjects.Count - 1)
            {
                rem++;
                continue;
            }
            break;
        }

        while (rem > 0)
        {
            GameObject.Destroy(motorUnits[0].moveGameobject);
            motorUnits.RemoveAt(0);
            rem--;
        }
    }

    void UpdateMoveFixedDistance()
    {
        DestroyGameobject();
        List<MotorUnit> removeRest = new List<MotorUnit>();
        if (motorUnits.Count > 0)
        {
            float newdis = motorUnits[0].AllDistance - motorUnits[0].CurrentDistance - speed * Time.deltaTime;
            if (newdis < 0)
            {
                motorUnits[0].id++;
                motorUnits[0].start = keyFrameGameobjects[motorUnits[0].id - 1];
                motorUnits[0].end = keyFrameGameobjects[motorUnits[0].id];
                motorUnits[0].CurrentDistance = Mathf.Abs(newdis);
            }
            else
            {
                motorUnits[0].CurrentDistance += speed * Time.deltaTime;
            }
        }
        if (motorUnits.Count > 1)
            for (int i = 1; i < motorUnits.Count; i++)
            {
                TargetCreatPara para = GetKeyFrameID(motorUnits[i - 1], distance);
                if (para.id > 0)
                {

                    motorUnits[i].id = para.id;
                    motorUnits[i].start = keyFrameGameobjects[para.id - 1];
                    motorUnits[i].end = keyFrameGameobjects[para.id];
                    motorUnits[i].CurrentDistance = para.distance;
                }
                else
                    removeRest.Add(motorUnits[i]);
            }
        while (removeRest.Count > 0)
        {
            GameObject.Destroy(removeRest[0].moveGameobject);
            motorUnits.Remove(removeRest[0]);
            removeRest.RemoveAt(0);
        }
    }

    void UpdateMoveFixedCreatSpeed()
    {
        DestroyGameobject();

        for (int i = 0; i < motorUnits.Count; i++)
        {
            float movedistance = speed * Time.deltaTime;
            float targetdistance = motorUnits[i].CurrentDistance + movedistance - motorUnits[i].AllDistance;
            if (targetdistance > 0)
            {
                motorUnits[i].id++;
                motorUnits[i].start = keyFrameGameobjects[motorUnits[i].id - 1];
                motorUnits[i].end = keyFrameGameobjects[motorUnits[i].id];
                motorUnits[i].CurrentDistance = targetdistance;
            }
            else
                motorUnits[i].CurrentDistance += movedistance;
        }
    }

    void UpdateMoveSingle()
    {
        if (singleGame != null)
        {
            float movedistance = speed * Time.deltaTime;
            float targetdistance = singleGame.CurrentDistance + movedistance - singleGame.AllDistance;
            if (targetdistance > 0)
            {
                if (singleGame.id < keyFrameGameobjects.Count - 1)
                {
                    singleGame.id++;
                    singleGame.start = keyFrameGameobjects[singleGame.id - 1];
                    singleGame.end = keyFrameGameobjects[singleGame.id];
                    singleGame.CurrentDistance = targetdistance;
                    if (indexChange != null)
                        indexChange(singleGame.id);
                }
                else if (playMode == PlayMode.SingleLoop)
                    CreatSingle();
                else if (playMode == PlayMode.SinglePingPong)
                {
                    List<GameObject> rem = new List<GameObject>();
                    for (int i = keyFrameGameobjects.Count - 1; i >= 0; i--)
                        rem.Add(keyFrameGameobjects[i]);
                    keyFrameGameobjects = rem;
                    CreatSingle();
                }
                else if (singleGame.AllDistance < singleGame.CurrentDistance)
                    singleGame.CurrentDistance = singleGame.AllDistance;
                else
                {
                    playing = false;
                    if (finishiAction != null)
                        finishiAction();
                }
            }
            else
                singleGame.CurrentDistance += movedistance;
        }
    }

    void CreatFirst()
    {
        GameObject game =UnityEngine.Object.Instantiate<GameObject>(templetGameobject);
        motorUnits.Add(CreatMotorUnit(game, 1));
    }

    void CreatElement()
    {
        if (templetGameobject == null)
            return;
        if (motorUnits.Count == 0)
        {
            CreatFirst();
        }
        else
        {
            if (playMode == PlayMode.FixedDistance)
            {
                int i = 10;
                while (i > 1)
                {
                    MotorUnit father = motorUnits[motorUnits.Count - 1];
                    TargetCreatPara para = GetKeyFrameID(father, distance);
                    if (para.id > 0)
                    {
                        GameObject game = UnityEngine.Object.Instantiate<GameObject>(templetGameobject);
                        MotorUnit mu = new MotorUnit();
                        mu.moveGameobject = game;
                        mu.start = keyFrameGameobjects[para.id - 1];
                        mu.end = keyFrameGameobjects[para.id];
                        mu.CurrentDistance = para.distance;
                        mu.id = para.id;
                        motorUnits.Add(mu);
                    }
                    i = para.id;
                }
            }
            if (playMode == PlayMode.FixedCreatSpeed)
            {
                if (Time.time - creatTime > creatSpeed)
                {
                    creatTime = Time.time;
                    CreatFirst();
                }
            }
        }
    }

    TargetCreatPara GetKeyFrameID(MotorUnit m, float dis)
    {
        float dist = m.CurrentDistance - dis;
        int rid = m.id;

        if (dist > 0)
        {
            return new TargetCreatPara(m.id, dist);
        }
        else
        {
            while (dist < 0)
            {
                rid--;
                if (rid < 1)
                    return new TargetCreatPara(-1, dis);
                dist += (keyFrameGameobjects[rid].transform.position - keyFrameGameobjects[rid - 1].transform.position).magnitude;
            }
            return new TargetCreatPara(rid, dist);
        }
    }

    //void ZMJIniKeyFrane()
    //{
    //    keyFrameGameobjects = new List<GameObject>();
    //    for (int i = 0; i < Fuzhi.mounts; i++)
    //    {
    //        GameObject game = GameObject.Find("SLKEY" + i);
    //        if (game != null)
    //            keyFrameGameobjects.Add(game);
    //    }
    //}

    //void ZMJIniSL()
    //{
    //    int ladder = 0;
    //    string elementName = "SSLL";
    //    if (elementName == null || elementName == "")
    //        return;
    //    for (int i = 0; i < Fuzhi.mounts * 6; i++)
    //    {
    //        GameObject game = GameObject.Find(elementName + i);
    //        if (game == null)
    //            continue;
    //        Vector3 gameposition = game.transform.position;
    //        for (int k = ladder; k < keyFrameGameobjects.Count - 1; k++)
    //        {
    //            Vector3 position1 = keyFrameGameobjects[k].transform.position;
    //            Vector3 position2 = keyFrameGameobjects[k + 1].transform.position;

    //            if (Vector3.Dot(position1 - gameposition, position2 - gameposition) < 0.1)
    //            {
    //                MotorUnit pair = new MotorUnit();
    //                pair.id = k + 1;
    //                pair.start = keyFrameGameobjects[k];
    //                pair.end = keyFrameGameobjects[k + 1];
    //                pair.moveGameobject = game;
    //                pair.CurrentDistance = 0;
    //                motorUnits.Add(pair);
    //                ladder = k;
    //                break;
    //            }
    //        }
    //    }
    //    List<MotorUnit> list = new List<MotorUnit>();
    //    for (int i = motorUnits.Count - 1; i >= 0; i--)
    //        list.Add(motorUnits[i]);
    //    motorUnits = list;
    //}

    struct TargetCreatPara
    {
        public float distance;
        public int id;

        public TargetCreatPara(int id, float dis)
        {
            this.id = id;
            this.distance = dis;
        }
    }

    void RefreshPostionAndRotateByID(MotorUnit m)
    {
        //m.
    }

    public static int IsBetweenPoint(Vector3 backpoint, Vector3 frontpoint, Vector3 normal, Vector3 targetpoint)
    {
        if (normal != Vector3.zero)
        {
            if (Vector3.Dot((frontpoint - backpoint), normal) < 0)
                normal = -normal;

            Plane plmiddle = new Plane(normal, targetpoint);
            Plane back = new Plane(normal, backpoint);
            bool backside = back.GetSide(targetpoint);
            if (!plmiddle.SameSide(backpoint, frontpoint))
                return 0;
            else if (backside)
                return 1;
            else
                return -1;
        }
        else
        {
            normal = frontpoint - backpoint;
            if (normal == Vector3.zero)
                return -2;
            return IsBetweenPoint(backpoint, frontpoint, normal, targetpoint);
        }
    }

    public enum PlayMode
    {
        FixedCreatSpeed,
        FixedDistance,
        SingleLoop,
        SingleOnce,
        SinglePingPong,
    }
}
