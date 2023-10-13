using UnityEngine;
using UnityEngine.UI;

public class TripleShootPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 0;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();
        if (_permanentPowerUpsSettings.IsTripleShootActive || !_permanentPowerUpsSettings.IsDoubleShootActive)
        {
            _thisButton.interactable = false;
        }
        else
        {
            _thisButton.interactable = true;
        }
    }
    public void Apply()
    {
        _permanentPowerUpsSettings.ActivateTripleShoot();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
