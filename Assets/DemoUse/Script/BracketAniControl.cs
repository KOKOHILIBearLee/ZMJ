using UnityEngine;
using System.Collections;

public class BracketAniControl : MonoBehaviour
{
    Animation ani;
    public float target;
    bool order;
    public bool play = false;
    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animation>();
        if (ani == null)
            Destroy(this);
        order = (target - ani[ani.clip.name].normalizedTime) >= 0;
        if (play)
        {
            ani[ani.clip.name].normalizedSpeed = 0.1f;
            //ani.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((order ? (ani[ani.clip.name].normalizedTime - target) : (target - ani[ani.clip.name].normalizedTime)) >= 0)
        {
            ani[ani.clip.name].normalizedTime = target;
            ani[ani.clip.name].normalizedSpeed = 0;
            Destroy(this);
        }
    }

    public static BracketAniControl PlayBracketAniControl(GameObject game, float target = 1, bool play = false)
    {
        if (game == null || game.GetComponent<Animation>() == null)
            return null;

        BracketAniControl control = game.AddComponent<BracketAniControl>();
        control.target = target;
        control.play = play;
        return control;
    }
}
