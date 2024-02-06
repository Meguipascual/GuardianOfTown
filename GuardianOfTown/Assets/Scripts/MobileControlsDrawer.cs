using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControlsDrawer : MonoBehaviour
{

    [SerializeField] private Image _touchMoveImage;
    [SerializeField] private Image _touchShootImage;
    [SerializeField] private Canvas _touchCanvas;

    // Start is called before the first frame update
    void Start()
    {
        _touchMoveImage.rectTransform.sizeDelta = new Vector2((Screen.width / 3)*2, (Screen.height/3)*2);
        Debug.Log($"Screen: {Screen.width} x {Screen.height}");
        Debug.Log($"size: {_touchMoveImage.rectTransform.sizeDelta}");
        _touchMoveImage.gameObject.transform.position = new Vector3(0 + _touchMoveImage.preferredWidth / 2, 0 + _touchMoveImage.preferredHeight / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
