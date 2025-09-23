using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using DG.Tweening;

public class MainVideoPanel : MonoBehaviour
{
    public Button _backBtn;
    public List<SidePanel> _sidePanel;
    public VideoPlayer _videoPlayer;
    public ParticleSystem _particalEffect;
    public GameObject _defaultImage;
    public GameObject _videoImage;

    // Start is called before the first frame update
    void Start()
    {
        _backBtn.onClick.AddListener(BackBtnClick);
    }

    void BackBtnClick()
    {
        for (int i = 0; i < _sidePanel.Count; i++)
        {
            _sidePanel[i].OffAnimationPanel();
        }
        _particalEffect.Play();
        _videoImage.SetActive(false);
        _defaultImage.SetActive(true);
    }

    public void OpenMainVideo()
    {
        UIPanelManager.Instance.AllOnOffPanel(false);
        OnOffSidePanel(true);
        _particalEffect.Play();
        _videoImage.SetActive(true);
        _defaultImage.SetActive(false);
        _videoPlayer.Play();
    }

    void OnOffSidePanel(bool State)
    {
        for (int i = 0; i < _sidePanel.Count; i++)
        {
            _sidePanel[i].gameObject.SetActive(State);
        }
    }
}
