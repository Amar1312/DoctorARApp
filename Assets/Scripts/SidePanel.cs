using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SidePanel : MonoBehaviour
{
    public RectTransform _rectTransform;
    public float _animationEndPoint;
    // Start is called before the first frame update
    void Start()
    {
        //_rectTransform = GetComponent<RectTransform>();
        
        
    }

    private void OnEnable()
    {
        _rectTransform.DOAnchorPosX(_animationEndPoint, 1f).SetEase(Ease.InOutSine);
    }

    public void OffAnimationPanel()
    {
        _rectTransform.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutSine).OnComplete(()=>
        {
            gameObject.SetActive(false);
            UIPanelManager.Instance.AllOnOffPanel(true);
        });
    }

}
