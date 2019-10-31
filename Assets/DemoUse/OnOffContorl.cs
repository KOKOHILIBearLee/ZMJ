using UnityEngine;
using System.Collections;

public class OnOffContorl : MonoBehaviour
{
    Animator ani;


    public bool IsUp
    {
        get
        {
            if (ani == null)
                ani = GetComponent<Animator>();
            if (ani != null)
                return ani.GetBool("Up");
            else
                return false;
        }

        set
        {
            if (ani == null)
                ani = GetComponent<Animator>();
            if (ani != null)
                ani.SetBool("Up", value);
        }
    }



    public bool test = false;
    private void Update()
    {
        if (test)
        {
            test = false;

            IsUp = !IsUp;
        }
    }

}
