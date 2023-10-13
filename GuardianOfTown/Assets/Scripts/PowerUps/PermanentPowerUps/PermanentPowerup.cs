using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentPowerup : MonoBehaviour
{
    protected LevelUpSliderManager _sliderManager;
    protected PermanentPowerUpsSettings _permanentPowerUpsSettings;
    protected PermanentPowerUpManager _permanentPowerUpManager;
    protected Button _thisButton;
    protected int _id;
    public int Id => _id;
    public int Index { get; set; }

    protected void Start()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        _permanentPowerUpsSettings = FindObjectOfType<PermanentPowerUpsSettings>();
        _permanentPowerUpManager = FindObjectOfType<PermanentPowerUpManager>();
    }
}
