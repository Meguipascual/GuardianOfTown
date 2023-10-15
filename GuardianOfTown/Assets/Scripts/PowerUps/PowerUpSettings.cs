using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSettings : MonoBehaviour
{
    public static PowerUpSettings Instance;
    public bool IsContinuousShootInUse {  get; set; }
    public bool IsSpeedIncreased {  get; set; }
    public float SpeedAmount {  get; set; }
    public float PreviousPlayerSpeed {  get; set; }

    private void Awake()
    {
        Instance = this;
    }
}
