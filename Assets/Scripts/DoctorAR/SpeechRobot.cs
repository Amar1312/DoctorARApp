using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using Jaimin;

public class SpeechRobot : Singleton<SpeechRobot>
{
    public Animator _robotAnimator;
    public Animator _homeRobotAnimator;

    private const string OFF_ROTATION = "OffRotetion";
    public bool _isScanning = false;
    public GameObject _tapToPlace;
    // Start is called before the first frame update
    void Start()
    {
        TextToSpeech.Instance.RegisterCallback(OFF_ROTATION, OffRotation);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnDestroy()
    {
        TextToSpeech.Instance.UnregisterCallback(OFF_ROTATION);
    }

    public void OffRotation()
    {
        _robotAnimator.SetBool("spach", false);
        if (_isScanning)
        {
            _tapToPlace.SetActive(true);
            _isScanning = false;
        }
        if (_homeRobotAnimator.gameObject.activeInHierarchy)
        {
            _homeRobotAnimator.SetBool("spach", false);
        }
    }

    public void RobotSpach()
    {
        _robotAnimator.SetBool("spach", true);
        if (_homeRobotAnimator.gameObject.activeInHierarchy)
        {
            _homeRobotAnimator.SetBool("spach", true);
        }
    }


    public void StartSpach(string Data)
    {
        StopSpach();
        RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(Data, OFF_ROTATION);
    }
    public void StopSpach()
    {
        TextToSpeech.Instance.StopSpeak();
    }

}
