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
        slider.value = fillValue;

        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value <= slider.maxValue * 0.2f)
        {
            fillImage.color = Color.red;
        }
        else if (slider.value <= slider.maxValue * 0.5f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Tags.BlueLight;
            //fillImage.color = Color.green;
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
