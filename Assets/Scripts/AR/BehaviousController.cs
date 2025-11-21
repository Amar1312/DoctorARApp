using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Amar
{
    public class BehaviousController : MonoBehaviour
    {
        public static BehaviousController Instance;
        //public Text ObjectName;
        //public Text _one;
        //public Text _two;
        public int number = 0;
        //public State _state;
        //public TMP_Dropdown _dropdown;

        [Space]
        [Header("Behaviour")]
        public GameObject Indicator;
        public GameObject ArTap;
        public bool ShowBottom, _canRotate, _canScale;
        public Slider _scaleslider;
        public GameObject _scalePanel, _objSelectionPanel;

        [Space]
        [Header("PLacment Position")]
        public Vector3 PlacementPos;

        [Space]
        [Header("All Combine")]
        private Vector3 startpos;
        private Vector3 EndPos;
        private Vector3 startRot;
        private Vector3 EndRot;
        private Vector3 RotDifferent;
        private Touch _touch;
        private Vector3 ObjectPosStart;
        private Vector3 ObjectPosEnd;

        public GameObject PlacedObject;
        private float _time;
        private float _runtime;
        //public GameObject _deleteObject;
        public int _speedInt;
        public int _rotationInt;
        private float Difference;

        public Text _testtext;
        [SerializeField] ArTapToPlaceObject arTap;

        private float initialDistance;
        private Vector3 initialScale;

        [SerializeField] Button _toggleHologram, _toggleNormal;
        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            OnObjectPlace();
        }

        private void Start()
        {
            _scaleslider.onValueChanged.AddListener(SliderScale);

            if (_toggleHologram != null)
            {
                _toggleHologram.onClick.AddListener(ToggleHologramClick);
            }

            if (_toggleNormal != null)
            {
                _toggleNormal.onClick.AddListener(ToggleNormalClick);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            GetTouch();
            MoveByFinger();
            GetPinch();
        }

        void ToggleHologramClick()
        {
            SceneManager.LoadScene(1);
        }

        void ToggleNormalClick()
        {
            SceneManager.LoadScene(0);
        }

        public void OnObjectSelect(bool select)
        {
            if (select)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                }
            }
        }


        public void OnObjectPlace()
        {
            ArTap.SetActive(true);
            Indicator.SetActive(true);
            Debug.Log("Object Place");
        }



        public void GetTouch()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);
            }

            if (_touch.phase == TouchPhase.Began)
            {
                if (true)
                {
                    //_testtext.text = "Touched";
                    startpos = _touch.position;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        //_testtext.text = "hit";
                        if (hit.collider.tag == "Model")
                        {
                            //_testtext.text = hit.collider.tag;
                            _time = Time.time;
                            _runtime = Time.time;
                            startpos = _touch.position;
                            RemoveMark();
                            PlacedObject = hit.collider.gameObject;
                            //StopAllCoroutines();
                            //StartCoroutine(PlacedObjectSetNull());
                            ObjectPosStart = PlacedObject.transform.position;
                            Debug.DrawRay(this.transform.position, hit.transform.position, Color.green);
                            _scaleslider.value = PlacedObject.transform.localScale.x;
                            PlaceMark();
                            //Debug.Log("slider value" + PlacedObject.transform.localScale.x);
                            //if (PlacedObject.transform.localScale.x > 1)
                            //	UiManager.Instance._ScaleSlider.value = PlacedObject.transform.localScale.x;
                            //else if (PlacedObject.transform.localScale.x < 1)
                            //	UiManager.Instance._ScaleSlider.value = -(1/PlacedObject.transform.localScale.x);
                            //else
                            //	UiManager.Instance._ScaleSlider.value = 0;

                        }
                    }
                }
            }
        }


        //IEnumerator PlacedObjectSetNull()
        //{
        //    print("Mihir");
        //    yield return new WaitForSeconds(4);
        //    RemoveMark();
        //    _scalePanel.SetActive(false);
        //}

        public void RemoveMark()
        {
            if (PlacedObject != null)
            {
                foreach (Transform child in PlacedObject.transform)
                {
                    if (child.name == "Mark")
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            PlacedObject = null;
            arTap.HideObject = false;
        }

        public void ShowDefaultDown()
        {
            ShowBottom = false;
            //UiManager.Instance.ToggleBottom(ShowBottom);
        }


        public void PlaceMark()
        {
            ShowBottom = true;
            ToggleBottom();
            if (PlacedObject != null)
            {
                foreach (Transform child in PlacedObject.transform)
                {
                    if (child.name == "Mark")
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            _scalePanel.SetActive(true);
            _objSelectionPanel.SetActive(false);
            //arTap.RemoveObjReference();
            arTap.HideObject = true;
            //arTap._placeButton.gameObject.SetActive(false);
        }

        public void DeleteObject()
        {
            GameObject obj = PlacedObject;

            Destroy(obj);
            PlacedObject = null;
        }

        public void ToggleBottom()
        {
            if (ShowBottom)
            {
                //UiManager.Instance.ToggleBottom(ShowBottom);
                ShowBottom = false;
            }
        }

        //public void Yes()
        //{
        //    if (PlacedObject != null)
        //    {
        //        Debug.Log("delete.....");
        //        Destroy(PlacedObject);
        //        PlacedObject = null;
        //        ShowBottom = false;
        //        //UiManager.Instance.ToggleBottom(ShowBottom);
        //    }
        //}

        //public void No()
        //{
        //    _deleteObject.SetActive(false);
        //    RemoveMark();
        //}

        public void MoveByFinger()
        {
            if (_touch.phase == TouchPhase.Began)
            {
                startpos = _touch.position;
            }

            if (Input.touchCount == 1 && PlacedObject != null && _touch.phase == TouchPhase.Moved)
            {
                if (!IsPointerOverUIObject())
                {
                    _runtime = Time.time;
                    startpos = _touch.position;
                    ObjectPosition(PlacedObject);
                }
            }

            if (Input.touchCount == 3 && _canRotate)
            {
                if (_touch.phase == TouchPhase.Moved)
                {
                    EndPos = _touch.position;
                    Difference = startpos.x - EndPos.x;

                    if (Difference > 0)
                    {
                        PlacedObject.transform.localEulerAngles += new Vector3(0f, _rotationInt * Time.deltaTime, 0);
                    }

                    if (Difference < 0)
                    {
                        PlacedObject.transform.localEulerAngles -= new Vector3(0f, _rotationInt * Time.deltaTime, 0);
                    }
                }
            }
        }

        public void GetPinch()
        {
            if (Input.touchCount == 2 && _canScale)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touch1.position, touch2.position);
                    initialScale = PlacedObject.transform.localScale;
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                    if (Mathf.Approximately(initialDistance, 0)) return;
                    float scaleFactor = currentDistance / initialDistance;
                    PlacedObject.transform.localScale = initialScale * scaleFactor;
                }
            }
        }

        public void ObjectPosition(GameObject SelectedObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            LayerMask Base = LayerMask.GetMask("Base");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Base))
            {
                if (hit.collider.tag == "base")
                {
                    SelectedObj.transform.position = Vector3.Lerp(SelectedObj.transform.position, hit.point, _speedInt * Time.deltaTime);
                }
            }
        }

        public void SliderScale(float value)
        {
            //if (value == 0)
            //    PlacedObject.transform.localScale = new Vector3(1, 1, 1);
            //else if (value >= 1)
            //    PlacedObject.transform.localScale = new Vector3(1, 1, 1) * value;
            //else if (value <= -1)
            //{
            //    float mult = 1 / value;
            //    PlacedObject.transform.localScale = new Vector3(-1, -1, -1) * mult;
            //}
            //else
            //{
            //    PlacedObject.transform.localScale = new Vector3(1, 1, 1);
            //}

            PlacedObject.transform.localScale = new Vector3(1, 1, 1) * value;

        }

        //public void CustomScale(float value)
        //{
        //    PlacedObject.transform.localScale = new Vector3(value, value, value);

        //    //if (PlacedObject.transform.localScale.x > 1)
        //    //	UiManager.Instance._ScaleSlider.value = PlacedObject.transform.localScale.x;
        //    //else if (PlacedObject.transform.localScale.x < 1)
        //    //	UiManager.Instance._ScaleSlider.value = -(1 / PlacedObject.transform.localScale.x);
        //    //else
        //    //	UiManager.Instance._ScaleSlider.value = 0;

        //}

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    }
}



