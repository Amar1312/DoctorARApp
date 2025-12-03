using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.UI;
using Amar;
using TMPro;


public class ArTapToPlaceObject : MonoBehaviour
{
    public static ArTapToPlaceObject Instance;
    public GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;
    //public ARPointCloudManager cloudPoints;
    private bool placementPoseIsValid = false;

    public GameObject objectToPlace;
    [HideInInspector]
    public Pose placementPose;
    public GameObject Base;
    [HideInInspector]
    public bool HideObject;

    [Space]
    [Header("Trial Object")]
    public GameObject _parentObject;
    GameObject InstObj;

    //public GameObject _navmesh;
    //public float navmeshDistance = -10;
    //public TMP_Text _GoatDistance;

    [Space]
    [Header("Toogle Image to place")]
    public Button _placeButton;
    public GameObject _scanmsg;
    //public ARCanvas _arCanvas;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Base.SetActive(false);
    }

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        _placeButton.onClick.AddListener(Placeobject);

//#if UNITY_EDITOR
//        Placeobject();
//        _parentObject.SetActive(true);
//        _parentObject.transform.position = placementPose.position;
//        _parentObject.transform.forward = placementPose.forward;
//#endif
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        // MakeLine();
    }

    private void OnDisable()
    {
        //UIManager.Instance.AnimationActive(false);
        //BehaviousController.Instance.PlacementPos = placementPose.position;
        //foreach (Transform child in _parentObject.transform)
        //{
        //    Destroy(child.gameObject);
        //}
        placementPoseIsValid = false;
    }

    public void Placeobject()
    {
        /*if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			PlaceObject();
		}*/

        SetObject();
        //_arCanvas.StartTimer();

    }

    public void SetObject()
    {
        Debug.Log("object set");
        if (placementPoseIsValid && objectToPlace != null)
        {
            //navmeshDistance = placementPose.position.y;
            //_GoatDistance.text = navmeshDistance.ToString();
            _placeButton.gameObject.SetActive(false);
            _parentObject.SetActive(true);
            //cloudPoints.SetTrackablesActive(false);


            _parentObject.transform.position = placementPose.position;
            _parentObject.transform.forward = placementPose.forward;
            //InstObj = Instantiate(objectToPlace, placementPose.position, placementPose.rotation, _parentObject.transform);
            //InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
            HideObject = true;
            Debug.Log("Place Object in Scann");
            // UiManager.Instance.desktopscale = InstObj.transform.localScale.x;
            //StartCoroutine(IenumAnimation());
            
        }


#if UNITY_EDITOR
        InstObj = Instantiate(objectToPlace, placementPose.position, placementPose.rotation, _parentObject.transform);
        InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
        //UiManager.Instance.desktopscale = InstObj.transform.localScale.x;
        //StartCoroutine(IenumAnimation());
#endif


    }


    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && !HideObject)
        {
            //UiManager.Instance.StartTutorial();
            //UiManager.Instance.AnimationActive(false);
            Base.SetActive(true);
            //UIManager.Instance.SwitchTotorial(0);

            placementIndicator.SetActive(true);
            _scanmsg.SetActive(false);
            _placeButton.gameObject.SetActive(true);
            _placeButton.interactable = true;
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            //Base.gameObject.transform.position = new Vector3(0, placementIndicator.transform.position.y, 0);  //For Horizontal		
            // _Collider.gameObject.transform.position = new Vector3(0, 0, placementIndicator.transform.position.z);	// For verical
        }
        else
        {
            if (!HideObject)
                _scanmsg.SetActive(true);
            else
                _scanmsg.SetActive(false);

            _placeButton.gameObject.SetActive(false);
            _placeButton.interactable = false;
            placementIndicator.SetActive(false);
            //SpeechRobot.Instance.StopSpach();
            //UIManager.Instance.AnimationActive(true);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        var hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public void AddObject(GameObject _object)
    {
        //objectToPlace.Add(_object);
    }

    public void OnTakePictureButtonClick()
    {
        HideObject = !HideObject;
        Debug.Log("Toggle" + HideObject);
    }



}
