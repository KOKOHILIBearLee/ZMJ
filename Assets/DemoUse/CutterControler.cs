using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CutterControler : MonoBehaviour
{
    public List<GameObject> regions = new List<GameObject>();
    List<Vector3> originalposition = new List<Vector3>();
    public RectTransform rightindication;
    public RectTransform leftindication;
    private void Awake()
    {
        rightindication = transform.Find("RightUpIndication").GetComponent<RectTransform>();
        leftindication = transform.Find("LeftUpIndication").GetComponent<RectTransform>();

        //Transform rightup = transform.Find("RightUp");
        //regions.Add(rightup.gameObject);
        //originalposition.Add(rightup.localPosition);


        //Transform rightdown = transform.Find("RightDown");
        //regions.Add(rightdown.gameObject);
        //originalposition.Add(rightdown.localPosition);


        //Transform leftdown = transform.Find("LeftDown");
        //regions.Add(leftdown.gameObject);
        //originalposition.Add(leftdown.localPosition);

        //Transform leftup = transform.Find("LeftUp");
        //regions.Add(leftup.gameObject);
        //originalposition.Add(leftup.localPosition);


        //Transform rightup1 = transform.Find("RightUp1");
        //regions.Add(rightup1.gameObject);
        //originalposition.Add(rightup1.localPosition);


        //Transform rightdown1 = transform.Find("RightDown1");
        //regions.Add(rightdown1.gameObject);
        //originalposition.Add(rightdown1.localPosition);


        //Transform leftdown1 = transform.Find("LeftDown1");
        //regions.Add(leftdown1.gameObject);
        //originalposition.Add(leftdown1.localPosition);

        //Transform leftup1 = transform.Find("LeftUp1");
        //regions.Add(leftup1.gameObject);
        //originalposition.Add(leftup1.localPosition);
    }

    public void AllRecovery()
    {
        GetRight();
        GetRight1();
        GetLeft();
        GetLeft1();
    }

    bool abandright = false;
    public void AbandonRight()
    {
        regions[0].transform.parent = transform.parent.parent;
        regions[1].transform.parent = transform.parent.parent;
        abandright = true;
    }

    bool abandright1 = false;
    public void AbandonRight1()
    {
        regions[4].transform.parent = transform.parent.parent;
        regions[5].transform.parent = transform.parent.parent;
        //if (abandleft)
        //{
        //    Debug.LogError("抛弃");
        //    regions[4].transform.localPosition = regions[3].transform.localPosition;
        //    regions[5].transform.localPosition = regions[2].transform.localPosition;
        //}
        abandright1 = true;
    }

    public void GetRight()
    {
        regions[0].transform.parent = transform;
        regions[0].transform.localPosition = originalposition[0];
        regions[1].transform.parent = transform;
        regions[1].transform.localPosition = originalposition[1];
        abandright = false;
    }

    public void GetRight1()
    {
        regions[4].transform.parent = transform;
        regions[4].transform.localPosition = originalposition[4];
        regions[5].transform.parent = transform;
        regions[5].transform.localPosition = originalposition[5];
        abandright1 = false;
    }

    bool abandleft = false;
    public void AbandonLeft()
    {
        regions[2].transform.parent = transform.parent.parent;
        regions[3].transform.parent = transform.parent.parent;
        if (!abandright1)
        {
            Debug.LogError("抛弃");
            regions[2].transform.position = regions[5].transform.position;
            regions[3].transform.position = regions[4].transform.position;
        }
        abandleft = true;
    }

    bool abandleft1 = false;
    public void AbandonLeft1()
    {
        regions[6].transform.parent = transform.parent.parent;
        regions[7].transform.parent = transform.parent.parent;
        abandleft1 = true;
    }

    public void GetLeft()
    {
        regions[2].transform.parent = transform;
        regions[2].transform.localPosition = originalposition[2];
        regions[3].transform.parent = transform;
        regions[3].transform.localPosition = originalposition[3];
        abandleft = false;
    }

    public void GetLeft1()
    {
        regions[6].transform.parent = transform;
        regions[6].transform.localPosition = originalposition[6];
        regions[7].transform.parent = transform;
        regions[7].transform.localPosition = originalposition[7];
        abandleft1 = false;
    }

    List<Vector3> preposition;// = new List<Vector3>();
    public List<List<Vector2>> Normal(Vector3 point, Vector3 direction)
    {
        List<Vector2> res1 = new List<Vector2>();
        List<Vector2> res2 = new List<Vector2>();
        List<Vector3> pre = new List<Vector3>();
        //preposition = new List<Vector3>();
        for (int i = 0; i < regions.Count; i++)
        {
            pre.Add(regions[i].transform.position);
            Vector3 result = -point + regions[i].transform.position;
            Vector2 resss = new Vector2(result.x / direction.x, result.y / direction.y);
            if (i <= 3)
                res1.Add(resss);
            else
                res2.Add(resss);
        }

        preposition = new List<Vector3>();

        pre.ForEach(pf =>
        {
            preposition.Add(pf);
        });

        return new List<List<Vector2>>(new List<Vector2>[] { res1, res2 });
    }

    public static void HideGame(GameObject game)
    {
        Image image = game.GetComponent<Image>();
        if (image != null)
        {
            image.enabled = false;
            Collider2D colider = game.GetComponent<Collider2D>();
            if (colider != null)
                colider.enabled = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (enabled)
    //    {
    //        HideGame(other.gameObject);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (enabled)
    //    {
    //        HideGame(other.gameObject);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (enabled)
    //    {
    //        HideGame(other.gameObject);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    GameObject game = collision.gameObject;
    //    Image image = game.GetComponent<Image>();
    //    if (image != null)
    //    {
    //        image.enabled = false;
    //        Collider2D colider = game.GetComponent<Collider2D>();
    //        if (colider != null)
    //            colider.enabled = false;
    //    }
    //}
}
