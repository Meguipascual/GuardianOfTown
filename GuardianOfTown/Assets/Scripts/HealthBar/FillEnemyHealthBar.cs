using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Image fillImage;
    public Slider slider;

    public void FillEnemySliderValue()
    {
        fillImage.enabled = true;
        float fillValue = (float)_enemy.HP / _enemy.HpMax;
        Debug.Log(fillImage.isActiveAndEnabled);
        slider.value = fillValue;

        Debug.Log($"Slider Actual value: {slider.value}");

        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value <= slider.maxValue * 0.2f)
        {
            //fillImage.color = Tags.RedLight;
            fillImage.color = Color.red;
            Debug.Log("Rojo");
        }
        else if (slider.value <= slider.maxValue * 0.5f)
        {
            //fillImage.color = Tags.Yellow;
            fillImage.color = Color.yellow;
            Debug.Log("Amarillo");
        }
        else
        {
            //fillImage.color = Tags.BlueLight;
            fillImage.color = Color.green;
            Debug.Log("Verde");
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
