using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DemoUse : MonoBehaviour
{

    public static DemoUse singleton = null;
    public GameObject changJing;

    // Use this for initialization
    void Start()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject trackGO = null;
    float trackBack = 0;
    float trackFront = 0;
    public void BondTrack(string trackName, float front, float back)
    {
        trackGO = SOManager.singleton.FindObject(trackName);
        trackFront = front;
        trackBack = back;
    }

    public void SetTrackWidth(string type)
    {
        //设置轨道宽度
        if (trackGO == null) return;

        float offset = 0;
        if (type == "快速调宽")
        {
            offset = -0.005f;
        }
        else if (type == "慢速调宽")
        {
            offset = -0.0005f;
        }
        else if (type == "慢速调窄")
        {
            offset = 0.0005f;
        }
        else if (type == "快速调窄")
        {
            offset = 0.005f;
        }

        Vector3 pos = trackGO.transform.localPosition;
        pos.z = pos.z+offset;
        if (pos.z < trackBack) pos.z = trackBack;
        if (pos.z > trackFront) pos.z = trackFront;
        trackGO.transform.localPosition = pos;
    }

    Grayscale cameraGray = null;
    public void SetCameraLight(float ratio)
    {
        if (cameraGray == null)
        {
            cameraGray = transform.Find("TextureCamera").GetComponent<Grayscale>();
        }

        cameraGray.rampOffset = -0.2f + 0.7f * ratio;
    }
}
