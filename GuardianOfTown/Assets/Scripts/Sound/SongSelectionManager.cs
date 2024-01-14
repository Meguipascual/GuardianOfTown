using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongSelectionManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private string[] _songTitles;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextMeshProUGUI _songTitleText;
    public AudioSource _audioSource;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _index = Random.Range(0, _audioClips.Length);
        Debug.Log($"Audio: {_audioClips[_index]}, Index: {_index}, Song: {_songTitles[_index]}");
        _audioSource.clip = _audioClips[_index];
        //_songTitleText.text = $"Song title: {_songTitles[_index]}\nMusic from: https://www.fiftysounds.com";
        _audioSource.Play();
        //StartCoroutine(ShowSongTitleInSeconds());
    }

    IEnumerator ShowSongTitleInSeconds()
    {
        _canvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        _canvas.SetActive(false);
    }
}
