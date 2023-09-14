using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuTitleManager : MonoBehaviour
{
    public void StartGameButton()
    {
        //SceneManager.LoadScene(Random.Range(1, 3));
        SceneManager.LoadScene(1);
    }

    public void Exitbutton()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }
}
