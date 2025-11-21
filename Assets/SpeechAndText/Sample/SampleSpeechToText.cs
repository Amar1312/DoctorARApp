using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using TMPro;

public class SampleSpeechToText : MonoBehaviour
{
    public bool isShowPopupAndroid = true;
    public GameObject loading;
    public Toggle toggleShowPopupAndroid;
    public InputField inputLocale;
    public InputField inputText;
    public float pitch;
    public float rate;

    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;

    public TextMeshProUGUI _responceText;
    //public AIFlowControl _aiFlow;
    //public FoodSelect _selectFood;
    //public int _resultSpeech;

    void Start()
    {
        Setting("en-US");
        loading.SetActive(false);
        SpeechToText1.Instance.onResultCallback = OnResultSpeech;
#if UNITY_ANDROID
        SpeechToText1.Instance.isShowPopupAndroid = isShowPopupAndroid;
        toggleShowPopupAndroid.isOn = isShowPopupAndroid;
        //toggleShowPopupAndroid.gameObject.SetActive(true);
        Permission.RequestUserPermission(Permission.Microphone);
#else
        toggleShowPopupAndroid.gameObject.SetActive(false);
#endif

    }


    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        SpeechToText1.Instance.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText1.Instance.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }
    void OnResultSpeech(string _data)
    {
        inputText.text = _data;
#if UNITY_IOS
        loading.SetActive(false);
#endif

        //ChatGPTUnity.Instance.ChatGptCall(_data, ChatGPTResponse);
        //if(_resultSpeech == 1)
        //{

        //_aiFlow.ChatAPi(_data);
        //}

        //switch (_resultSpeech)
        //{
        //    case 1:
        //        _aiFlow.ChatAPi(_data);
        //        break;

        //    case 2:
        //        _selectFood.SelectFood(_data);
        //        break;
        //}
    }


    public void SpeakText(string data)
    {
        TextToSpeech.Instance.StartSpeak(data);
    }

    public void OnClickSpeak()
    {
        TextToSpeech.Instance.StartSpeak(inputText.text);
    }

    /// <summary>
    /// </summary>
    public void OnClickStopSpeak()
    {
        TextToSpeech.Instance.StopSpeak();
    }

    /// <summary>
    /// </summary>
    /// <param name="code"></param>
    public void Setting(string code)
    {
        txtLocale.text = "Locale: " + code;
        txtPitch.text = "Pitch: " + pitch;
        txtRate.text = "Rate: " + rate;
        SpeechToText1.Instance.Setting(code);
        TextToSpeech.Instance.Setting(code, pitch, rate);
    }

    /// <summary>
    /// Button Click
    /// </summary>
    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleShowAndroidPopupChanged(bool value)
    {
        isShowPopupAndroid = value;
        SpeechToText1.Instance.isShowPopupAndroid = isShowPopupAndroid;
    }

    public void LoadBackScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
