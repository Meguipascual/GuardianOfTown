using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectionManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioClip[] _redFogAudioClips;
    [SerializeField] private string[] _songTitles;
    [SerializeField] private string[] _redFogSongTitles;
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private TextMeshProUGUI _songTitleText;
    [SerializeField] private TextMeshProUGUI _songLinkText;
    [SerializeField] private float _secondsToShowMessage;
    public AudioSource _audioSource;
    private int _index;


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == Tags.WorldTouch && DataPersistantManager.Instance.Stage >= GameManager.Instance.StageToActivateRedFog)
        {
            RedFogSongsPlay();
        }
        else
        {
            NormalSongsPlay();
        }
    }

    private void NormalSongsPlay()
    {
        _index = Random.Range(0, _audioClips.Length);
        _audioSource.clip = _audioClips[_index];
        _songTitleText.text = $"{_songTitles[_index]}";
        _songLinkText.text = $"From: https://www.fiftysounds.com";
        _audioSource.Play();
        StartCoroutine(ShowSongTitleInSeconds(_secondsToShowMessage));
    }

    private void RedFogSongsPlay()
    {
        _index = Random.Range(0, _redFogAudioClips.Length);
        _audioSource.clip = _redFogAudioClips[_index];
        _songTitleText.text = $"{_redFogSongTitles[_index]}";
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
