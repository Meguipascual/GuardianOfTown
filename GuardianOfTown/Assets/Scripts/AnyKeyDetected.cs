using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyDetected : MonoBehaviour
{
    private MenuTitleManager _menuTitleManager;
    private void Start()
    {
        _menuTitleManager = FindObjectOfType<MenuTitleManager>();
    }
    // Update is called once per frame
    void Update()
    {
       if (Input.anyKey)
        {
            _menuTitleManager.SkipPrologueButton();
        } 
    }
}
