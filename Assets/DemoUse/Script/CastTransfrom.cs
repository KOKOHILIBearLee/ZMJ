using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastTransfrom : MonoBehaviour
{
    public GameObject Target;
    public static CastTransfrom singleton;
    // Use this for initialization
    public CutterGTAnimation cutterr;
    public CutterGTAnimation cutterl;
    public float distance = 0;
    Transform rightarr;
    Transform leftarr;
    Transform movedirarr;

    Transform rightarr2;
    Transform leftarr2;

    public Transform direction;


    Animation leftani;
    Animation rightani;
    Image img;

    public Sprite cutdown;
    public Sprite cuttri;
    public Sprite cutknife;
    public Sprite followmiddle;

    public void CutterGTToRight()
    {
        cutterr.direction = true;
        cutterl.direction = false;
    }

    public void CutterGTToLeft()
    {
        cutterr.direction = false;
        cutterl.direction = true;
    }

    private void Awake()
    {
        singleton = this;
        rightarr = transform.Find("Body/RA_F/RightArm/Arrow");
        leftarr = transform.Find("Body/LA_F/LeftArm/Arrow");
        rightarr2 = transform.Find("Body/RA_F/RightArm/Arrow02");
        leftarr2 = transform.Find("Body/LA_F/LeftArm/Arrow02");
        leftani = transform.Find("Body/LA_F").GetComponent<Animation>();
        rightani = transform.Find("Body/RA_F").GetComponent<Animation>();
        movedirarr = transform.Find("Body/Arrow");
        img = transform.Find("Hint").GetComponent<Image>();
        HintVisible(false);
        //InitilizeAnimation();
    }

    public void LeftArmUp()
    {
        leftani.Stop();
        leftani[leftani.clip.name].normalizedTime = 0;
        leftani[leftani.clip.name].speed = 1;
        leftani.Play();
    }

    public void LeftArmDown()
    {
        leftani.Stop();
        leftani[leftani.clip.name].normalizedTime = 1;
        leftani[leftani.clip.name].speed = -1;
        leftani.Play();
    }

    public void RightArmUp()
    {
        rightani.Stop();
        rightani[rightani.clip.name].normalizedTime = 1;
        rightani[rightani.clip.name].speed = -1;
        rightani.Play();
    }

    public void RightArmDown()
    {
        rightani.Stop();
        rightani[rightani.clip.name].normalizedTime = 0;
        rightani[rightani.clip.name].speed = 1;
        rightani.Play();
    }

    public void InitilizeAnimation()
    {
        Debug.LogError("初始化CastTransfrom动画");

        // LeftArmUp();
        //RightArmDown();

        leftani[leftani.clip.name].normalizedTime = 1;
        leftani[leftani.clip.name].speed = 0;
        leftani.Play();

        rightani[rightani.clip.name].normalizedTime = 1;
        rightani[rightani.clip.name].speed = 0;
        rightani.Play();
    }

    public void ToRight()
    {
        rightarr.gameObject.SetActive(true);
        leftarr.gameObject.SetActive(true);

        rightarr2.gameObject.SetActive(false);
        leftarr2.gameObject.SetActive(false);
        MiddleVisible(true);
        CutterGTToRight();
        movedirarr.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ToLeft()
    {
        rightarr.gameObject.SetActive(false);
        leftarr.gameObject.SetActive(false);

        rightarr2.gameObject.SetActive(true);
        leftarr2.gameObject.SetActive(true);
        MiddleVisible(true);
        CutterGTToLeft();
        movedirarr.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void MiddleVisible(bool val)
    {
        movedirarr.gameObject.SetActive(val);
    }

    public void CutDown()
    {
        HintVisible(true);
        img.sprite = cutdown;
    }

    public void CutTri()
    {
        HintVisible(true);
        img.sprite = cuttri;
    }

    public void CutKnife()
    {
        HintVisible(true);
        img.sprite = cutknife;
    }

    public void FollowMiddle()
    {
        HintVisible(true);
        img.sprite = followmiddle;
    }

    public void HintVisible(bool val)
    {
        if (Cutter2DAnimation.singleton == null || Cutter2DAnimation.singleton.GetVisible())
            img.enabled = val;
    }

    // Update is called once per frame
    void Update()
    {
        Plane plane = new Plane(direction.up, direction.position + direction.up * distance);

        Ray ray = new Ray(Target.transform.position, direction.up);
        float enter;
        plane.Raycast(ray, out enter);



        this.transform.localEulerAngles = Vector3.zero;

        this.transform.position = ray.GetPoint(enter);//new Vector3(localpoint.x, distance, localpoint.z);
    }
}
