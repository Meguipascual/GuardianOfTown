using UnityEngine;
using UnityEngine.UI;

public class MoreBulletsPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 7;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();


        for (int i = 0; i < _permanentPowerUpsSettings.AreMoreBulletsWasted.Length; i++)
        {
            if (!_permanentPowerUpsSettings.AreMoreBulletsWasted[i])
            {
                _thisButton.interactable = true;
                return;
            }
        }

        _thisButton.interactable = false;
    }
    public void Apply()
    {
        _permanentPowerUpsSettings.ActivateMoreBullets();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
