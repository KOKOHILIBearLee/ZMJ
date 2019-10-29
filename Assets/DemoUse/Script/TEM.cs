using UnityEngine;
using System.Collections;

public class TEM : MonoBehaviour {
    Animation animation;

    // Animator animator;
    // Use this for initialization
    void Start()
	{
        animation = GetComponent<Animation>();
        if (animation == null)
            return;
        AnimationEvent aniamtionEvent = new AnimationEvent();
        aniamtionEvent.functionName = "Attach";
        aniamtionEvent.time = 140;
        animation.clip.AddEvent(aniamtionEvent);
        //	animation.Play ();

        TEM[] t = this.GetComponents<TEM>();
        foreach (TEM tt in t)
            if (tt != this)
                Destroy(tt);
        //TEM t = this.GetComponent<TEM>();
        //if (t != null)
        //    Destroy(t);
    }

    public void Attach()
    {
        FlowMachine.bFlow = false;
        GameObject game = GameObject.FindGameObjectWithTag("VRC");
        GameObject gamedat = GameObject.Find("FHDT_TZ");
        if (game != null && gamedat != null)
            game.transform.position = gamedat.transform.position;
        Destroy(this);
    }
}
