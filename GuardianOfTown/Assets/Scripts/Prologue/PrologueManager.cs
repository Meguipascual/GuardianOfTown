using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{
    [SerializeField] private string _oldManSpeech;
    [SerializeField] private string _boysSpeech;
    [SerializeField] private TextMeshProUGUI _oldManText;
    [SerializeField] private TextMeshProUGUI _boysText;
    [SerializeField] private Transform _imageTransform;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject[] _speechBubles;
    [SerializeField] private GameObject _speechBublesParentGameObject;
    [SerializeField] private Button _skipButton;
    [SerializeField] private SceneLoading _sceneLoading;
    [SerializeField] private float _rotateIndex;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private AudioSource _lettersAudioSource;
    [SerializeField] private AudioSource _speechBubblesAudioSource;
    [SerializeField] private AudioSource _firstSongSource;
    [SerializeField] private AudioSource _secondSongSource;



    // Start is called before the first frame update
    void Start()
    {
        _sceneLoading = FindObjectOfType<SceneLoading>();
        StartCoroutine("StartOldManSpeech");
    }

    IEnumerator StartOldManSpeech()
    {
        _boysText.gameObject.SetActive(false);
        _oldManText.gameObject.SetActive(true);
        foreach (var item in _oldManSpeech)
        {
            _oldManText.text += item;
            _lettersAudioSource.Play();
            yield return new WaitForSeconds(.1f);
        }
        StartCoroutine("StartBoysSpeech");

        _oldManText.gameObject.transform.Rotate(new Vector3 (0,0, _rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, -_rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, _rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, -_rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, _rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, -_rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, _rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, -_rotateIndex));
        yield return new WaitForSeconds(_rotateSpeed);
        _oldManText.gameObject.transform.Rotate(new Vector3(0, 0, 0));
    }

    IEnumerator StartBoysSpeech()
    {
        _firstSongSource.Stop();
        _secondSongSource.Play();
        _speechBublesParentGameObject.gameObject.SetActive(true);
        foreach (var item in _speechBubles)
        {
            _speechBubblesAudioSource.Play();
            Instantiate<GameObject>( item , _speechBublesParentGameObject.transform);
            yield return new WaitForSeconds(1.5f);
        }
        _imageTransform.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        _skipButton.gameObject.SetActive(false);
        _loadingPanel.gameObject.SetActive(true);
        _sceneLoading.LoadNextSceneAsync();
    }
}
