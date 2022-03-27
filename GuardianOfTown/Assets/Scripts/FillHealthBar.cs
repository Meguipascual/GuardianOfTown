using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour
{
    public PlayerController playerController;
    public Image fillImage;
    public Slider slider;

    // Start is called before the first frame update
    void awake()
    {
        slider.maxValue = playerController.HpMax;
        FillSliderValue();
    }
    
    public void FillSliderValue()
    {
        float fillValue = (float)playerController.HP / (float)playerController.HpMax;
        slider.value = fillValue;
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
