using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingTouchButton : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed;
    private ShootingManager _shootingManager;

    private void Start()
    {
        _shootingManager = FindObjectOfType<ShootingManager>();
    }

    public void OnUpdateSelected(BaseEventData data)
    {
        if (isPressed)
        {
            _shootingManager.ShootEasyMode();
        }
        else
        {
            OverHeatedManager.Instance.CoolCannon();
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;

    }
}