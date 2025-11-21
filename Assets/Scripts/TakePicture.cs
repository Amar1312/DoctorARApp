using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TakePicture : MonoBehaviour
{
    public GameObject Screeshot;
    public RawImage _image;
    public GameObject[] _DeactiveCanvas;
    public Texture2D ss;
    //public MyArCamera myArCamera;

    //[SerializeField] Button _Button;

    [SerializeField] GameObject SsBackground;
    public float _timeToDisplaySS;
    //public TextMeshProUGUI _debugText;

    void Start()
    {
        //_Button.onClick.AddListener(ButtonClick);


    }
    public void ButtonClick()
    {
        Screeshot.SetActive(false);
    }

    public void TakeCameraPicture()
    {
        /*			//ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
        #if UNITY_ANDROID
                    if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                    {
                        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                    }
        #endif
                    //NativeToolkit.SaveScreenshot(DateTime.Now.ToString(CultureInfo.InvariantCulture), Application.productName);*/



        //	StartCoroutine(Screenshotsceen());
        StartCoroutine(TakeScreenshotAndSave());
        //Debug.Log("SS");
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        //_DeactiveCanvas.SetActive(false);
        foreach (GameObject item in _DeactiveCanvas)
        {
            item.SetActive(false);
        }
        //SsBackground.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        yield return new WaitForEndOfFrame();

        /*Texture2D*/
        ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        _image.texture = ss;

        //SsBackground.SetActive(false);
        StartCoroutine(IenumStartScreenshot());
        NativeGallery.SaveImageToGallery(ss, "Toll", "Image.png");
        _image.texture = ss;

        // Save the screenshot to Gallery/Photos
        //Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png"));

        yield return new WaitForSeconds(2.0f);
        //// To avoid memory leaks
        //Destroy(ss);

    }


    IEnumerator IenumStartScreenshot()
    {

        Screeshot.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        //Screeshot.SetActive(false);
        //_DeactiveCanvas.SetActive(true);
        foreach (GameObject item in _DeactiveCanvas)
        {
            item.SetActive(true);
        }

        Invoke(nameof(ButtonClick), _timeToDisplaySS);
    }
}


