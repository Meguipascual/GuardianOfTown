using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongSelectionManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private string[] _songTitles;
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private TextMeshProUGUI _songTitleText;
    [SerializeField] private TextMeshProUGUI _songLinkText;
    [SerializeField] private float _secondsToShowMessage;
    public AudioSource _audioSource;
    private int _index;


    // Start is called before the first frame update
    void Start()
    {
        _index = Random.Range(0, _audioClips.Length);
        Debug.Log($"Audio: {_audioClips[_index]}, Index: {_index}, Song: {_songTitles[_index]}");
        _audioSource.clip = _audioClips[_index];
        _songTitleText.text = $"{_songTitles[_index]}";
        _songLinkText.text = $"From: https://www.fiftysounds.com";
        _audioSource.Play();
        StartCoroutine(ShowSongTitleInSeconds(_secondsToShowMessage));
    }

    IEnumerator ShowSongTitleInSeconds(float seconds)
    {
        _titlePanel.SetActive(true);
        yield return new WaitForSeconds(seconds);
        _titlePanel.SetActive(false);
    }
}
