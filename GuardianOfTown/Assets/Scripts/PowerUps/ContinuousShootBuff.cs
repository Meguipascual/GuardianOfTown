using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/ContinuousShootBuff")]
public class ContinuousShootBuff : PoweupEffect
{
    [SerializeField] private float amount;
    private MonoBehaviour m_MonoBehaviour;
    public override void Apply(GameObject target)
    {
        if (!GameSettings.Instance.IsEasyModeActive)
        {
            m_MonoBehaviour = FindObjectOfType<PlayerController>();
            var player = target.GetComponent<PlayerController>();
            GameSettings.Instance.IsEasyModeActive = true;
            m_MonoBehaviour.StartCoroutine(ActivateEffect(player));
        }
        else
        {
            Debug.Log("Already in use");
        }
    }

    IEnumerator ActivateEffect(PlayerController player)
    {
        Debug.Log($"deactivate easy mode in {amount} seconds");
        yield return new WaitForSeconds(amount);
        Debug.Log($"deactivate easy mode");
        GameSettings.Instance.IsEasyModeActive = false;
    }
}
