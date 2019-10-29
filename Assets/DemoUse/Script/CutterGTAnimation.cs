using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutterGTAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float interval;
    public bool direction;
    Image imgs;
    // Use this for initialization
    void Start()
    {
        imgs = GetComponent<Image>();
    }

    int i = 0;
    float last;
    // Update is called once per frame
    void Update()
    {
        imgs.sprite = sprites[i];
        last += Time.deltaTime;

        if (last > interval)
        {
            last = 0;
            if (direction)
                i = (i + 1) % sprites.Length;
            else
                i = (i - 1 + sprites.Length) % sprites.Length;
        }
    }
}
