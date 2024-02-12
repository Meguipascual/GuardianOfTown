using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TextMeshProUGUI _loadingValue;
    [SerializeField] private TextMeshProUGUI _tipBody;
    [SerializeField] private string[] _tips;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextSceneAsync()
    {
        _tipBody.text = _tips[Random.Range(0,_tips.Length)];
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Tags.WorldTouch);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (int)(progress * 100) + "%");
            _loadingValue.text = (int)(progress * 100) + "%";
            _loadingSlider.value = progress;

            yield return null;
        }
    }
}
