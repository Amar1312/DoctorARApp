using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TextSpeech;

public class HomePanel : MonoBehaviour
{
    public Button _playBtn;
    public Button _urlBtn;

    public GameObject _homePanel;
    //public GameObject _insPanel;
    //public InstructionPanel _instruction;
    public GameObject _gameCanvas;
    public GameObject _arCamera;


    // Start is called before the first frame update
    void Start()
    {
        //string Data = "Welcome to the Israel Medical Association AI version, you are about to experience a first-of-its-kind holographic experience in which you can learn about the IMA and understand how important it is to be part of the medical community in Israel. Here are all the benefits and all the benefits";
        _playBtn.onClick.AddListener(PlayBtnClick);
        _urlBtn.onClick.AddListener(UrlBtnClick);
        //SpeechRobot.Instance.StartSpach(Data);
        AudioManager.Instance.SetClipNumber(0);
    }


    void PlayBtnClick()
    {

        //TextToSpeech.Instance.StopSpeak();
        AudioManager.Instance._isScanning = true;
        //string FirstData = "I'll explain to you how to start the experience, look around you through the camera, choose a suitable place and get close to it, scan the ground by moving the phone to the right and left slowly";
        //SpeechRobot.Instance.StartSpach(FirstData);

        AudioManager.Instance.SetClipNumber(1);
        _gameCanvas.SetActive(true);
        _arCamera.SetActive(true);
        Invoke(nameof(Off), 0.5f);
    }

    void Off()
    {
        _homePanel.SetActive(false);
    }

    void UrlBtnClick()
    {
        Application.OpenURL("https://www.mixed.place/");
    }



    public void NextScene()
    {
        SceneManager.LoadScene(0);
    }
}
