using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyFrameAnimation : MonoBehaviour
{
    public List<GameObject> keyFrameGameobjects = new List<GameObject>();
    public List<GameObject> ZMJEnd = new List<GameObject>();
    List<MotorUnit> motorUnits = new List<MotorUnit>();
    public float distance;

    public bool playing = true;

    public float speed;

    public GameObject templetGameobject;

    public static bool SSLLJ = false;

    bool IsSSLL = false;

    public static bool once = true;

    GameObject parentGo;

    private void Start()
    {
        ZMJIniKeyFrane();
        IsSSLL = this.gameObject.name == "SSLLjingZhi";
        if (!IsSSLL)
            ZMJIniSL();

        parentGo = GameObject.Find("SLFather");
    }

    public class MotorUnit
    {
        public GameObject moveGameobject;
        public GameObject start;
        public GameObject end;
        float allDistance { get { return (start.transform.position - end.transform.position).magnitude; }}
        public float AllDistance { get { return allDistance; } }
        float currentDistance;
        public float CurrentDistance { get { return currentDistance; }
            set {
                currentDistance = value;
                if (value < 0)
                    currentDistance = 0;
                if (value > allDistance)
                    currentDistance = allDistance;
                Refresh();
            } }
        //public float CurrentDistance { get { return currentDistance; } }
        public int id;

        public void Refresh()
        {
            Ray ray = new Ray(start.transform.position, end.transform.position - start.transform.position);
            moveGameobject.transform.position = ray.GetPoint(currentDistance);
           // print(start.transform.position + "   " + (end.transform.position - start.transform.position) + "   " + ray.GetPoint(currentDistance) + "   " + currentDistance);
            moveGameobject.transform.forward = end.transform.position - start.transform.position;
            //MonoBehaviour.print("ssasa    "+moveGameobject.transform.position);
        }
    }

    private void Update()
    {
        if (SSLLJ && IsSSLL)
        {
            speed = 0;
            if (once)
            {
                once = false;
                MotorUnit mu = new MotorUnit();
                mu.id = keyFrameGameobjects.Count - 1;
                mu.moveGameobject = templetGameobject;
                mu.start = keyFrameGameobjects[keyFrameGameobjects.Count - 2];
                mu.end = keyFrameGameobjects[keyFrameGameobjects.Count - 1];
                mu.CurrentDistance = 0;
            }
            if (playing)
            {
                CreatElement();
                UpdateMove();
            }
        }
        else if (IsSSLL)
        {
            while (motorUnits.Count > 0)
            {
                Destroy(motorUnits[0].moveGameobject);
                motorUnits.RemoveAt(0);
            }
        }
        else
        {
            if (playing)
            {
                CreatElement();
                UpdateMove();
            }
        }
    }

    void UpdateMove()
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
            Destroy(motorUnits[0].moveGameobject);
            motorUnits.RemoveAt(0);
            rem--;
        }

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
                TargetCreatPara para = GetKeyFrameID(motorUnits[i-1], distance);
                if (para.id > 0)
                {

                    motorUnits[i].id = para.id;
                    motorUnits[i].start = keyFrameGameobjects[para.id - 1];
                    motorUnits[i].end = keyFrameGameobjects[para.id];
                    motorUnits[i].CurrentDistance = para.distance;
                }
            }
    }


    void CreatElement()
    {
        if (templetGameobject == null)
            return;
        if (motorUnits.Count == 0)
        {
            GameObject game = Object.Instantiate<GameObject>(templetGameobject);
            if (parentGo!=null)
            {
                game.transform.parent = parentGo.transform;
            }
            MotorUnit mu = new MotorUnit();
            mu.moveGameobject = game;
            mu.start = keyFrameGameobjects[0];
            mu.end = keyFrameGameobjects[1];
            mu.CurrentDistance = 0;
            mu.id = 1;
            motorUnits.Add(mu);
        }
        else
        {
            int i = 10;
            while (i > 1)
            {
                MotorUnit father = motorUnits[motorUnits.Count - 1];
                TargetCreatPara para = GetKeyFrameID(father, distance);
                if (para.id > 0)
                {
                    GameObject game = Object.Instantiate<GameObject>(templetGameobject);
                    GameObject parentGo = GameObject.Find("SLFather");
                    if (parentGo != null)
                    {
                        game.transform.parent = parentGo.transform;
                    }
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
           // print("CreatElement__11");
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

    void ZMJIniKeyFrane()
    {
      //  keyFrameGameobjects = new List<GameObject>();
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            GameObject game = GameObject.Find("SLKEY" + i);
            if (game != null)
                keyFrameGameobjects.Add(game);
        }
        for (int i = 0; i < ZMJEnd.Count; i++)
        {
            keyFrameGameobjects.Add(ZMJEnd[i]);
        }
    }

    void ZMJIniSL()
    {
        int ladder = 0;
        string elementName = "SSLL";
        if (elementName == null || elementName == "")
            return;
        for (int i = 0; i < Fuzhi.mounts * 6; i++)
        {
            GameObject game = GameObject.Find(elementName + i);
            if (game == null)
                continue;
            Vector3 gameposition = game.transform.position;
            for (int k = ladder; k < keyFrameGameobjects.Count - 1; k++)
            {
                Vector3 position1 = keyFrameGameobjects[k].transform.position;
                Vector3 position2 = keyFrameGameobjects[k + 1].transform.position;

                if (Vector3.Dot(position1 - gameposition, position2 - gameposition) < 0.1)
                {
                    MotorUnit pair = new MotorUnit();
                    pair.id = k + 1;
                    pair.start = keyFrameGameobjects[k];
                    pair.end = keyFrameGameobjects[k + 1];
                    pair.moveGameobject = game;
                    pair.CurrentDistance = 0;
                    motorUnits.Add(pair);
                    ladder = k;
                    break;
                }
            }
        }
        List<MotorUnit> list = new List<MotorUnit>();
        for (int i = motorUnits.Count - 1; i >= 0; i--)
            list.Add(motorUnits[i]);
        motorUnits = list;
    }

    public struct TargetCreatPara
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
}
