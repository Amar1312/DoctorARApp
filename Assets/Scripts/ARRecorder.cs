using System.Collections;
using System.Collections.Generic;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine;
using UnityEngine.Events;
using Amar;

public class ARRecorder : MonoBehaviour
{
    [Header(@"Recording")]
    public Camera videoCamera;
    public int videoWidth = 720;

    private MP4Recorder recorder;
    private CameraInput cameraInput;
    private string lastVideoPath;
    //public VideoPlayerManager _VideoPlayerManager;
    public GameObject VideoMessage;
    public bool _videoStart;
    public TakePicture _tackPicture;


    public void StartRecording()
    {
        Debug.Log("Star Recoding");
        //BehaviousController.Instance.Recording = true;
        // Compute the video width dynamically to match the screen's aspect ratio
        var videoHeight = (int)(videoWidth / videoCamera.aspect);
        videoHeight = videoHeight >> 1 << 1;
        //_VideoPlayerManager.VideoWidth = videoWidth;
        //_VideoPlayerManager.VideoHeight = videoHeight;

        var clock = new RealtimeClock();
        recorder = new MP4Recorder(videoWidth, videoHeight, 30);
        cameraInput = new CameraInput(recorder, clock, videoCamera);
        // Attach an optimized frame input to the camera input for better performance
        //        if (Application.platform == RuntimePlatform.Android)
        //            cameraInput.frameInput = new GLESRenderTextureInput(recorder, multithreading: true);
        //        else if (Application.platform ==  RuntimePlatform.IPhonePlayer)
        //            cameraInput.frameInput = new MTLRenderTextureInput(recorder, multithreading: true);
        if (Screen.orientation == ScreenOrientation.Portrait)
            Screen.orientation = ScreenOrientation.Portrait;
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        else if (Screen.orientation == ScreenOrientation.LandscapeRight)
            Screen.orientation = ScreenOrientation.LandscapeRight;


        _videoStart = true;
    }

    public async void StopRecording()
    {
        Debug.Log("Stop Recoding");
        // Stop camera input and recorder
        cameraInput.Dispose();
        lastVideoPath = await recorder.FinishWriting();
        NativeGallery.SaveVideoToGallery(lastVideoPath, Application.productName,
            "ScreenRecord_" + Application.productName + Random.Range(1000, 10000));

        //BehaviousController.Instance.Recording = false;
        VideoMessage.SetActive(true);
        Invoke(nameof(OffVideoMessage), 1.25f);
        //_VideoPlayerManager.VideoPath = lastVideoPath;
        //VideoRecordingDone.Invoke();
        _videoStart = false;
    }

    void OffVideoMessage()
    {
        VideoMessage.SetActive(false);
    }

    //public UnityEvent VideoRecordingDone;

    public void PointerUPRecoding()
    {
        
        if (_videoStart)
        {
           
            StopRecording();
        }
        else
        {
            _tackPicture.TakeCameraPicture();
        }
    }
}
