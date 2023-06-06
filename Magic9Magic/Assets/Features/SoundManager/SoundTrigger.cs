using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _audioSource = Tools.GetComponentWithAssertion<AudioSource>(gameObject);

        if (_audioSource.outputAudioMixerGroup == null)
            Tools.LogError(gameObject.name + ": AudioSource.Output = NULL");
        if (_audioSource.clip == null)
            Tools.LogError(gameObject.name + ": _audioSource.clip == NULL");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.Stop();
        }
    }
}
