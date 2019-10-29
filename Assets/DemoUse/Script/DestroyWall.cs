using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyWall : MonoBehaviour
{

    public List<GameObject> cutter;
    List<GameObject> target;
    public GameObject father;
    public float radius;
    public float height;
    List<GameObject> destroy = new List<GameObject>();

    GameObject particle1;
    GameObject particle2;

    public static DestroyWall singleton = null;
    //singleton
    // Use this for initialization
    public bool control1 = false;
    public bool control2 = false;

    public float onoffvalue = 0.5f;

    float all = 0;
    float yes = 0;

    private void Awake()
    {
        singleton = this;
        particle1 = GameObject.Find("LZ_BangD001").transform.Find("mei001").gameObject;
        particle2 = GameObject.Find("LZ_BangD002").transform.Find("mei001").gameObject;
    }

    void Start()
    {
        IniFather();
    }

    public void IniFather()
    {
        target = new List<GameObject>();
        try
        {
            foreach (Transform ch in father.transform)
            {
                try
                {
                    target.Add(ch.gameObject);
                }
                catch { }
            }
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (all > 50)
        {
            all = 0;
            yes = 0;
        }
        all++;
        int rem = 0;
        for (int i = 0; i < cutter.Count; i++)
        {
            for (int k = 0; k < target.Count; k++)
            {
                try
                {
                    if (((i == 0 && control1) || (i == 1 && control2)) && (height <= 0 ? IsDestroy(cutter[i], target[k]) : IsDestroyCylinder(cutter[i], target[k])))
                    {
                        yes++;
                        destroy.Add(target[k]);
                        if (rem == 0)
                        {
                            if (i == 0)
                                rem = 1;
                            if (i == 1)
                                rem = 2;
                        }
                        else if (rem == 1 && i == 1)
                            rem = 3;
                        else if (rem == 2 && i == 0)
                            rem = 3;
                    }
                }
                catch { }
            }
        }
        if (all > 10)
        {
            if (destroy.Count > 0)
            {
                if (yes / all > onoffvalue)
                {
                   ControlParticle(rem);
                }
                else
                {
                    if (rem == 1)
                    {
                        SetParticleBool(2, false);
                    }

                    if (rem == 2)
                    {
                        SetParticleBool(1, false);
                    }
                    if (rem == 0)
                    {
                        SetParticleBool(1, false);
                        SetParticleBool(2, false);
                    }
                }
            }
            else
            {
                if (yes / all < onoffvalue)
                {
                    SetParticleBool(1, false);
                    SetParticleBool(2, false);
                }
            }
        }
        while (destroy.Count > 0)
        {
            target.Remove(destroy[0]);
            Destroy(destroy[0]);
            destroy.RemoveAt(0);
        }
    }


    public void ControlParticle(int con)
    {
        if (con == 1 || con == 3)
            if (particle1 == null)
            {
                GameObject father = SOManager.singleton.FindObject("LZ_BangD001");
                particle1 = Fuzhi.FindChildAll(father, "mei001(Clone)");
                if (particle1 == null)
                    AddParticle(1);
                father = SOManager.singleton.FindObject("LZ_BangD001");
                particle1 = Fuzhi.FindChildAll(father, "mei001(Clone)");
                SetParticleBool(1, true);
            }
            else
            {
                SetParticleBool(1, true);
            }

        if (con == 2 || con == 3)
            if (particle2 == null)
            {
                GameObject father = SOManager.singleton.FindObject("LZ_BangD002");
                particle2 = Fuzhi.FindChildAll(father, "mei002(Clone)");
                if (particle2 == null)
                    AddParticle(2);
                father = SOManager.singleton.FindObject("LZ_BangD002");
                particle2 = Fuzhi.FindChildAll(father, "mei002(Clone)");
                SetParticleBool(2, true);
            }
            else
            {
                SetParticleBool(2, true);
            }
    }

    public void SetParticleBool(int swit, bool onoff)
    {
        if (particle1 != null && (swit == 1 || swit == 3))
        {
            if (particle1.activeSelf != onoff)
                particle1.SetActive(onoff);
        }
        if (particle2 != null && (swit == 2 || swit == 3))
        {
            if (particle2.activeSelf != onoff)
                particle2.SetActive(onoff);
        }
    }

    //public void SetParticle(string fathername, string gamename, bool onoff)
    //{
    //    GameObject particleF = SOManager.singleton.FindObject(fathername);
    //    if (particleF == null)
    //        return;
    //    GameObject game = Fuzhi.FindChildAll(particleF, gamename);
    //    if (game == null)
    //        return;

    //    ParticleSystem.EmissionModule module = game.GetComponent<ParticleSystem>().emission;
    //    module.enabled = onoff;
    //}

    public void AddParticle(int swit)
    {
        if (swit == 1)
            Training.TrainingManager.ExecuteScript("PlayPartical,LZ_BangD001,mei001,,-90:180:0");
        if (swit == 2)
            Training.TrainingManager.ExecuteScript("PlayPartical,LZ_BangD002,mei002,,-90:180:0");
    }

    bool IsDestroy(GameObject cutter, GameObject destorytarget)
    {
        return (destorytarget.transform.position - cutter.transform.position).magnitude <= radius;
    }

    bool IsDestroyCylinder(GameObject cutter, GameObject destorytarget)
    {
        Vector3 disvector3 = destorytarget.transform.position - cutter.transform.position;
        float h = Mathf.Abs(Vector3.Dot((cutter.transform.up.normalized), disvector3));
        return h <= height && (disvector3.sqrMagnitude - Mathf.Pow(h, 2)) <= radius * radius;
    }
}
