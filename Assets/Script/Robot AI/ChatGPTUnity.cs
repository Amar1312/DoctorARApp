using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Jaimin;

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatRequest
{
    public string model = "gpt-4o-mini";
    public Message[] messages;
    public float temperature = 0.7f;
}

[System.Serializable]
public class ChatResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

// Your MonoBehaviour script
public class ChatGPTUnity : Singleton<ChatGPTUnity>
{
    public GameObject _robotMessagePanel;
    public GameObject _thingMessagePanel;

    public Animator _robotAnimator;

    public string apiKey = "YOUR_API_KEY_HERE";
    public string apiUrl = "https://api.openai.com/v1/chat/completions";
    public GameObject _loadingPanel;

    private string cusapiKey = "YOUR_CUSTOMGPT_API_KEY";
    private string projectId = "YOUR_PROJECT_ID";


    #region Check Connection

    public void CheckInternet(Action<bool> action)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            action(true);
        }
        else
        {
            action(false);
        }
    }

    #endregion

    public void ChatGptCall(string question, Action<bool, string> response)
    {
        CheckInternet(status =>
        {
            if (status)
                StartCoroutine(SendChatRequest(question, response));
            else
                Debug.Log("Error No Internet Connection");
        });
    }

    public IEnumerator SendChatRequest(string userQuestion, Action<bool, string> onResponse)
    {
        //_loadingPanel.SetActive(true);
        _robotMessagePanel.SetActive(false);
        _thingMessagePanel.SetActive(true);
        Message userMsg = new Message { role = "user", content = userQuestion };
        ChatRequest req = new ChatRequest
        {
            messages = new Message[] { userMsg }
        };
        string json = JsonUtility.ToJson(req);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error + " — " + www.downloadHandler.text);
                onResponse?.Invoke(false, "Error: " + www.error);
            }
            else
            {
                string respJson = www.downloadHandler.text;
                ChatResponse resp = JsonUtility.FromJson<ChatResponse>(respJson);
                string answer = resp.choices[0].message.content;
                onResponse?.Invoke(true, answer);
            }
            //_loadingPanel.SetActive(false);
            _thingMessagePanel.SetActive(false);
            _robotMessagePanel.SetActive(true);
        }
    }

    public void ChatGptCallWithImage(string question, Texture2D image, Action<bool, string> response)
    {
        CheckInternet(status =>
        {
            if (status)
                StartCoroutine(SendChatRequestWithImage(question, image, response));
            else
                Debug.Log("Error No Internet Connection");
        });
    }

    public IEnumerator SendChatRequestWithImage(string userQuestion, Texture2D image, Action<bool, string> onResponse)
    {
        Debug.Log("In API");
        //_loadingPanel.SetActive(true);
        _thingMessagePanel.SetActive(true);
        _robotMessagePanel.SetActive(false);

        // Build JSON string manually
        string json;

        if (image != null)
        {
            //  Convert image to Base64
            byte[] imageBytes = image.EncodeToPNG();
            string imageBase64 = Convert.ToBase64String(imageBytes);

            //  Correct JSON for GPT-4-vision
            json = $@"
        {{
            ""model"": ""gpt-4o-mini"",
            ""messages"": [
                {{
                    ""role"": ""user"",
                    ""content"": [
                        {{""type"": ""text"", ""text"": ""{userQuestion.Replace("\"", "\\\"")}""}},
                        {{""type"": ""image_url"", ""image_url"": {{ ""url"": ""data:image/png;base64,{imageBase64}"" }} }}
                    ]
                }}
            ]
        }}";
        }
        else
        {
            // Build JSON for text-only question
            json = $@"
        {{
            ""model"": ""gpt-4o-mini"",
            ""messages"": [
                {{
                    ""role"": ""user"",
                    ""content"": [
                        {{""type"": ""text"", ""text"": ""{userQuestion.Replace("\"", "\\\"")}""}}
                    ]
                }}
            ]
        }}";
        }

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error + " — " + www.downloadHandler.text);
                onResponse?.Invoke(false, "Error: " + www.error);
            }
            else
            {
                string respJson = www.downloadHandler.text;
                //Debug.Log(respJson);

                // Because GPT-4-vision responses can vary, it’s safer to parse manually
                // Extract text response safely
                string answer = "";
                try
                {
                    int contentIndex = respJson.IndexOf("\"content\":");
                    if (contentIndex != -1)
                    {
                        int quoteStart = respJson.IndexOf("\"", contentIndex + 10);
                        int quoteEnd = respJson.IndexOf("\"", quoteStart + 1);
                        answer = respJson.Substring(quoteStart + 1, quoteEnd - quoteStart - 1);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Parsing error: " + e.Message);
                }

                onResponse?.Invoke(true, answer);
            }

            //_loadingPanel.SetActive(false);
            _thingMessagePanel.SetActive(false);
            _robotMessagePanel.SetActive(true);
        }
    }


    // Step 2: Ask Question from Document
    public void SendQuestion(string question, Action<bool, string> response)
    {
        CheckInternet(status =>
        {
            if (status)
            {
                if (string.IsNullOrEmpty(question))
                {
                    //responseText.text = "Please enter a question.";
                    response?.Invoke(false, "Please enter a question.");
                    return;
                }
                StartCoroutine(SendQueryToCustomGPT(question, response));
            }
            else
                Debug.Log("Error No Internet Connection");
        });
    }

    private IEnumerator SendQueryToCustomGPT(string question, Action<bool, string> onResponse)
    {
        string url = $"https://app.customgpt.ai/api/v1/projects/{projectId}/query";
        string jsonData = JsonUtility.ToJson(new QueryData(question));

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + cusapiKey);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                //responseText.text = ParseCustomGPTResponse(response);
                onResponse?.Invoke(true, ParseCustomGPTResponse(response));
            }
            else
            {
                //responseText.text = " Query failed: " + www.error;
                onResponse?.Invoke(false, " Query failed: ");
            }
        }
    }

    private string ParseCustomGPTResponse(string json)
    {
        // Optional: adjust this parsing based on CustomGPT response format
        var data = JsonUtility.FromJson<ResponseData>(json);
        return data.answer ?? "No answer found.";
    }

    public void RobotSpach()
    {
        _robotAnimator.SetBool("spach", true); 
    }

    public void RobotNormal()
    {
        _robotAnimator.SetBool("spach", false);
    }

    [System.Serializable]
    public class QueryData
    {
        public string query;
        public QueryData(string question)
        {
            query = question;
        }
    }

    [System.Serializable]
    public class ResponseData
    {
        public string answer;
    }
}
