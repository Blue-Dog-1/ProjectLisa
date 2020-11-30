using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StarAnim : MonoBehaviour
{
    [SerializeField]
    Image nextStar;
    public bool isActov = false;
    public Animation animation;

    Image im;
    Animation anim;
    private void Awake()
    {
        isActov = false;
    }
    public void StartAnimation()
    {
        if (isActov)
        {
            nextStar.enabled = true;
            animation.Play();
        }
        
    }
    public void StartAnimation(bool Activ)
    {
        im = GetComponent<Image>();
        im.enabled = true;
        anim = GetComponent<Animation>();
        anim.Play();
    }


}
