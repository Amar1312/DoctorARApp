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
    private bool placementPoseIsValid = false;

    [HideInInspector]
    public GameObject objectToPlace;
    [HideInInspector]
    public Pose placementPose;
    [HideInInspector]
    public int placedObject;
    public GameObject Base;
    [HideInInspector]
    public bool HideObject;

    [Space]
    [Header("Trial Object")]
    public GameObject _TestObject;
    public GameObject _parentObject;
    GameObject InstObj;
    GameObject Hologram;


    [Space]
    [Header("For making line")]
    public LineRenderer lineRenderer;
    public TMP_Text measurementText;
    private Vector3 startPos;
    private Vector3 endPos;

    [Space]
    [Header("Toogle Image to place")]
    public Sprite _Noplace, _place;
    public Button _placeButton;
    public string _width;
    public string _widthHologram;
    public GameObject /*_scalePanel,*/ _scanPanel, _objSelectionPanel;
    public bool hologram, placeHologram;
    public bool hologramone;
    public GameObject _hologram;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        //_TestObject = AppManager.Instance.ObjectToPlace;
        //_width = AppManager.Instance._widthObject;
        Base.SetActive(false);

        //UiManager.Instance._DefaultBottom.Open();

        //Placeobject(_TestObject);
    }

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();


    }



    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void OnDisable()
    {
        //UIManager.Instance.AnimationActive(false);
        BehaviousController.Instance.PlacementPos = placementPose.position;
        foreach (Transform child in _parentObject.transform)
        {
            Destroy(child.gameObject);
        }
        placementPoseIsValid = false;
    }

    public void Placeobject(GameObject SetCurrentGameobject)
    {
        /*if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }*/

        objectToPlace = SetCurrentGameobject;
        SetObject();
#if UNITY_EDITOR
        //SetObject();
#endif
    }

    //public void SetID(int ButtonID)
    //{
    //    placedObject = ButtonID;
    //    Debug.Log("Placed object" + ButtonID);
    //}

    public void SetObject()
    {

#if !UNITY_EDITOR
        if (placementPoseIsValid && hologram && !hologramone)
        {
            placeHologram = true;
            //Base.SetActive(false);
            Hologram = Instantiate(_hologram, placementPose.position, placementPose.rotation, _parentObject.transform);
            Hologram.transform.localEulerAngles = new Vector3(Hologram.transform.localEulerAngles.x, Hologram.transform.localEulerAngles.y + 180, Hologram.transform.localEulerAngles.z);
            //Hologram.transform.localScale = new Vector3(1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()));
        }

        Debug.Log("object set");
        if (placementPoseIsValid && objectToPlace != null && !hologram)
        {
            InstObj = Instantiate(objectToPlace, placementPose.position, placementPose.rotation, _parentObject.transform);
            InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
            InstObj.transform.localScale = new Vector3(1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()));
        }
        else if(objectToPlace != null && hologramone)
        {
            Vector3 posForHolo = new Vector3(Hologram.transform.position.x, Hologram.transform.position.y + 1f, Hologram.transform.position.z);
            InstObj = Instantiate(objectToPlace, posForHolo, placementPose.rotation, _parentObject.transform);
            InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
            InstObj.transform.localScale = new Vector3(InstObj.transform.localScale.x / float.Parse(_widthHologram.Trim()), InstObj.transform.localScale.y / float.Parse(_widthHologram.Trim()), InstObj.transform.localScale.z / float.Parse(_widthHologram.Trim()));
        }

        hologramone = true;
        //HideObject = true;
#endif

#if UNITY_EDITOR
        if (hologram && !hologramone)
        {
            placeHologram = true;
            //Base.SetActive(false);
            //Vector3 basePos = Base.transform.localPosition;
            //basePos.y = 1;
            //Base.transform.localPosition = basePos;


            Hologram = Instantiate(_hologram, placementPose.position, placementPose.rotation, _parentObject.transform);
            Hologram.transform.localEulerAngles = new Vector3(Hologram.transform.localEulerAngles.x, Hologram.transform.localEulerAngles.y + 180, Hologram.transform.localEulerAngles.z);
            //Hologram.transform.localScale = new Vector3(1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()));
        }

        Debug.Log("object set");
        if (objectToPlace != null && !hologram)
        {
            InstObj = Instantiate(objectToPlace, placementPose.position, placementPose.rotation, _parentObject.transform);
            InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
            InstObj.transform.localScale = new Vector3(1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()));
        }
        else if (objectToPlace != null && hologramone && hologram)
        {
            Vector3 posForHolo = new Vector3(Hologram.transform.position.x, Hologram.transform.position.y + 1f, Hologram.transform.position.z);
            InstObj = Instantiate(objectToPlace, posForHolo, placementPose.rotation, _parentObject.transform);
            InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
            InstObj.transform.localScale = new Vector3(InstObj.transform.localScale.x / float.Parse(_widthHologram.Trim()), InstObj.transform.localScale.y / float.Parse(_widthHologram.Trim()), InstObj.transform.localScale.z / float.Parse(_widthHologram.Trim()));
        }
        hologramone = true;

        //if (objectToPlace != null)
        //{
        //    InstObj = Instantiate(objectToPlace, placementPose.position, placementPose.rotation, _parentObject.transform);
        //    InstObj.transform.localEulerAngles = new Vector3(InstObj.transform.localEulerAngles.x, InstObj.transform.localEulerAngles.y + 180, InstObj.transform.localEulerAngles.z);
        //    InstObj.transform.localScale = new Vector3(1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()), 1 / float.Parse(_width.Trim()));
        //}

#endif

        //Base.SetActive(false);
    }

    public void RemoveObjReference()
    {
        objectToPlace = null;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && !HideObject)
        {
            Base.SetActive(true);
            _scanPanel.SetActive(false);
            //_placeButton.gameObject.SetActive(true);
            if (placeHologram)
            {
                placementIndicator.SetActive(false);
                Base.gameObject.transform.position = new Vector3(0, Hologram.transform.position.y + 1f, 0);  //For Horizontal		

            }
            else
            {
                placementIndicator.SetActive(true);
                Base.gameObject.transform.position = new Vector3(0, placementIndicator.transform.position.y, 0);  //For Horizontal		
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            //_placeButton.interactable = true;
            _objSelectionPanel.SetActive(true);
            // _Collider.gameObject.transform.position = new Vector3(0, 0, placementIndicator.transform.position.z);	// For verical
        }
        else
        {
            if (!placementPoseIsValid && !HideObject)
            {
                _scanPanel.SetActive(true);
            }
            else
            {
                _scanPanel.SetActive(false);
            }

            _placeButton.gameObject.SetActive(false);
            _placeButton.interactable = false;

            placementIndicator.SetActive(false);
            _objSelectionPanel.SetActive(false);

        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        var hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }


    //public void AddObject(GameObject _object)
    //{
    //    //objectToPlace.Add(_object);
    //}

    //public void OnTakePictureButtonClick()
    //{
    //    HideObject = !HideObject;
    //    Debug.Log("Toggle" + HideObject);
    //}

}
