using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace TextSpeech
{
    public class TextToSpeech : MonoBehaviour
    {
        #region Init
        private static TextToSpeech _instance;
        public static TextToSpeech Instance
        {
            get
            {
                if (_instance == null)
                {
                    //Create if it doesn't exist
                    GameObject go = new GameObject("TextToSpeech");
                    _instance = go.AddComponent<TextToSpeech>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
        }
        #endregion


        // Callback management system
        private Dictionary<string, Action> _registeredCallbacks = new Dictionary<string, Action>();
        private string _currentSpeechId = "";

        [System.Obsolete("Use RegisterCallback and StartSpeakWithId instead")]
        public Action onStartCallBack;
        [System.Obsolete("Use RegisterCallback and StartSpeakWithId instead")]
        public Action onDoneCallback;
        public Action<string> onSpeakRangeCallback;

        [Range(0.5f, 2)]
        public float pitch = 1f; //[0.5 - 2] Default 1
        [Range(0.5f, 2)]
        public float rate = 1f; //[min - max] android:[0.5 - 2] iOS:[0 - 1]

        // Constants for duration calculation
        private const float AVERAGE_WPM = 150f; // Average words per minute for speech
        private const float PUNCTUATION_PAUSE = 0.3f; // Additional pause for punctuation

        // Register a callback with a unique identifier
        public void RegisterCallback(string callbackId, Action callback)
        {
            if (_registeredCallbacks.ContainsKey(callbackId))
            {
                _registeredCallbacks[callbackId] = callback;
            }
            else
            {
                _registeredCallbacks.Add(callbackId, callback);
            }
            Debug.Log($"Registered callback: {callbackId}");
        }

        // Unregister a callback
        public void UnregisterCallback(string callbackId)
        {
            if (_registeredCallbacks.ContainsKey(callbackId))
            {
                _registeredCallbacks.Remove(callbackId);
                Debug.Log($"Unregistered callback: {callbackId}");
            }
        }

        // Start speaking with a specific callback identifier
        public void StartSpeakWithId(string text, string callbackId)
        {
            _currentSpeechId = callbackId;
            StartSpeak(text);
        }


        public void Setting(string language, float _pitch, float _rate)
        {
            pitch = _pitch;
            rate = _rate;
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_SettingSpeak(language, pitch, rate / 2);
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("SettingTextToSpeed", language, pitch, rate);
#endif
        }
        public void StartSpeak(string _message)
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StartSpeak(_message);
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("OpenTextToSpeed", _message);
#endif
        }

        // Method that returns duration directly
        public float GetSpeakingDuration(string _message)
        {
            return CalculateSpeakingDuration(_message);
        }

        // Calculate estimated speaking duration based on text content and speech rate
        private float CalculateSpeakingDuration(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0f;

            // Count words
            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;

            // Count sentences and punctuation for pauses
            int sentenceCount = 0;
            int commaCount = 0;

            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                    sentenceCount++;
                else if (c == ',' || c == ';' || c == ':')
                    commaCount++;
            }

            // Base calculation: words per minute adjusted by speech rate
            float adjustedWPM = AVERAGE_WPM * rate;
            float baseDuration = (wordCount / adjustedWPM) * 60f; // Convert to seconds

            // Add pauses for punctuation
            float punctuationPauses = (sentenceCount * PUNCTUATION_PAUSE) + (commaCount * PUNCTUATION_PAUSE * 0.5f);

            // Adjust for pitch (higher pitch tends to be slightly faster)
            float pitchAdjustment = 1f + ((1f - pitch) * 0.1f);

            float totalDuration = (baseDuration + punctuationPauses) * pitchAdjustment;

            return Mathf.Max(1f, totalDuration); // Minimum 1 seconds
        }

        public void StopSpeak()
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StopSpeak();
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("StopTextToSpeed");
#endif
        }

        public void onSpeechRange(string _message)
        {
            if (onSpeakRangeCallback != null && _message != null)
            {
                onSpeakRangeCallback(_message);
            }
        }
        public void onStart(string _message)
        {
            if (onStartCallBack != null)
                onStartCallBack();
        }
        public void onDone(string _message)
        {
            // Call the specific callback if registered
            if (!string.IsNullOrEmpty(_currentSpeechId) && _registeredCallbacks.ContainsKey(_currentSpeechId))
            {
                _registeredCallbacks[_currentSpeechId]?.Invoke();
                Debug.Log($"Called specific callback: {_currentSpeechId}");
            }

            // Call legacy callback for backward compatibility
            if (onDoneCallback != null)
                onDoneCallback();

            // Clear current speech ID
            _currentSpeechId = "";
        }
        public void onError(string _message)
        {
        }
        public void onMessage(string _message)
        {

        }
        /** Denotes the language is available for the language by the locale, but not the country and variant. */
        public const int LANG_AVAILABLE = 0;
        /** Denotes the language data is missing. */
        public const int LANG_MISSING_DATA = -1;
        /** Denotes the language is not supported. */
        public const int LANG_NOT_SUPPORTED = -2;
        public void onSettingResult(string _params)
        {
            int _error = int.Parse(_params);
            string _message = "";
            if (_error == LANG_MISSING_DATA || _error == LANG_NOT_SUPPORTED)
            {
                _message = "This Language is not supported";
            }
            else
            {
                _message = "This Language valid";
            }
            Debug.Log(_message);
        }

#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _TAG_StartSpeak(string _message);

        [DllImport("__Internal")]
        private static extern void _TAG_SettingSpeak(string _language, float _pitch, float _rate);

        [DllImport("__Internal")]
        private static extern void _TAG_StopSpeak();
#endif
    }
}