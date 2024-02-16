using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdWarningSceneManager : MonoBehaviour
{
    [SerializeField] private int _delay;
    private Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        if(_delay == 0)
        {
            _delay = 3;
        }
        _coroutine = StartCoroutine(ChangeSceneInSeconds(_delay));
    }

    public void SkipWarning()
    {
        StopCoroutine(_coroutine);
        SceneManager.LoadScene(Tags.Menu);
    }

    IEnumerator ChangeSceneInSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(Tags.Menu);
    }
}
