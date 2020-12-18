using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] RectTransform SwitchShadows;
    [SerializeField] RectTransform SwitchAtrDisk;
    [SerializeField] Slider sliderPhysic;
    [SerializeField] GameObject AtractionDisk;

    bool shadowOn = false;
    
    Vector2 anchorMinOff = new Vector2(0.33f, 0f);
    Vector2 anchorMinOn = new Vector2(0.67f, 0f);

    Vector2 anchorMaxOff = new Vector2(0.33f, 1f);
    Vector2 anchorMaxOn = new Vector2(0.67f, 1f);

    float[] steps = new float[5] { 0.075f, 0.07f, 0.067f, 0.064f, 0.06f };

    private void Start()
    {
        var shadows =  PlayerPrefs.GetInt("Shadows", 0);
        var atrDisk = PlayerPrefs.GetInt("AtrDisk", 1);
        var fixedDeltaTimeIndex = PlayerPrefs.GetInt("FixedDeltaTime", 2);


        shadowOn = (shadows == 1);
        if (shadowOn) Shadows();

        if ((atrDisk == 1) != AtractionDisk.active)
            AtrDisk();

        sliderPhysic.value = 0.2f * (fixedDeltaTimeIndex + 1);
        Time.fixedDeltaTime = steps[fixedDeltaTimeIndex];
    }
    public void Shadows()
    {
        if (shadowOn)
        {
            QualitySettings.SetQualityLevel(0, true);
            SwitchShadows.anchorMax = anchorMaxOff;
            SwitchShadows.anchorMin = anchorMinOff;
            shadowOn = false;

            PlayerPrefs.SetInt("Shadows", 0);
        }
        else
        {
            QualitySettings.SetQualityLevel(2, true);
            SwitchShadows.anchorMax = anchorMaxOn;
            SwitchShadows.anchorMin = anchorMinOn;
            shadowOn = true;

            PlayerPrefs.SetInt("Shadows", 2);
        }
    }
    public void AtrDisk()
    {
        AtractionDisk.SetActive(!AtractionDisk.active);
        if (AtractionDisk.active)
        {
            SwitchAtrDisk.anchorMax = anchorMaxOn;
            SwitchAtrDisk.anchorMin = anchorMinOn;
            PlayerPrefs.SetInt("AtrDisk", 1);
        }
        else
        {
            SwitchAtrDisk.anchorMax = anchorMaxOff;
            SwitchAtrDisk.anchorMin = anchorMinOff;
            PlayerPrefs.SetInt("AtrDisk", 0);
        }
    }
    public void OnPointerUp()
    {
        float _step = 0f;
        float value = sliderPhysic.value;
        int index = 0;
        for (int i = 0; i < steps.Length - 1; i++)
        {
            _step += 0.2f;
            if (_step < (value + 0.21f) )
            {
                if (sliderPhysic.value > 0.95f)
                {
                    sliderPhysic.value = 1f;
                    index = steps.Length - 1;
                }
                else
                {
                    sliderPhysic.value = _step;
                    index = i;
                }
            }
        }
        Time.fixedDeltaTime = steps[index];
        PlayerPrefs.SetInt("FixedDeltaTime", index);
    }

    
}
