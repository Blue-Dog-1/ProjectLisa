using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlicedAdapter : MonoBehaviour
{
    [SerializeField] float ScreenHeight;

    void Start()
    {
        Image image = GetComponent<Image>();
        float ratioHeight = Screen.height / ScreenHeight;
        image.pixelsPerUnitMultiplier /= ratioHeight;
    }

    
}
