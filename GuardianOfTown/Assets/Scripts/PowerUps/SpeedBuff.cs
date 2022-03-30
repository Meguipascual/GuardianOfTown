using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PoweupEffect
{
    [SerializeField] private float amount;

    public override void Apply(GameObject target)
    {
        var player = target.GetComponent<PlayerController>();
        if (player.Speed > 16f)
        {
            Debug.Log("Don't speed up");
            return;
        }
        FindObjectOfType<MonoBehaviour>().StartCoroutine(ActivateEffect(player));
    }
    
    IEnumerator ActivateEffect(PlayerController player)
    {
        var speed = player.Speed;
        player.Speed += amount;
        Debug.Log("Speed Augmented");
        yield return new WaitForSeconds(3f);
        player.Speed = speed;
    } 
}
