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
            var player = other.GetComponent<PlayerController>();
            player.PlayPowerUpSound();
            player.ShowYeiiImageInSeconds(.5f);
            powerupEffect.Apply(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.CompareTag(Tags.Wall))
        {
            Destroy(gameObject);
        }
    }
}
