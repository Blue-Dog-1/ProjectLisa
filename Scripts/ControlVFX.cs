using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ControlVFX : MonoBehaviour
{
    public VisualEffect Dust;
    public VisualEffect Stones;
    public void dust(float i)
    {
        Dust.SetFloat("Rate", i);
        Stones.SetFloat("Rate", i);
    }
    
}

