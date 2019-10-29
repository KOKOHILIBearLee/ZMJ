using UnityEngine;
using System.Collections.Generic;

public class TextureCameraControl : MonoBehaviour {

    public static TextureCameraControl singleton = null;

    public GameObject image;
    float moveDelta = 0.0001f;

    float left = 0.06f;
    float right = -0.06f;
    float front = 0.163f;
    float back = 0.160f;

    Transform bondTransLR;
    Transform bondTransFB;

    GameObject pcb;
    float pcbWidth = 0;
    float pcbHeight = 0;
    string pcbImage = "";

    Vector3 pcbZeroPos = new Vector3();
    Vector3 pcbSize = new Vector3();

    // Use this for initialization
    void Start () {
        singleton = this;

        SetImageVisible(false);

    }

    public void BondGo(string cameraObj, string lrObj, string fbObj, string frontStr, string backStr, string leftStr, string rightStr)
    {
        GameObject camGo = SOManager.singleton.FindObject(cameraObj);
        if (camGo == null) return;

        transform.position = camGo.transform.position;
        //transform.eulerAngles = camGo.transform.eulerAngles;

        GameObject go = SOManager.singleton.FindObject(lrObj);
        if (go == null) return;
        bondTransLR = go.transform;

        go = SOManager.singleton.FindObject(fbObj);
        if (go == null) return;
        bondTransFB = go.transform;

        left = float.Parse(leftStr);
        right = float.Parse(rightStr);
        front = float.Parse(frontStr);
        back = float.Parse(backStr);

        pcbSize = new Vector3();

        InitPCB();
    }

    public void ImportPCB(float w, float h, string img)
    {
        pcbWidth = w;
        pcbHeight = h;
        pcbImage = "file:///" + img;
    }

    public void InitPCB()
    {
        pcb = GameObject.Find("PCB-1");
        if (pcb == null) return;

        if (pcbWidth != 0)
        {
            pcb.transform.localScale = new Vector3(pcbWidth / 130f, pcbHeight / 305.3f, 1f);
        }

        //加载贴图
        if (pcbImage != "")
        {
            List<SOMaterialProp> props = new List<SOMaterialProp>();
            Material material = pcb.GetComponent<Renderer>().material;
            props.Add(new SOMaterialProp(material, "_MainTex"));
            StartCoroutine(TPFileReader.LoadTexture(pcbImage, props));
        }

        pcb.AddComponent<BoxCollider>();
        pcbSize = pcb.GetComponent<BoxCollider>().bounds.size;
        pcbSize = new Vector3(pcbSize.x, pcbSize.y, -pcbSize.z);
        
    }

    public void SetImageVisible(bool tf)
    {
        image.SetActive(tf);
    }

    public void SetImageRect(string par)
    {
        if(!image.activeSelf) image.SetActive(true);

        string[] pars = par.Split(':');
        RectTransform rect = image.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(float.Parse(pars[0]), float.Parse(pars[1]));
        rect.anchorMax = new Vector2(float.Parse(pars[2]), float.Parse(pars[3]));
    }

    public void MoveCamera(int dir)
    {
        if (bondTransFB == null || bondTransLR == null) return;

        int time = 1;
        if (dir > 3) time = 4;

        if (dir == 0 || dir == 4)//front
        {
            if (bondTransFB.localPosition.z < front)
            {
				Move(transform, 0, 0, moveDelta*time);
				Move(bondTransFB, 0, 0, moveDelta * time);
            }
        }
        else if(dir == 1 || dir == 5)//back
        {
            if (bondTransFB.localPosition.z > back)
            {
				Move(transform, 0, 0, -moveDelta * time);
				Move(bondTransFB, 0, 0, -moveDelta * time);
            }
        }
        else if(dir == 2 || dir == 6)//left
        {
            if (bondTransLR.localPosition.x < left)
            {
                Move(transform, moveDelta * time, 0, 0);
                Move(bondTransLR, moveDelta * time, 0, 0);
            }
        }
        else//right
        {
            if (bondTransLR.localPosition.x > right)
            {
                Move(transform, -moveDelta * time, 0, 0);
                Move(bondTransLR, -moveDelta * time, 0, 0);
            }
        }
    }

    void Move(Transform tran, float offx, float offy, float offz)
    {
		tran.position = new Vector3(tran.position.x + offx, tran.position.y + offy, tran.position.z +offz);
    }

    public void SelectStep(float xratio, float yratio, float zratio, float rotation)
    {
        pcbZeroPos = pcb.transform.position - pcbSize / 2;

        Vector3 selectPos = new Vector3();
        selectPos.x = pcbZeroPos.x + pcbSize.x * xratio;
        selectPos.y = pcbZeroPos.y + pcbSize.y * zratio;
        selectPos.z = pcbZeroPos.z + pcbSize.z * yratio;

        GameObject selCube = GameObject.Find("SelectCube");
        selCube.transform.position = selectPos;
        Vector3 eulerAngles = selCube.transform.eulerAngles;
        selCube.transform.eulerAngles = new Vector3(eulerAngles.x, rotation, eulerAngles.z);

        Vector3 offset = selCube.transform.position - transform.position;
        Move(transform, offset.x, 0, offset.z);
        Move(bondTransLR, offset.x, 0, 0);
        Move(bondTransFB, 0, 0, offset.z);
    }

    public string GetCameraPos()
    {
        if (pcb != null)
        {
            pcbZeroPos = pcb.transform.position - pcbSize / 2;
            float xratio = (transform.position.x - pcbZeroPos.x) / pcbSize.x;
            float yratio = (transform.position.y - pcbZeroPos.y) / pcbSize.y;
            float zratio = (transform.position.z - pcbZeroPos.z) / pcbSize.z;

            return xratio + ":" + yratio + ":" + zratio;
        }

        return transform.position.x + ":" + transform.position.y + ":" + transform.position.z;
    }
}
