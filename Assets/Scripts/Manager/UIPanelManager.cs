using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jaimin;

public class UIPanelManager : Singleton<UIPanelManager>
{
    public List<MainPanels> _allSamePanel;
    public GameObject _mainImage;
    public MainVideoPanel _mainVideoPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void AllOnOffPanel(bool state)
    {
        for (int i = 0; i < _allSamePanel.Count; i++)
        {
            _allSamePanel[i].gameObject.SetActive(state);
        }
        //_mainImage.SetActive(state);
    }

}
