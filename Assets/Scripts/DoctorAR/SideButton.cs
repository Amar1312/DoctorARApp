using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideButton : MonoBehaviour
{
    public Button _meBtn;
    public string _titleText;
    public string _mainPanelText;

    public MainPanel _mainPanel;
    public GameObject _clickEffect;
    public TextMeshProUGUI _title;
    public int _meIndex;

    // Start is called before the first frame update
    void Start()
    {
        _meBtn.onClick.AddListener(MeBtnClick);
        _title.text = _titleText;
    }

    void MeBtnClick()
    {
        _mainPanel.SetClickData(_titleText, _mainPanelText, _meIndex);
        if (_clickEffect != null)
        {
            _clickEffect.SetActive(true);
            Invoke(nameof(OffEffect), 1.5f);
        }
    }

    void OffEffect()
    {
        _clickEffect.SetActive(false);
    }
}
