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
    /*void Start()
    {
        slider.maxValue = 1;
        FillSliderValue();
    }*/
    
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
            fillImage.color = Tags.RedLight;
        }
        else if (slider.value <= slider.maxValue * 0.5f)
        {
            fillImage.color = Tags.Yellow;
        }
        else
        {
            //Azul claro
            fillImage.color = Tags.BlueLight;
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
