using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using DG.Tweening;

public class MainPanels : MonoBehaviour
{
    public Button _openBtn;
    public Button _backBtn;
    public List<SidePanel> _sidePanel;
    public Image _frameImage;
    public Sprite _normalFrame, _highLightFrame;
    public GameObject _videoQuad;
    public VideoPlayer _videoPlayer;
    public RectTransform _rectTransform;
    bool firstTime;
    Vector2 _orignalPosition;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _orignalPosition = _rectTransform.anchoredPosition;
        OnOffSidePanel(false);
        _openBtn.onClick.AddListener(OpenBtnClick);
        _backBtn.onClick.AddListener(BackBtnClick);
        _frameImage.sprite = _normalFrame;
        _rectTransform.DOAnchorPosY(_orignalPosition.y + 0.15f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void OpenBtnClick()
    {
        UIPanelManager.Instance.AllOnOffPanel(false);
        this.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(true);
        _openBtn.gameObject.SetActive(false);
        OnOffSidePanel(true);
    }

    void BackBtnClick()
    {
        UIPanelManager.Instance.AllOnOffPanel(true);
        _openBtn.gameObject.SetActive(true);
        _backBtn.gameObject.SetActive(false);
        OnOffSidePanel(false);
    }

    void OnOffSidePanel(bool State)
    {
        for (int i = 0; i < _sidePanel.Count; i++)
        {
            _sidePanel[i].gameObject.SetActive(State);
        }
    }

    private void Update()
    {
        if (_videoPlayer.frame > 2)
        {
            if (!_videoQuad.activeInHierarchy && !firstTime)
            {
                _videoQuad.SetActive(true);
                firstTime = true;
            }
        }
        //else
        //{
        //    if (!firstTime)
        //    {
        //        _videoQuad.SetActive(false);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            //Debug.Log("Camera Enter ......");
            _frameImage.sprite = _highLightFrame;
            _videoPlayer.Play();
            //_videoQuad.SetActive(true);
            firstTime = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            //Debug.Log("Camera Exit ......");
            _frameImage.sprite = _normalFrame;
            _videoQuad.SetActive(false);
            _videoPlayer.Stop();

        }
    }
}
