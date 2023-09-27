using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpsSettings : MonoBehaviour
{
    public bool IsOverHeatingUnactive;
    public bool IsFrontSwordActive;
    public bool IsBackShootActive;
    public bool IsInfiniteContinuousShootActive;
    public bool IsDoubleShootActive;
    public bool IsTripleShootActive;
    public bool []AreMoreBulletsActive;
    public bool []AreShootSpeedActive;
    // Start is called before the first frame update
    void Start()
    {
        AreMoreBulletsActive = new bool[3];
        AreShootSpeedActive = new bool[5];
    }
}
