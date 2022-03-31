using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PoweupEffect
{
    [SerializeField] private float amount;
    private MonoBehaviour m_MonoBehaviour;

    public override void Apply(GameObject target)
    {
        m_MonoBehaviour = FindObjectOfType<MonoBehaviour>();
        var player = target.GetComponent<PlayerController>();
        if (player.Speed > 16f)
        {
            Debug.Log("Don't speed up");
            return;
        }
        m_MonoBehaviour.StartCoroutine(ActivateEffect(player));
    }

    IEnumerator ActivateEffect(PlayerController player)
    {
        var speed = player.Speed;
        player.Speed += amount;
        Debug.Log("Speed Augmented");
        yield return new WaitForSeconds(3f);
        player.Speed = speed;
        Debug.Log("Speed Reverted");
    } 
}
