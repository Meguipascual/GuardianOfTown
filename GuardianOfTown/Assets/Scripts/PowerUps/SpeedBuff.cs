using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/SpeedBuff")]
public class SpeedBuff : PoweupEffect
{
    [SerializeField] private float amount;
    private MonoBehaviour m_MonoBehaviour;
    private PlayerController _player;

    public override void Apply(GameObject target)
    {
        m_MonoBehaviour = FindObjectOfType<PlayerController>();
        _player = target.GetComponent<PlayerController>();
        m_MonoBehaviour.StartCoroutine(ActivateEffect());
    }

    IEnumerator ActivateEffect()
    {
        PowerUpSettings.Instance.IsSpeedIncreased = true;
        PowerUpSettings.Instance.SpeedAmount = amount;
        PowerUpSettings.Instance.PreviousPlayerSpeed = _player.Speed;
        _player.Speed += amount;
        Debug.Log("Speed Augmented");
        GameManager.SharedInstance._menuPlayerSpeedText.text = $"Speed: {_player.Speed}";
        yield return new WaitForSeconds(3f);
        DeactivateEffect();
    } 

    public void DeactivateEffect()
    {
        _player.Speed -= amount;
        PowerUpSettings.Instance.IsSpeedIncreased = false;
        PowerUpSettings.Instance.SpeedAmount = 0;
        GameManager.SharedInstance._menuPlayerSpeedText.text = $"Speed: {_player.Speed}";
        Debug.Log("Speed Reverted");
    }
}
