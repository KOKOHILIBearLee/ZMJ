using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cutter2DAnimation : MonoBehaviour
{

    public static Cutter2DAnimation singleton;

    bool descending;

    public Vector3 cutterPosition;

    public GameObject startPoint;
    public GameObject endPoint;

    public GameObject uiStartPoint;
    public GameObject uiEndPoint;

    public float scale = 1;

    public float scalecutterrotation = 1;
    public float scalehubang = 1;
    public float scaleshensuobi = 1;
    public float scaleshensuobi1 = 1;
    public float scalestrack = 1;

    float shensuobi1FirstStageAxis_Y = 11.1f;
    float shensuobi1FirstStageScale = 1.655f;
    float shensuobi1SecondStageAxis_y = 201.1f;
    float shensuobi1SecondStageScale = 2;

    float trackFirstStageAxis_Y = 0;
    float trackFirstStageScale = 2;
    float trackSecondStageAxis_Y = -380;
    float trackSecondStageScale = 1;


    GameObject realCutter;
    public GameObject realTracksFather;
    public List<GameObject> realTracks;

    public GameObject realHuBangFather;
    List<GameObject> realHuBangs;

    public GameObject realShensuobiFather;
    List<GameObject> realShensuobi;

    public GameObject realShensuobi1Father;
    List<GameObject> realShensuobi1;

    List<List<GameObject>> mineGroup;

    public GameObject mineWall;
    public GameObject cutter;

    public GameObject screen;

    public bool playing = true;

    CutterControler control1;
    RectTransform control1Trans;
    CutterControler control2;
    RectTransform control2Trans;

    List<GameObject> mineWalls;
    List<GameObject> tracks;
    List<GameObject> huban;
    List<GameObject> shensuobi;
    List<GameObject> shensuobi1;
    BoundsRect actualRect;
    BoundsRect uiRect;

    public GameObject mineWallsAll;
    List<GameObject> mineWallsAllLocate = new List<GameObject>();

    Material minwallmat;

    RectTransform Cutter1_FirstStageKey1;
    RectTransform Cutter1_FirstStageKey2;
    RectTransform Cutter1_SecondStageKey1;
    RectTransform Cutter1_SecondStageKey2;

    RectTransform Cutter2_FirstStageKey1;
    RectTransform Cutter2_FirstStageKey2;
    RectTransform Cutter2_SecondStageKey1;
    RectTransform Cutter2_SecondStageKey2;
    RectTransform Cutter1StartPoint;
    RectTransform Cutter2StartPoint;

    List<Vector3> CutterStageKeyDefault = new List<Vector3>();
    private void Awake()
    {
        singleton = this;

        Cutter1StartPoint = mineWall.transform.Find("Image/Cutter1StartPoint").GetComponent<RectTransform>();
        Cutter2StartPoint = mineWall.transform.Find("Image/Cutter2StartPoint").GetComponent<RectTransform>();

        Cutter1_FirstStageKey1 = mineWall.transform.Find("Image/Cutter1_FirstStageKey1").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter1_FirstStageKey1.transform.localPosition);

        Cutter1_FirstStageKey2 = mineWall.transform.Find("Image/Cutter1_FirstStageKey2").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter1_FirstStageKey2.transform.localPosition);

        Cutter1_SecondStageKey1 = mineWall.transform.Find("Image/Cutter1_SecondStageKey1").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter1_SecondStageKey1.transform.localPosition);

        Cutter1_SecondStageKey2 = mineWall.transform.Find("Image/Cutter1_SecondStageKey2").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter1_SecondStageKey2.transform.localPosition);

        Cutter2_FirstStageKey1 = mineWall.transform.Find("Image/Cutter2_FirstStageKey1").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter2_FirstStageKey1.transform.localPosition);

        Cutter2_FirstStageKey2 = mineWall.transform.Find("Image/Cutter2_FirstStageKey2").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter2_FirstStageKey2.transform.localPosition);

        Cutter2_SecondStageKey1 = mineWall.transform.Find("Image/Cutter2_SecondStageKey1").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter2_SecondStageKey1.transform.localPosition);

        Cutter2_SecondStageKey2 = mineWall.transform.Find("Image/Cutter2_SecondStageKey2").GetComponent<RectTransform>();
        CutterStageKeyDefault.Add(Cutter2_SecondStageKey2.transform.localPosition);


        mineWallsAll = mineWall.transform.GetChild(0).gameObject;
        minwallmat = mineWallsAll.GetComponent<Image>().material;
        mineWallsAllLocate.Add(mineWallsAll.transform.Find("RightUp").gameObject);
        mineWallsAllLocate.Add(mineWallsAll.transform.Find("RightDown").gameObject);
        mineWallsAllLocate.Add(mineWallsAll.transform.Find("LeftDown").gameObject);
        mineWallsAllLocate.Add(mineWallsAll.transform.Find("LeftUp").gameObject);

        Visible(false);
        // gameObject.SetActive(false);
        RecoveryCutterStagePosition();
    }

    private void Start()
    {
        InitilizeTracks();
        InitilizeRealTracks();
    }

    public static GameObject FindHideChildGameObject(GameObject parent, string childName)
    {
        if (parent.name == childName)
        {
            return parent;
        }
        if (parent.transform.childCount < 1)
        {
            return null;
        }
        GameObject obj = null;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject go = parent.transform.GetChild(i).gameObject;
            obj = FindHideChildGameObject(go, childName);
            if (obj != null)
            {
                break;
            }
        }
        return obj;
    }

    public void InitilizeTracks()
    {
        tracks = new List<GameObject>();
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            GameObject result = GameObject.Find("Sketator" + i);
            if (result != null)
                tracks.Add(result);
        }

        //互帮
        huban = new List<GameObject>();
        for (int i = 1; i < Fuzhi.mounts+2; i++)
        {
            GameObject result = new GameObject();
            if (SOManager.singleton.currSceneName=="ZNCM_G")
            {
                GameObject parent = GameObject.Find("Bracket" + i);
                result = FindHideChildGameObject(parent, "huabanghouban"); //qianbizhou_2
            }
            else
            {
                result = GameObject.Find("SSmallest3" + i);
            }

            if (result != null)
                huban.Add(result);
        }

        //伸缩臂
        shensuobi = new List<GameObject>();
        for (int i = 1; i < Fuzhi.mounts+2; i++)
        {
            GameObject result = new GameObject();
            if (SOManager.singleton.currSceneName == "ZNCM_G")
            {
                GameObject parent = GameObject.Find("Bracket" + i);
                result = FindHideChildGameObject(parent, "qianbi_1");
            }
            else
            {
                result = GameObject.Find("Max3" + i);
                result = result.transform.Find("FalseMax2/UpArm").gameObject;
            }

            if (result != null)
                shensuobi.Add(result);
        }

        //伸缩臂父物体
        shensuobi1 = new List<GameObject>();
        for (int i = 1; i < Fuzhi.mounts+2; i++)
        {
            GameObject result = new GameObject();
            if (SOManager.singleton.currSceneName == "ZNCM_G")
            {
                GameObject parent = GameObject.Find("Bracket" + i);
                result = FindHideChildGameObject(parent, "shangbi_1");// dizuo
            }
            else
            {
                result = GameObject.Find("Max3" + i);
                result = result.transform.Find("FalseMax2").gameObject;
            }
            if (result != null)
                shensuobi1.Add(result);
        }
        print("");
    }

    public void InitilizeRealTracks()
    {
        realTracks = new List<GameObject>();
        if (realTracksFather != null)
            foreach (Transform tran in realTracksFather.transform)
                realTracks.Add(tran.gameObject);

        realHuBangs = new List<GameObject>();
        if (realHuBangFather != null)
            foreach (Transform tran in realHuBangFather.transform)
                realHuBangs.Add(tran.gameObject);

        realShensuobi = new List<GameObject>();
        if (realShensuobiFather != null)
            foreach (Transform tran in realShensuobiFather.transform)
                realShensuobi.Add(tran.gameObject);

        realShensuobi1 = new List<GameObject>();
        if (realShensuobi1Father != null)
            foreach (Transform tran in realShensuobi1Father.transform)
                realShensuobi1.Add(tran.gameObject);
    }

    List<Vector2> originalregion = new List<Vector2>(new Vector2[] { new Vector2(-0.1f, -0.1f), new Vector2(1.1f, -0.1f), new Vector2(1, 0.5f), new Vector2(0, 0.5f) });
    private void Update()
    {
        if (!playing)
            return;

        RecoveryZAxis();
        if (control1Trans == null && control1 != null)
            control1Trans = control1.gameObject.GetComponent<RectTransform>();
        if (control2Trans == null && control2 != null)
            control2Trans = control2.gameObject.GetComponent<RectTransform>();


        if (control1Trans != null)
        {
            control1Trans.anchoredPosition3D = cutterPosition;
            control1Trans.localEulerAngles = Vector3.zero;
        }
        if (control2Trans != null)
        {
            control2Trans.anchoredPosition3D = cutterPosition;
            control2Trans.localEulerAngles = Vector3.zero;
        }

        InitilizeRect();
        Refresh();
        DescendingUpdate();


        minwallmat.ClearRegion(false);

        // minwallmat.AddRenderCullHoleRegion(originalregion, CommonZheng.RegionType.Region);
        UpdateAniamtion().ForEach(gf =>
        {
            minwallmat.AddRenderCullHoleRegion(gf, CommonZheng.RegionType.Region);
        });
    }

    public float basepoint;
    public float baserate;

    void Refresh()
    {
        if (actualRect != null && uiRect != null)
        {
            if (cutter != null && realCutter != null)
            {
                Vector2 local = uiRect.NormalPointToCapPoint(actualRect.CapPointToNormalPoint(realCutter.transform.position));
                cutter.transform.localPosition = new Vector3(local.x, local.y * scale, 0);
                Vector3 euler = realCutter.transform.eulerAngles;
                cutter.transform.localEulerAngles = new Vector3(0, 0, -euler.y * scalecutterrotation);
                if (screen != null)
                    transform.position = screen.transform.position - cutter.transform.position + transform.position;
            }
            if (realTracks != null)
                for (int i = 0; i < realTracks.Count; i++)
                {
                    if (tracks != null)
                        try
                        {
                            Vector2 local = uiRect.NormalPointToCapPoint(actualRect.CapPointToNormalPoint(tracks[i].transform.position));
                            realTracks[i].transform.localPosition = new Vector3(local.x, local.y * scalestrack, 0);
                        }
                        catch { }
                }

            if (realHuBangs != null)
                for (int i = 0; i < realHuBangs.Count; i++)
                {
                    if (huban != null)
                        try
                        {
                            Vector2 local = uiRect.NormalPointToCapPoint(actualRect.CapPointToNormalPoint(huban[i].transform.position));
                            realHuBangs[i].transform.localPosition = new Vector3(local.x, local.y * scalehubang, 0);
                            //realHuBangs[i].transform.localScale = new Vector3(1, (realHuBangs[i].transform.localPosition.y - basepoint) * baserate, 1);
                        }
                        catch { }
                }

            if (realShensuobi != null)
                for (int i = 0; i < realShensuobi.Count; i++)
                {
                    if (shensuobi != null)
                        try
                        {
                            Vector2 local = uiRect.NormalPointToCapPoint(actualRect.CapPointToNormalPoint(shensuobi[i].transform.position));
                            realShensuobi[i].transform.localPosition = new Vector3(local.x, local.y * scaleshensuobi, 0);
                        }
                        catch { }
                }

            if (realShensuobi1 != null)
                for (int i = 0; i < realShensuobi1.Count; i++)
                {
                    if (shensuobi1 != null)
                        try
                        {
                            Vector2 local = uiRect.NormalPointToCapPoint(actualRect.CapPointToNormalPoint(shensuobi1[i].transform.position));
                            realShensuobi1[i].transform.localPosition = new Vector3(local.x, local.y * scaleshensuobi1, 0);
                        }
                        catch { }
                }
        }
    }

    int startDescedingIndex = -1;
    int currentDescedingIndex = -1;
    public int stepdistance;
    public int maxdepth = 5;
    public float descendoffset = 0;

    public void RecoveryCutterStagePosition()
    {
        Cutter1_FirstStageKey1.transform.localPosition = CutterStageKeyDefault[0];
        Cutter1_FirstStageKey2.transform.localPosition = CutterStageKeyDefault[1];
        Cutter1_SecondStageKey1.transform.localPosition = CutterStageKeyDefault[2];
        Cutter1_SecondStageKey2.transform.localPosition = CutterStageKeyDefault[3];
        Cutter2_FirstStageKey1.transform.localPosition = CutterStageKeyDefault[4];
        Cutter2_FirstStageKey2.transform.localPosition = CutterStageKeyDefault[5];
        Cutter2_SecondStageKey1.transform.localPosition = CutterStageKeyDefault[6];
        Cutter2_SecondStageKey2.transform.localPosition = CutterStageKeyDefault[7];
    }

    public void RecoveryZAxis()
    {
        Vector3 p = Cutter1_FirstStageKey1.transform.localPosition;
        Cutter1_FirstStageKey1.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter1_FirstStageKey2.transform.localPosition;
        Cutter1_FirstStageKey2.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter1_SecondStageKey1.transform.localPosition;
        Cutter1_SecondStageKey1.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter1_SecondStageKey2.transform.localPosition;
        Cutter1_SecondStageKey2.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter2_FirstStageKey1.transform.localPosition;
        Cutter2_FirstStageKey1.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter2_FirstStageKey2.transform.localPosition;
        Cutter2_FirstStageKey2.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter2_SecondStageKey1.transform.localPosition;
        Cutter2_SecondStageKey1.transform.localPosition = new Vector3(p.x, p.y, 0);

        p = Cutter2_SecondStageKey2.transform.localPosition;
        Cutter2_SecondStageKey2.transform.localPosition = new Vector3(p.x, p.y, 0);
    }

    public List<Vector2> GetLines(GameObject target1, GameObject target2)
    {
        Vector3 point = mineWallsAllLocate[2].transform.position;// + mineWallsAllLocate[3].transform.position) / 2;
        Vector3 diretion = mineWallsAllLocate[0].transform.position - mineWallsAllLocate[2].transform.position;
        Quaternion ReferenceRotation = Quaternion.FromToRotation(Vector3.right, mineWallsAllLocate[1].transform.position - mineWallsAllLocate[2].transform.position);
        return Normal(point, diretion, target1, target2, ReferenceRotation);
    }

    void DescendingUpdate()
    {
        if (!descending || cutter == null)
            return;

        Ray ray = new Ray(cutter.transform.position + cutter.transform.right * descendoffset, cutter.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int index = FindIndexMineGroup(hit.collider.gameObject);
            if (index < 0)
                return;
            if (startDescedingIndex < 0)
                startDescedingIndex = index;
            currentDescedingIndex = index;
            DescendingRefresh();
        }
    }

    void DescendingRefresh()
    {
        int step = startDescedingIndex < currentDescedingIndex ? 1 : -1;
        int temstart = startDescedingIndex;
        if (stepdistance < 1)
            stepdistance = 1;
        int hideladder = 0;
        while (temstart != currentDescedingIndex)
        {
            List<GameObject> result = mineGroup[temstart];

            if (hideladder > maxdepth)
            {
                SetDescending(false);
                return;
            }

            for (int i = 0; i < hideladder; i++)
                CutterControler.HideGame(result[result.Count - 1 - i]);

            if ((startDescedingIndex - temstart) % stepdistance == 0)
                hideladder++;

            temstart = temstart + step;
        }
    }

    int FindIndexMineGroup(GameObject game)
    {
        int result = -1;
        for (int i = 0; i < mineGroup.Count; i++)
            if (mineGroup[i].Contains(game))
                return i;
        return result;
    }

    void InitilizeRect()
    {
        if (startPoint == null || endPoint == null || uiStartPoint == null || uiEndPoint == null)
            Debug.LogError("标识点为空无法创建地图");
        else
        {
            actualRect = new BoundsRect(startPoint.transform.position, endPoint.transform.position, true);
            uiRect = new BoundsRect(uiStartPoint.transform.localPosition, uiEndPoint.transform.localPosition, false);
        }
    }

    public void SetCutter1(bool val)
    {
        if (control1 != null)
            control1.enabled = val;
    }

    public void SetCutter2(bool val)
    {
        if (control2 != null)
            control2.enabled = val;
    }

    public void SetDescending(bool val, float desoffset = float.MaxValue)
    {
        descending = val;
        startDescedingIndex = -1;
        currentDescedingIndex = -1;
        if (desoffset != float.MaxValue)
            descendoffset = desoffset;
    }

    public void InitilizeAll(GameObject realcutter)
    {
        if (cutter != null)
        {
            try
            {
                control1 = cutter.transform.GetChild(0).gameObject.GetComponent<CutterControler>();
                control1.enabled = false;
            }
            catch { }
            try
            {
                control2 = cutter.transform.GetChild(1).gameObject.GetComponent<CutterControler>();
                control2.enabled = false;
            }
            catch { }
        }
        if (SOManager.singleton.currSceneName == "ZNCM_G")
            Visible(true);

        //gameObject.SetActive(true);
        realCutter = realcutter;
        InitilizeFather(true);
        InitilizeRect();
        InitilizeTracks();
        InitilizeRealTracks();
    }

    public void InitilizeFather(bool onoff = false)
    {
        if (mineWall != null)
        {
            mineGroup = new List<List<GameObject>>();
            int count = mineWall.GetComponent<GridLayoutGroup>().constraintCount;
            mineWalls = new List<GameObject>();
            foreach (Transform tran in mineWall.transform)
            {
                mineWalls.Add(tran.gameObject);
                Image image = tran.GetComponent<Image>();
                Collider2D collider = tran.GetComponent<Collider2D>();
                if (image != null)
                    image.enabled = true;
                if (collider != null)
                    collider.enabled = onoff;

                if (mineGroup.Count == 0)
                    mineGroup.Add(new List<GameObject> { tran.gameObject });
                else
                {
                    List<GameObject> last = mineGroup[mineGroup.Count - 1];
                    if (last.Count < count)
                        last.Add(tran.gameObject);
                    else
                        mineGroup.Add(new List<GameObject>() { tran.gameObject });
                }
            }
        }
    }

    List<Vector2> Normal(Vector3 point, Vector3 direction, GameObject target1, GameObject target2, Quaternion rotation)
    {
        direction = Quaternion.Inverse(rotation) * direction;

        List<Vector2> res1 = new List<Vector2>();

        List<GameObject> regions = new List<GameObject>();

        regions.Add(target1);
        regions.Add(target2);

        for (int i = 0; i < regions.Count; i++)
        {
            Vector3 result = -point + regions[i].transform.position;
            result = Quaternion.Inverse(rotation) * result;
            Vector2 resss = new Vector2(result.x / direction.x, result.y / direction.y);
            res1.Add(resss);
        }

        res1.Add(new Vector2(res1[1].x - 0.0001f, -0.1f));
        res1.Add(new Vector2(res1[0].x - 0.0002f, -0.2f));

        return res1;
    }

    public void SwitchScenceInitilize()
    {
        RecoveryCutterStagePosition();
        InitilizeAnimation();
    }

    public void InitilizeAnimation()
    {
        cutter1firstonoff = false;
        cutter1firstline = false;
        cutter1secondonoff = false;
        cutter1secondline = false;
        cutter1thirdonoff = false;
        cutter1thirdline = false;


        cutter2firstonoff = false;
        cutter2firstline = false;
        cutter2secondonoff = false;
        cutter2secondline = false;
        cutter2thirdonoff = false;
        cutter2thirdline = false;

        ShenSuoBi1FirstStage();
        TrackFirstStage();
        scalecutterrotation = 2;

        minwallmat.ClearRegion();

        //CastTransfrom.singleton.InitilizeAnimation();
    }

    public void ShenSuoBi1FirstStage()
    {
        RectTransform ssb1tran = realShensuobi1Father.GetComponent<RectTransform>();
        if (ssb1tran != null)
        {
            Vector3 posi = ssb1tran.anchoredPosition;
            ssb1tran.anchoredPosition = new Vector2(posi.x, shensuobi1FirstStageAxis_Y);
            scaleshensuobi1 = shensuobi1FirstStageScale;
        }
    }

    public void TrackFirstStage()
    {
        RectTransform rect = realTracksFather.GetComponent<RectTransform>();

        if (rect != null)
        {
            Vector2 v = rect.anchoredPosition;
            rect.anchoredPosition = new Vector2(v.x, trackFirstStageAxis_Y);
            scalestrack = trackFirstStageScale;
        }
    }

    public void TrackSecondStage()
    {
        RectTransform rect = realTracksFather.GetComponent<RectTransform>();

        if (rect != null)
        {
            Vector2 v = rect.anchoredPosition;
            rect.anchoredPosition = new Vector2(v.x, trackSecondStageAxis_Y);
            scalestrack = trackSecondStageScale;
        }
    }

    public void ShenSuoBi1SecondStage()
    {
        RectTransform ssb1tran = realShensuobi1Father.GetComponent<RectTransform>();
        if (ssb1tran != null)
        {
            Vector3 posi = ssb1tran.anchoredPosition;
            ssb1tran.anchoredPosition = new Vector2(posi.x, shensuobi1SecondStageAxis_y);
            scaleshensuobi1 = shensuobi1SecondStageScale;
        }
    }

    #region cutter1
    bool cutter1firstonoff = false;
    bool cutter1firstline = false;
    public void StartCutter1FirstStageAnimation()
    {
        cutter1firstonoff = true;
        cutter1firstline = true;
        Vector3 dir = /*control1.rightindication.position*/Cutter1StartPoint.position - Cutter1_FirstStageKey1.position;
        Cutter1_FirstStageKey1.position += dir;
        Cutter1_FirstStageKey2.position += dir;
    }

    public List<Vector2> UpdateCutter1First()
    {
        try
        {
            Plane plane = new Plane(mineWallsAll.transform.right, control1.leftindication.position);
            Ray ray = new Ray(Cutter1_FirstStageKey1.position, Cutter1_FirstStageKey2.transform.position - Cutter1_FirstStageKey1.transform.position);
            float enter;
            plane.Raycast(ray, out enter);
            if (cutter1firstonoff)
                Cutter1_FirstStageKey2.position = ray.GetPoint(enter);
            if (cutter1firstline)
                return GetLines(Cutter1_FirstStageKey1.gameObject, Cutter1_FirstStageKey2.gameObject);
            else
                return null;
        }
        catch
        {
            //Debug.LogError("报错了：UpdateCutter1First");
            return null;
        }
    }


    bool cutter1secondonoff = false;
    bool cutter1secondline = false;
    public void StartCutter1SecondStageAnimation()
    {
        cutter1secondonoff = true;
        cutter1secondline = true;
        cutter1firstonoff = false;



        Vector3 dir = Cutter1_FirstStageKey2.position - Cutter1_SecondStageKey1.position;



        Cutter1_SecondStageKey1.position += dir;
        Cutter1_SecondStageKey2.position += dir;
    }

    public void StopCutter1SecondStageAnimation()
    {

        cutter1secondonoff = false;
    }

    public List<Vector2> UpdateCutter1Second()
    {
        try
        {
            Vector3 indication = control1.leftindication.position;

            Plane plane = new Plane(mineWallsAll.transform.right, indication);
            Ray ray = new Ray(Cutter1_SecondStageKey1.position, Cutter1_SecondStageKey2.transform.position - Cutter1_SecondStageKey1.transform.position);
            float enter;
            plane.Raycast(ray, out enter);
            if (cutter1secondonoff || cutter1thirdonoff)
                Cutter1_SecondStageKey2.position = ray.GetPoint(enter);
            if (cutter1secondline || cutter1thirdline)
                return GetLines(Cutter1_SecondStageKey1.gameObject, Cutter1_SecondStageKey2.gameObject);
            else
                return null;
        }
        catch
        {
            return null;
        }
    }


    bool cutter1thirdonoff = false;
    bool cutter1thirdline = false;
    public void StartCutter1ThirdStageAnimation()
    {

        StopCutter1SecondStageAnimation();
        cutter1thirdonoff = true;
        cutter1thirdline = true;
    }

    public void StopCutter1ThirdStageAnimation()
    {
        cutter1thirdonoff = false;
    }
    #endregion

    #region cutter2

    bool cutter2firstonoff = false;
    bool cutter2firstline = false;
    public void StartCutter2FirstStageAnimation()
    {

        cutter2firstonoff = true;
        cutter2firstline = true;
        Vector3 dir = /*control2.rightindication.position*/Cutter2StartPoint.position - Cutter2_FirstStageKey2.position;
        Cutter2_FirstStageKey1.position += dir;
        Cutter2_FirstStageKey2.position += dir;
    }

    public List<Vector2> UpdateCutter2First()
    {
        Plane plane = new Plane(mineWallsAll.transform.right, control2.leftindication.position);
        Ray ray = new Ray(Cutter2_FirstStageKey1.position, Cutter2_FirstStageKey2.transform.position - Cutter2_FirstStageKey1.transform.position);
        float enter;
        plane.Raycast(ray, out enter);
        if (cutter2firstonoff)
            Cutter2_FirstStageKey2.position = ray.GetPoint(enter);
        if (cutter2firstline)
            return GetLines(Cutter2_FirstStageKey1.gameObject, Cutter2_FirstStageKey2.gameObject);
        else
            return null;
    }


    bool cutter2secondonoff = false;
    bool cutter2secondline = false;
    public void StartCutter2SecondStageAnimation()
    {

        cutter2secondonoff = true;
        cutter2secondline = true;
        cutter2firstonoff = false;



        Vector3 dir = Cutter2_FirstStageKey2.position - Cutter2_SecondStageKey1.position;



        Cutter2_SecondStageKey1.position += dir;
        Cutter2_SecondStageKey2.position += dir;
    }

    public void StopCutter2SecondStageAnimation()
    {

        cutter2secondonoff = false;
    }

    public List<Vector2> UpdateCutter2Second()
    {
        Vector3 indication = Vector3.zero;
        if (cutter2secondonoff)
            indication = control2.leftindication.position;
        if (cutter2thirdonoff)
            indication = control2.rightindication.position;

        Plane plane = new Plane(mineWallsAll.transform.right, indication);
        Ray ray = new Ray();

        if (cutter2secondonoff)
            ray = new Ray(Cutter2_SecondStageKey1.position, Cutter2_SecondStageKey2.transform.position - Cutter2_SecondStageKey1.transform.position);
        if (cutter2thirdonoff)
            ray = new Ray(Cutter2_SecondStageKey2.position, Cutter2_SecondStageKey1.transform.position - Cutter2_SecondStageKey2.transform.position);

        float enter;
        plane.Raycast(ray, out enter);
        if (cutter2secondonoff)
            Cutter2_SecondStageKey2.position = ray.GetPoint(enter);
        if (cutter2thirdonoff)
            Cutter2_SecondStageKey1.position = ray.GetPoint(enter);
        if (cutter2secondline || cutter2thirdline)
            return GetLines(Cutter2_SecondStageKey1.gameObject, Cutter2_SecondStageKey2.gameObject);
        else
            return null;
    }


    bool cutter2thirdonoff = false;
    bool cutter2thirdline = false;
    public void StartCutter2ThirdStageAnimation()
    {

        StopCutter2SecondStageAnimation();
        cutter2thirdonoff = true;
        cutter2thirdline = true;
    }

    public void StopCutter2ThirdStageAnimation()
    {

        cutter2thirdonoff = false;
    }

    #endregion

    public List<List<Vector2>> UpdateAniamtion()
    {
        List<List<Vector2>> result = new List<List<Vector2>>();
        try
        {
            List<Vector2> cutter1first = UpdateCutter1First();
            List<Vector2> cutter1second = UpdateCutter1Second();
            List<Vector2> cutter2first = UpdateCutter2First();
            List<Vector2> cutter2second = UpdateCutter2Second();


            if (cutter1first != null)
                result.Add(cutter1first);
            if (cutter1second != null)
                result.Add(cutter1second);
            if (cutter2first != null)
                result.Add(cutter2first);
            if (cutter2second != null)
                result.Add(cutter2second);
        }
        catch { }
        if (result.Count == 0)
            result = new List<List<Vector2>>(new List<Vector2>[] { new List<Vector2>(new Vector2[] { new Vector2(10, 10), new Vector2(11, 11), new Vector2(20, 21) }) });
        return result;//new List<List<Vector2>>(new List<Vector2>[] { new List<Vector2>(new Vector2[] { new Vector2(0, 0), new Vector2(0.5f, 0.5f), new Vector2(0.1f, 1) }) });
    }

    public void Visible(bool val, Transform tran = null, bool setplay = true)
    {
        if (setplay)
            playing = val;
        if (tran == null)
            tran = transform;
        foreach (Transform c in tran)
        {
            Image img = c.GetComponent<Image>();
            if (img != null)
                img.enabled = val;
            Visible(val, c, setplay);
        }
    }

    public bool GetVisible(Transform tran = null)
    {

        Transform minefloor = transform.Find("MineWall/Image");
        if (minefloor != null)
        {
            Image img = minefloor.GetComponent<Image>();
            if (img != null)
                return img.enabled;
        }
        //if (tran == null)
        //    tran = transform;
        //foreach (Transform c in tran)
        //{
        //    Image img = c.GetComponent<Image>();
        //    if (img != null)
        //        if (img.enabled)
        //            return true;
        //    if (GetVisible(c))
        //        return true;
        //}
        return false;
    }

    public static void InitilizeHuBang2D(int start, int end)
    {
        try
        {
            GameObject f = GameObject.Find("HuBang");
            int s = Mathf.Min(start, end);
            int e = Mathf.Max(start, end);

            for (int i = 0; i < f.transform.childCount; i++)
            {
                try
                {
                    Animation ani = f.transform.GetChild(i).GetComponent<Animation>();
                    if (i >= s && i <= e)
                    {
                        ani[ani.clip.name].speed = 1;
                        ani[ani.clip.name].normalizedTime = 1;
                        ani.Play();
                    }
                    else
                    {
                        ani[ani.clip.name].speed = -1;
                        ani[ani.clip.name].normalizedTime = 0;
                        ani.Play();
                    }
                }
                catch { }
            }
        }
        catch
        {

        }
    }
}

public class BoundsRect
{
    Vector2 center;
    public Vector2 Center
    {
        get
        {
            return center;
        }
    }

    Vector2 size;
    public Vector2 Size
    {
        get
        {
            return size;
        }
    }

    public BoundsRect(Vector2 point1, Vector2 point2)
    {
        InitlizeByVectore2(point1, point2);
    }

    void InitlizeByVectore2(Vector2 point1, Vector2 point2)
    {
        center = (point1 + point2) / 2;
        size = new Vector2(Mathf.Abs(point1.x - point2.x), Mathf.Abs(point1.y - point2.y));
    }

    public BoundsRect(Vector3 point1, Vector3 point2, bool is3d)
    {
        Vector2 pointv1 = Vector2.zero;
        Vector2 pointv2 = Vector2.zero;
        if (is3d)
        {
            pointv1 = new Vector2(point1.x, point1.z);
            pointv2 = new Vector2(point2.x, point2.z);
        }
        else
        {
            pointv1 = new Vector2(point1.x, point1.y);
            pointv2 = new Vector2(point2.x, point2.y);
        }
        InitlizeByVectore2(pointv1, pointv2);
    }

    public Vector2 NormalPointToCapPoint(Vector2 normalpoint)
    {
        Vector2 direction = new Vector2(normalpoint.x * size.x, normalpoint.y * size.y);
        return center + direction;
    }

    public Vector2 CapPointToNormalPoint(Vector2 cappoint)
    {
        Vector2 direction = cappoint - center;
        Vector2 normalpoint = new Vector2(direction.x / size.x, direction.y / size.y);
        return normalpoint;
    }

    public Vector2 CapPointToNormalPoint(Vector3 cappoint)
    {
        Vector2 cappointv = new Vector2(cappoint.x, cappoint.z);
        Vector2 direction = cappointv - center;
        Vector2 normalpoint = new Vector2(direction.x / size.x, direction.y / size.y);
        return normalpoint;
    }
}