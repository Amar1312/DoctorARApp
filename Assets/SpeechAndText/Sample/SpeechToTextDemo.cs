using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TextSpeech;

public class SpeechToTextDemo : MonoBehaviour, ISpeechToTextListener
{
    public TextMeshProUGUI SpeechText;
    public AIFlowControl _flowControl;
    //public Button StartSpeechToTextButton, StopSpeechToTextButton;
    //public Slider VoiceLevelSlider;
    public bool PreferOfflineRecognition;

    private float normalizedVoiceLevel;

    public GameObject _speechPanel;
    public AIFlowControl _aiFlow;
    public FoodSelect _selectFood;
    public int _resultSpeech;

    public AudioClip _onClip;
    public AudioClip _offClip;
    public AudioSource _audioSource;
    public SphereFollow _spherFollw;

    private void Awake()
    {
        SpeechToText.Initialize("en-US");

        //StartSpeechToTextButton.onClick.AddListener(StartSpeechToText);
        //StopSpeechToTextButton.onClick.AddListener(StopSpeechToText);
    }


    //private void Update()
    //{
    //    StartSpeechToTextButton.interactable = SpeechToText.IsServiceAvailable(PreferOfflineRecognition) && !SpeechToText.IsBusy();
    //    StopSpeechToTextButton.interactable = SpeechToText.IsBusy();

    //    // You may also apply some noise to the voice level for a more fluid animation (e.g. via Mathf.PerlinNoise)
    //    //VoiceLevelSlider.value = Mathf.Lerp(VoiceLevelSlider.value, normalizedVoiceLevel, 15f * Time.unscaledDeltaTime);
    //}

    public void ChangeLanguage(string preferredLanguage)
    {
        if (!SpeechToText.Initialize(preferredLanguage))
            SpeechText.text = "Couldn't initialize with language: " + preferredLanguage;
    }

    public void StartSpeechToText()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                _speechPanel.SetActive(true);
                if (SpeechToText.Start(this, preferOfflineRecognition: PreferOfflineRecognition))
                    SpeechText.text = "";
                else
                    SpeechText.text = "Couldn't start speech recognition session!";
            }
            else
                SpeechText.text = "Permission is denied!";
        });

        _audioSource.clip = _onClip;
        _audioSource.Play();
    }

    public void StopSpeechToText()
    {
        SpeechToText.ForceStop();
    }

    void ISpeechToTextListener.OnReadyForSpeech()
    {
        Debug.Log("OnReadyForSpeech");
    }

    void ISpeechToTextListener.OnBeginningOfSpeech()
    {
        Debug.Log("OnBeginningOfSpeech");
    }

    void ISpeechToTextListener.OnVoiceLevelChanged(float normalizedVoiceLevel)
    {
        // Note that On Android, voice detection starts with a beep sound and it can trigger this callback. You may want to ignore this callback for ~0.5s on Android.
        this.normalizedVoiceLevel = normalizedVoiceLevel;
    }

    void ISpeechToTextListener.OnPartialResultReceived(string spokenText)
    {
        Debug.Log("OnPartialResultReceived: " + spokenText);
        SpeechText.text = spokenText;
    }

    void ISpeechToTextListener.OnResultReceived(string spokenText, int? errorCode)
    {
        Debug.Log("OnResultReceived: " + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : ""));
        SpeechText.text = spokenText;
        normalizedVoiceLevel = 0f;
        Debug.Log("error code " + errorCode);
        _audioSource.clip = _offClip;
        _audioSource.Play();
        if (errorCode == 11)
        {
            StartSpeechToText();
            return;
        }
        if (errorCode == 6 || errorCode == 7)
        {
            //_speechPanel.SetActive(false);
            //Invoke(nameof(OnMick), 1f);
            return;
        }
        // Recommended approach:
        // - If errorCode is 0, session was aborted via SpeechToText.Cancel. Handle the case appropriately.
        // - If errorCode is 9, notify the user that they must grant Microphone permission to the Google app and call SpeechToText.OpenGoogleAppSettings.
        // - If the speech session took shorter than 1 seconds (should be an error) or a null/empty spokenText is returned, prompt the user to try again (note that if
        //   errorCode is 6, then the user hasn't spoken and the session has timed out as expected).
        _speechPanel.SetActive(false);

        string ques = spokenText.ToLower();

        if (ques.Contains("what"))
        {
            if (ques.Contains("this") || ques.Contains("that"))
            {
                _aiFlow.NormalPicture(spokenText);
                return;
            }
        }

        if (ques.Contains("follow"))
        {
            _spherFollw.BackPosition();
            _selectFood.BackPosition();
            return;
        }

        if (ques.Contains("stop"))
        {
            FoodSelect.Instance.StopSpeak();      
           // TextToSpeech.Instance.StartSpeak("Okay, I am going. If you need any help, you can simply click mike button at top and I will be available for you.");
            return;
        }

        //if (_resultSpeech == 2 || _resultSpeech == 3)
        //{
        //    if (ques.Contains("stop"))
        //    {
        //        _selectFood.OffFoodFlow();
        //        TextToSpeech.Instance.StartSpeak("Okay, I am going. If you need any help you can simply click mike button at top");
        //        return;
        //    }
        //}


        switch (_resultSpeech)
        {
            case 1:
                _aiFlow.ChatAPi(spokenText);
                break;

            case 2:
                _selectFood.SelectFood(spokenText);
                break;

            case 3:
                _selectFood.CheckDish(spokenText);
                break;
        }
    }

    void OnMick()
    {
        StartSpeechToText();
    }
}