using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;

public class FoodSelect : MonoBehaviour
{
    public static FoodSelect Instance = null;

    //public SampleSpeechToText _smapleSpeech;
    public AIFlowControl _flowControl;
    public SpeechToTextDemo _speechToText;

    public Button _foodMainBtn;
    public List<FoodDishObj> _foodPrefab;
    public List<string> _foodNameList;
    public Transform _followPoint;
    public String _foodFirstDish;
    public GameObject _foodListPanel;

    public ArTapToPlaceObject _arTapPlace;

    // Unique callback identifier for food flow
    private const string FOOD_CALLBACK_ID = "FoodSelect";
    private const string FOOD_START_ID = "FoodStart";
    private const string OFF_ROTATION = "OffRotetion";
    private const string ON_MICK = "OnMick";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _foodMainBtn.onClick.AddListener(FoodMainBtnClick);
        //TextToSpeech.Instance.onDoneCallback = OnSpeech;
        // Register food flow callback
        TextToSpeech.Instance.RegisterCallback(FOOD_CALLBACK_ID, OnSpeech);
        TextToSpeech.Instance.RegisterCallback(FOOD_START_ID, OpenList);
        TextToSpeech.Instance.RegisterCallback(ON_MICK, OnlyOnMich);
        TextToSpeech.Instance.RegisterCallback(OFF_ROTATION, OffRotation);
    }

    void OnDestroy()
    {
        // Always unregister when destroyed
        TextToSpeech.Instance.UnregisterCallback(FOOD_CALLBACK_ID);
        TextToSpeech.Instance.UnregisterCallback(FOOD_START_ID);
        TextToSpeech.Instance.UnregisterCallback(OFF_ROTATION);
        TextToSpeech.Instance.UnregisterCallback(ON_MICK);
    }

    public void FoodMainBtnClick()
    {
        _flowControl._answerText.text = _foodFirstDish;
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(_foodFirstDish, FOOD_START_ID);
    }

    void OpenList()
    {
        ChatGPTUnity.Instance.RobotNormal();
        Debug.Log("Call On Speeck Dish");
        _speechToText._resultSpeech = 3;
        _speechToText.StartSpeechToText();
    }

    public void CheckDish(string Question)
    {
        string ques = Question.ToLower();
        Debug.Log(Question + " dish data");
        if (ques.Contains("menu") || ques.Contains("dishes") || ques.Contains("food") || ques.Contains("options"))
        {
            Speechfood();
        }
    }


    public void Speechfood()
    {
        _flowControl.StopAllLoop();
        _foodListPanel.SetActive(true);
        string foodList = " Here’s our list of dishes. What would you like to try today? \n";
        //string foodList = " select Any Food And Speak food name \n";
        for (int i = 0; i < _foodNameList.Count; i++)
        {
            foodList = foodList + ", " + _foodNameList[i];
        }

        ChatGPTUnity.Instance.RobotSpach();
        _flowControl._answerText.text = foodList;
        TextToSpeech.Instance.StartSpeakWithId(foodList, FOOD_CALLBACK_ID);

    }

    void OnSpeech()
    {
        ChatGPTUnity.Instance.RobotNormal();
        Debug.Log("Call On Speeck food");
        //_smapleSpeech._resultSpeech = 2;
        //_smapleSpeech.StartRecording();
        _speechToText._resultSpeech = 2;
        _speechToText.StartSpeechToText();
    }


    public void SelectFood(string _foodName)
    {
        _foodListPanel.SetActive(false);
        string Food = _foodName.ToLower();
        int IndexMach = GetStringIndex(Food);
        if (IndexMach == -1)
        {
            string Allfood = "";
            for (int i = 0; i < _foodNameList.Count; i++)
            {
                Allfood = Allfood + "," + _foodNameList[i];
            }
            string Ques = "You are a strict food name identifier. \nFrom the list[" + Allfood + "], find the food name that most closely matches " + _foodName + " by spelling similarity, not by meaning. \n If the word exactly or almost matches(e.g.same or minor spelling difference), return the exact name from the list.If there is no close spelling match, return \"Null\".Answer with only one word — the matched food name or Null.";

            ChatAPi(Ques);
        }
        else
        {
            Debug.Log("Mach Food Index :" + IndexMach + " " + _foodName);
            SpawnDish(IndexMach);
        }
    }

    public int GetStringIndex(string search)
    {
        //if (search == null || _foodName == null)
        //{
        //    return -1;
        //}
        int idx = _foodNameList.IndexOf(search);
        if (idx >= 0)
        {
            return idx;
        }
        else
        {
            return -1;
        }
    }

    public void ChatAPi(string que)
    {
        Debug.Log("APi call .... " + que);
        ChatGPTUnity.Instance.ChatGptCall(que, ChatGPTResponse);
    }

    void ChatGPTResponse(bool state, string response)
    {
        Debug.Log(response);
        if (state)
        {
            string _food = response.ToLower();
            int IndexMach = GetStringIndex(_food);
            if (IndexMach == -1)
            {
                string Allfood = "";
                for (int i = 0; i < _foodNameList.Count; i++)
                {
                    Allfood = Allfood + "," + _foodNameList[i];
                }
                string data = "Your Food is not in Food List Try Again From the list[" + Allfood + "]";
                _flowControl._answerText.text = data;
                _foodListPanel.SetActive(true);
                ChatGPTUnity.Instance.RobotSpach();
                TextToSpeech.Instance.StartSpeakWithId(data, FOOD_CALLBACK_ID);
            }
            else
            {
                SpawnDish(IndexMach);
            }
        }
        else
        {
            Debug.Log("Somthing Wrong Try After Sometime");
            string data = "Somthing Wrong Try After Sometime";
            _flowControl._answerText.text = data;
            ChatGPTUnity.Instance.RobotSpach();
            TextToSpeech.Instance.StartSpeakWithId(data,ON_MICK);
        }

    }

    public void SpawnDish(int Index)
    {
        FoodDishObj Food = Instantiate(_foodPrefab[Index]);

        string OkeyText = "Okey, Let me Know where do you want to Place it.";
        _flowControl._answerText.text = OkeyText;
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(OkeyText,OFF_ROTATION);
        Food.gameObject.SetActive(false);
        _arTapPlace._parentObject = Food.gameObject;
        _arTapPlace.objectToPlace = Food.gameObject;
        _arTapPlace.HideObject = false;
        _arTapPlace.gameObject.SetActive(true);
    }

    public void foodPlaceDone()
    {
        _foodListPanel.SetActive(true);
        string foodList = "Here is your food. Do you want more? I have some other food Option too.. Like ";
        for (int i = 0; i < _foodNameList.Count; i++)
        {
            foodList = foodList + ", " + _foodNameList[i];
        }

        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(foodList, FOOD_CALLBACK_ID);
        _flowControl._answerText.text = foodList;

    }

    public void OffFoodFlow()
    {
        _foodListPanel.SetActive(false);
        string flowDone = "Okey, enjoy your food Let me Know if you need to Order any Other food anytime.";
        _flowControl._answerText.text = flowDone;
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(flowDone,OFF_ROTATION);
    }

    public void OffRotation()
    {
        ChatGPTUnity.Instance.RobotNormal();
    }

    public void OnlyOnMich()
    {
        ChatGPTUnity.Instance.RobotNormal();
        _speechToText.StartSpeechToText();
    }

    public void BackPosition()
    {
        string Mess = "Hey!, I am back. Let me know how can I help you ?";
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId(Mess, ON_MICK);
        _flowControl._answerText.text = Mess;
    }

    public void StopSpeak()
    {
        _flowControl._answerText.text = "Okay, I am going.If you need any help, you can simply click mike button at top and I will be available for you.";
        _foodListPanel.SetActive(false);
        ChatGPTUnity.Instance.RobotSpach();
        TextToSpeech.Instance.StartSpeakWithId("Okay, I am going.If you need any help, you can simply click mike button at top and I will be available for you.", OFF_ROTATION);
    }
}
