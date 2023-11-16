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
            other.GetComponent<PlayerController>().PlayPowerUpSound();
            powerupEffect.Apply(other.gameObject);
        }else if(other.CompareTag(Tags.Wall))
        {
            Destroy(gameObject);
        }
    }

    private void PlayPowerUpSound()
    {

    }
}
