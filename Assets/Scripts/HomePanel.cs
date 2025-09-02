using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomePanel : MonoBehaviour
{
    public Button _playBtn;
    public Button _urlBtn;

    public GameObject _homePanel;
    public GameObject _insPanel;
    public InstructionPanel _instruction;


    // Start is called before the first frame update
    void Start()
    {
        _playBtn.onClick.AddListener(PlayBtnClick);
        _urlBtn.onClick.AddListener(UrlBtnClick);

    }


    void PlayBtnClick()
    {
        _insPanel.SetActive(true);
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
