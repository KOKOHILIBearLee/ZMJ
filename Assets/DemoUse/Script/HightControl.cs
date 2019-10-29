using UnityEngine;
using System.Collections;

public class HightControl : MonoBehaviour
{
    PluginEvent.HighlightableObject obj;
    // Use this for initialization
    void Start()
    {
        obj = GetComponent<PluginEvent.HighlightableObject>();
        obj.ConstantOn(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        obj.ConstantOn(Color.red);
    }
}
