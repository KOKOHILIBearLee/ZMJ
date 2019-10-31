using UnityEngine;
using System.Collections;

public class HandControler : MonoBehaviour
{
    public static HandControler singleton;
    Animator animator;
    public bool hold = false;

    private void Awake()
    {
        singleton = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
       
        //animator.s
    }

    void Update()
    {
        animator.SetBool("Hold", hold);
    }
}
