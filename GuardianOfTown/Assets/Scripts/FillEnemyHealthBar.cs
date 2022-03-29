using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillEnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Image fillImage;
    public Slider slider;

    // Start is called before the first frame update
    void awake()
    {
        slider.maxValue = enemy.HpMax;
    }
    
    public void FillEnemySliderValue()
    {
        
        
        fillImage.enabled = true;
        float fillValue = (float)enemy.HP / (float)enemy.HpMax;
        slider.value = fillValue;
        Debug.Log($"enemy hp: {enemy.HP} hpMax: {enemy.HpMax}");

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
        }else
        {
            fillImage.color = Color.green;
        }
    }

    public void ModifySliderMaxValue(int value) 
    {
        slider.maxValue = value; 
    }
}
