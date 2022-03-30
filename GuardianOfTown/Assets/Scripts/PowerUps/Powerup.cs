using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PoweupEffect powerupEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Destroy(gameObject);
            powerupEffect.Apply(other.gameObject);
        }
        
    }
}
