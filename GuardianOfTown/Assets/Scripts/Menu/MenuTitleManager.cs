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
    [SerializeField] private GameObject _loadingGameObject;
    [SerializeField] private GameObject _mainMenuGameObject;
    [SerializeField] private SceneLoading _sceneLoading;
    public Toggle skipToggle;

    private void Start()
    {
        if (!(SceneManager.GetActiveScene().name == Tags.Menu))
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

    public void StartButtonClick()
    {
        if (_isPrologueSkipped)
        {
            _mainMenuGameObject.gameObject.SetActive(false);
            _loadingGameObject.gameObject.SetActive(true);
            _sceneLoading.LoadNextSceneAsync();
            return;
        }
        SceneManager.LoadScene(Tags.Prologue);
    }

    public void SkipPrologueButton()
    {
        //Silence Prologue Sounds
        SceneManager.LoadScene(Tags.WorldTouch);
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
