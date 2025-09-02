using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;


[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackingManager : MonoBehaviour
{

    private ARTrackedImageManager TrackedImageManager;
    public List<GameObject> prefabToPlace = new List<GameObject>();
    public List<string> trackStates;
    public List<Sprite> _sprites = new List<Sprite>();
    public TextMeshProUGUI _logMessage, _logMessage2;
    public Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    [SerializeField] Transform _parent;
    //public GameObject _trackedObj;
    [SerializeField] TextMeshProUGUI _debugText;
    public InstructionPanel _InsPanel;

    public GameObject _spawnObj1;

    private void Awake()
    {
        TrackedImageManager = GetComponent<ARTrackedImageManager>();
        // TrackedImageManager.enabled = false;
        print("Tracking Manager Awake Call");

        int i = 0;

        for (int j = 0; j < _sprites.Count; j++)
        {
            //GameObject _spawnObj = prefabToPlace.Find(x => x.name == _sprites[j].name);
            GameObject _spawnObj = prefabToPlace[int.Parse(_sprites[j].name)];
            //_spawnObj.SetActive(false);
            GameObject newARObject = Instantiate(_spawnObj/*, Vector3.zero, Quaternion.identity, _parent*/);
            ////ChildManager.Instance._allChild.Add(newARObject);
            if (newARObject.TryGetComponent<SpawnObject>(out SpawnObject _sp))
            {
                _sp._Panel = _InsPanel;
            }
            //_spawnObj.SetActive(true);
            newARObject.SetActive(false);
            trackStates.Add("None");
            newARObject.name = i.ToString();
            arObjects.Add(newARObject.name, newARObject);
            i++;
        }

    }

    public void CreateObject(GameObject arObject)
    {
        GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
        newARObject.SetActive(false);
        newARObject.name = arObject.name;
        arObjects.Add(arObject.name, newARObject);
        //newARObject.transform.localScale = new Vector3(1, 1, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        TrackedImageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable()
    {
        AllArObjectSetOff();
        TrackedImageManager.trackedImagesChanged -= OnChanged;
    }

    void AllArObjectSetOff()
    {
        //foreach (Transform child in _parent)
        //{
        //    child.gameObject.SetActive(false);
        //}
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        Debug.Log("OnChange Call");
        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            // Handle added event
            TrackingHandler(newImage, true);
        }

        foreach (ARTrackedImage updatedImage in eventArgs.updated)
        {
            //Debug.Log("Updated "+updatedImage.transform.position.x);
            TrackingHandler(updatedImage, false);
        }

        foreach (ARTrackedImage removedImage in eventArgs.removed)
        {
            // Handle removed event
            //Doesn't get called anymore as ARCORE and ARKIT track moving images better now
        }
    }


    public void TrackingHandler(ARTrackedImage image, bool newImage)
    {

        if (newImage)
        {
            Debug.Log("Added");
            trackStates[Int32.Parse(image.referenceImage.name)] = "Tracking";
            arObjects[image.referenceImage.name].transform.position = image.transform.position;
            _logMessage.text = "Adding....  " + arObjects[image.referenceImage.name].transform.position.ToString() + "   status   " + arObjects[image.referenceImage.name].activeSelf;
            StartCoroutine(ExcuteDelay(1));


        }
        else
        {

            _logMessage2.text = image.trackingState.ToString();

            if (image.trackingState.ToString() == "Limited")
            {
                Debug.Log("Limited");
                //check if the image tracking
                trackStates[Int32.Parse(image.referenceImage.name)] = "Limited";
                arObjects[image.referenceImage.name].SetActive(false);

            }

            if (image.trackingState.ToString() == "Tracking")
            {
                // Quaternion q = new Quaternion(image.transform.rotation.x + 90, image.transform.rotation.y, image.transform.rotation.z, image.transform.rotation.w);
                Debug.Log("Tracking");
                arObjects[image.referenceImage.name].transform.position = image.transform.position;
                arObjects[image.referenceImage.name].transform.forward = -image.transform.up;

                //arObjects[image.referenceImage.name].SetActive(true);
                //add the tracking status to the array
                if (trackStates[Int32.Parse(image.referenceImage.name)] == "Limited")
                {
                    StartCoroutine(ExcuteDelay(1));
                    trackStates[Int32.Parse(image.referenceImage.name)] = "Tracking";
                }
            }

        }

        _logMessage.text = image.referenceImage.name + " " + image.trackingState.ToString() + " " + trackStates[Int32.Parse(image.referenceImage.name)];


        IEnumerator ExcuteDelay(float t)
        {
            arObjects[image.referenceImage.name].transform.position = image.transform.position;
            arObjects[image.referenceImage.name].transform.rotation = image.transform.rotation;
            
            yield return new WaitForSeconds(t);
            //_trackedObj = arObjects[image.referenceImage.name];


            //arObjects[image.referenceImage.name].SetActive(true);
            //PlayerPrefs.SetInt("FirstON", 0);
            //PlayerPrefs.SetInt("Tutorial", 0);
            //AnalyticsManager.Instance.ImageTracked();
              

        }

    }

}
