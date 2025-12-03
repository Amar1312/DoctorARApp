using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jaimin;

public class AudioManager : Singleton<AudioManager>
{
    public Animator _robotAnimator;
    public Animator _homeRobotAnimator;

    public List<AudioClip> _audioClip;
    public AudioSource _audioSource;

    public bool _isScanning = false;
    public GameObject _tapToPlace;

    public int _buttonIndex;
    public List<AudioClip> _buttonClickAudio;


    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void SetClipNumber(int Index)
    {
        CancelInvoke(nameof(PlayButtonSound));
        float duration = _audioClip[Index].length;
        Debug.Log(duration + " Time");
        Invoke(nameof(OffRotation), duration);

        _audioSource.clip = _audioClip[Index];
        _audioSource.Play();
        if (Index == 4)
        {
            Invoke(nameof(PlayButtonSound), duration + 1f);
        }

    }

    public void PlayButtonSound()
    {
        _audioSource.clip = _buttonClickAudio[_buttonIndex];
        _audioSource.Play();
    }


    public void RobotSpach()
    {
    }

    public void OffRotation()
    {
        if (_isScanning)
        {
            _tapToPlace.SetActive(true);
            _isScanning = false;
        }
    }

}
