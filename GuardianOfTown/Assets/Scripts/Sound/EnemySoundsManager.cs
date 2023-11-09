using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundsManager : MonoBehaviour
{
    public AudioClip[] _hurtSounds;
    public AudioClip[] _deathSounds;
    public AudioClip[] _stepSounds;
    [SerializeField] private float _pitchMin;
    [SerializeField] private float _pitchMax;
    private AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();        
    }

    public void PlayRandomHurtSound()
    {
        if (_hurtSounds.Length != 0)
        {
            _source.clip = _hurtSounds[Random.Range(0, _hurtSounds.Length)];
            _source.pitch = Random.Range(_pitchMin, _pitchMax);
            _source.Play();
        }
    }

    public void PlayRandomDeathSound()
    {
        if (_deathSounds.Length != 0)
        {
            _source.clip = _deathSounds[Random.Range(0, _deathSounds.Length)];
            _source.pitch = Random.Range(_pitchMin, _pitchMax);
            _source.Play();
        }
    }

    public void PlayRandomStepSound()
    {
        if (_stepSounds.Length != 0)
        {
            _source.clip = _stepSounds[Random.Range(0, _stepSounds.Length)];
            _source.pitch = Random.Range(_pitchMin, _pitchMax);
            _source.Play();
        }
    }
}
