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

        if(slider.value <= slider.maxValue * 0.2f)
        {
            fillImage.color = Color.red;
        }else if (slider.value <= slider.maxValue * 0.5f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
