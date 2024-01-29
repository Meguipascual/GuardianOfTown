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
    void Start()
    {
        slider.maxValue = playerController.HP / playerController.HpMax;
        FillSliderValue();
    }
    
    public void FillSliderValue()
    {
        float fillValue = (float)playerController.HP / (float)playerController.HpMax;
        Debug.Log($"Slider Value: {fillValue}, Slider max: {slider.maxValue}");
        slider.value = fillValue;
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value <= slider.maxValue * 0.2f)
        {
            fillImage.color = new Color(0.94f, 0.24f, 0f);
        }
        else if (slider.value <= slider.maxValue * 0.5f)
        {
            fillImage.color = new Color(0.89f, 0.75f, 0.13f);
        }
        else
        {
            //Azul claro
            fillImage.color = new Color(0, 0.59f, 0.55f);
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
