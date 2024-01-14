using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuTitleManager : MonoBehaviour
{
    [SerializeField] private bool _isPrologueSkipped;
    public Toggle skipToggle;

    private void Start()
    {
        if (!(SceneManager.GetActiveScene().buildIndex == 0))
        {
            return;
        }
        
        if (skipToggle == null) { return; }

        if (PlayerPrefs.GetString("IsSkipped") == "False")
        {
            _isPrologueSkipped = false;
            skipToggle.isOn = false;
        }
        else
        {
            _isPrologueSkipped = true;
            skipToggle.isOn = true;
        }
    }

    public void SkipPrologueButton()
    {
        SceneManager.LoadScene(1);
    }

    public void StartGameButton()
    {
        Debug.Log($"Device Type: {SystemInfo.deviceType}");
        if (_isPrologueSkipped)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void Exitbutton()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void SkipPrologueToggle()
    {
        _isPrologueSkipped = skipToggle.isOn;
        PlayerPrefs.SetString("IsSkipped", _isPrologueSkipped.ToString());
    }
}
