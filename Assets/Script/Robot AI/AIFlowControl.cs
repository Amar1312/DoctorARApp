using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using TMPro;

public class AIFlowControl : MonoBehaviour
{
    //public SampleSpeechToText _speechText;
    public FoodSelect _foodSelect;
    public SpeechToTextDemo _speechToText;
    public TakePicture _takePicture;

    public string _firstMessage;
    public string _cameraMessage;
    public TextMeshProUGUI _answerText;
    public Button _mickBtn;

    private Coroutine _onSpeak;
    private Coroutine _cameraCoroutine;
    private Coroutine _normalCameraCoroutine;


    private const string AI_CALLBACK_ID = "AIFlowControl";
    // Start is called before the first frame update
    void Start()
    {
        _mickBtn.onClick.AddListener(MickBtnClick);
        SetFirstMessage();
        //TextToSpeech.Instance.onDoneCallback = onSpacker;
        TextToSpeech.Instance.RegisterCallback(AI_CALLBACK_ID, onSpacker);
    }
    void OnDestroy()
    {
        // Always unregister when destroyed
        TextToSpeech.Instance.UnregisterCallback(AI_CALLBACK_ID);
    }


    void SetFirstMessage()
    {
        //TextToSpeech.Instance.StartSpeak(_firstMessage);
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(_firstMessage, AI_CALLBACK_ID);
        _answerText.text = _firstMessage;

        //float comTime = TextToSpeech.Instance.GetSpeakingDuration(_firstMessage);
        //Invoke(nameof(onSpacker), comTime + 1f);
    }

    //public IEnumerator OnSpeak()
    //{
    //    _speechText.StartRecording();
    //    yield return new WaitForSeconds(2f);
    //    //_speechText.StopRecording();
    //}
    public void MickBtnClick()
    {
        onSpacker();
    }

    public void onSpacker()
    {
        //StartCoroutine(OnSpeak());
        Debug.Log("on Sheech Now AI");
        ChatGPTUnity.Instance.RobotNormal();
        //_speechText._resultSpeech = 1;
        //_speechText.StartRecording();
        _speechToText._resultSpeech = 1;
        _speechToText.StartSpeechToText();
    }



    public void ChatAPi(string que)
    {
        string Message = que.ToLower();
        //if (Message.Contains("what"))
        //{
        //    if (Message.Contains("this") || Message.Contains("that"))
        //    {
        //        NormalPicture(que);
        //        return;
        //    }

        //}

        if (Message.Contains("food") || Message.Contains("foods"))
        {
            _foodSelect.FoodMainBtnClick();
            return;
        }
        Debug.Log("APi call .... " + que);
        ChatGPTUnity.Instance.ChatGptCallWithImage(que, _takePicture.ss, ChatGPTResponse);

    }

    void ChatGPTResponse(bool state, string response)
    {
        float comTime = 0f;
        Debug.Log(response);

        if (state)
        {
            //_responceText.text = response;
            //TextToSpeech.Instance.StartSpeak(response);
            ChatGPTUnity.Instance.RobotSpach();
            TextToSpeech.Instance.StartSpeakWithId(response, AI_CALLBACK_ID);
            _answerText.text = response;
            Debug.Log(comTime + " responce Time");
            //Invoke(nameof(onSpacker), comTime + 1f);
        }
        else
        {
            //_responceText.text = "Somthing Wrong";
            string wrongData = "Somthing Wrong Try After Sometime";
            //TextToSpeech.Instance.StartSpeak(wrongData);
            ChatGPTUnity.Instance.RobotSpach();
            TextToSpeech.Instance.StartSpeakWithId(wrongData, AI_CALLBACK_ID);
            _answerText.text = wrongData;
        }
        _takePicture.ss = null;


    }


    public IEnumerator CameraBtnClick()
    {
        //CancelInvoke(nameof(onSpacker));

        _takePicture.TakeCameraPicture();

        yield return new WaitForSeconds(_takePicture._timeToDisplaySS);

        //TextToSpeech.Instance.StartSpeak(_cameraMessage);
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(_cameraMessage, AI_CALLBACK_ID);
        _answerText.text = _cameraMessage;

        //ChatAPi("Give Detail About Image ");
    }
    public void PictureClick()
    {
        _cameraCoroutine = StartCoroutine(CameraBtnClick());
    }


    public void StopAllLoop()
    {
        if (_cameraCoroutine != null)
        {
            StopCoroutine(_cameraCoroutine);
            _cameraCoroutine = null;
        }

        if(_normalCameraCoroutine != null)
        {
            StopCoroutine(_normalCameraCoroutine);
            _normalCameraCoroutine = null;
        }
        //CancelInvoke(nameof(onSpacker));
    }

    public void NormalPicture(string Que)
    {
        _normalCameraCoroutine = StartCoroutine(NormalCamera(Que));
    }

    public IEnumerator NormalCamera(string Que)
    {
        _takePicture.TakeCameraPicture();

        yield return new WaitForSeconds(_takePicture._timeToDisplaySS);

        ChatGPTUnity.Instance.ChatGptCallWithImage(Que, _takePicture.ss, ChatGPTResponse);
    }

    
}
