using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jaimin;

public class AudioManager : Singleton<AudioManager>
{
    public Animator _robotAnimator;
    public Animator _homeRobotAnimator;

    public GameObject _characterMouthHome, _speakingMouthHome, _characterMouthAR, _speakingMouthAR;

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
        if (Index == 0)
        {
            _homeRobotAnimator.SetBool("Hello", true);
            StartCoroutine(PlayAudio(Index, 3f));
        }
        else if(Index == 1)
        {
            _homeRobotAnimator.SetBool("Dance0", true);
            StartCoroutine(PlayAudio(Index, 3f));
        }
        else
           //_robotAnimator.SetBool
            StartCoroutine(PlayAudio(Index, 0f));


        //PlayAudio(Index);

    }

    IEnumerator PlayAudio(int Index, float wait)
    {
        yield return new WaitForSeconds(wait);
        SetCharacterNormal();
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
        _speakingMouthHome.SetActive(false);
        _characterMouthHome.SetActive(true);
        _speakingMouthAR.SetActive(false);
        _characterMouthAR.SetActive(true);


        _homeRobotAnimator.SetBool("Win", true);
        //Invoke(nameof(SetCharacterNormal), 3f);

        if (_isScanning)
        {
            _tapToPlace.SetActive(true);
            _isScanning = false;
            //_homeRobotAnimator.SetBool("Win", true);
        }
    }

    public void SetCharacterNormal()
    {
        _characterMouthHome.SetActive(false);
        _speakingMouthHome.SetActive(true);

        _speakingMouthAR.SetActive(true);
        _characterMouthAR.SetActive(false);


    }

}
