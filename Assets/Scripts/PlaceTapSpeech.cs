using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTapSpeech : MonoBehaviour
{
    private void OnEnable()
    {
        //string Data = "Great, you've found a great place, now click on the screen and the display will appear in front of you";
        //SpeechRobot.Instance.StartSpach(Data);
        AudioManager.Instance.SetClipNumber(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
