using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPanel : MonoBehaviour
{
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _containtText;
    public List<GameObject> _gifList;
    public SphereFollow _sperefollw;
    public Transform _robotPoint;
    public List<GameObject> _hightLightCart;

    // Start is called before the first frame update
    void Start()
    {
        //string Data = "Wow what fun, you can watch an interactive holographic interface from any direction, you can get as close and far-reaching as you choose, and feel it appearing in front of you in the real world. " +
        //"Click on each of the IMA's areas of activity and I will provide you with information about them, so that you understand how much it is worth to be part of the Israel Medical Association.";
        //SpeechRobot.Instance.StartSpach(Data);
        AudioManager.Instance.SetClipNumber(3);
        _sperefollw.NearpointSet(_robotPoint.position);
    }

    public void SetClickData(string title, string containt,int Index)
    {
        _containtText.text = containt;
        _titleText.text = title;
        OnOneGif(Index);
    }

    public void OnOneGif(int num)
    {
        for (int i = 0; i < _gifList.Count; i++)
        {
            _gifList[i].SetActive(false);
        }
        _gifList[num].SetActive(true);

        for (int i = 0; i < _hightLightCart.Count; i++)
        {
            _hightLightCart[i].SetActive(false);
        }
        _hightLightCart[num].SetActive(true);
    }
}
