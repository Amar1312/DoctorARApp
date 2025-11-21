using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.SceneManagement;


public class SphereFollow : MonoBehaviour
{
    public bool _followCamera;
    public GameObject _spherePoint ;
    public bool _back;

    public LayerMask _layer;
    public LineRenderer _line;

    private Camera _mainCamera;
    public Vector3 _postiontoFollow;

    private bool changingpos;

    // Start is called before the first frame update
    void Start()
    {
        _followCamera = true;
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void  LateUpdate()
    {
        if (_followCamera)
        {
            float disX = this.transform.position.x - Camera.main.transform.position.x;
            float disz = this.transform.position.z - Camera.main.transform.position.z;

            //Debug.Log(disX + " " + disz);
            if((Mathf.Abs(disX) > 2 || Mathf.Abs(disz) > 2) && !changingpos)
            {
                //Debug.Log("changing pos");
                _spherePoint.GetComponentInParent<CubeFollow>().SetRobotPos();
                changingpos = true;

                //var Lookat1 = Quaternion.LookRotation(Camera.main.transform.forward);
                //Lookat1.x = 0;
                //Lookat1.z = 0;
                //this.transform.rotation = Lookat1;

            }

            if (changingpos)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, _spherePoint.transform.position, 0.1f);
              

                
                if (this.transform.position == _spherePoint.transform.position)
                {
                    changingpos = false;
                    _back = false;
                    this.transform.rotation = _spherePoint.transform.rotation;
                    //this.transform.rotation = Vector3.RotateTowards(this.transform.rotation, _spherePoint.transform.rotation, 1f,1f);
                }
                else
                {
                    if (_back)
                    {
                        Debug.Log("changing");
                        Vector3 relativePos = transform.position - _spherePoint.transform.position;


                        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
                        Debug.Log("this rotation" + transform.rotation);
                    }

                }

            }

            else
            {
              
                //var Lookat = Quaternion.LookRotation(Camera.main.transform.forward);
                //Lookat.x = 0;
                //Lookat.z = 0;
                //this.transform.rotation = Lookat;
            }


            //this.transform.position = Vector3.MoveTowards(this.transform.position, _spherePoint.transform.position, 0.8f);

            //transform.position = _spherePoint.transform.position;
            //this.transform.rotation = _spherePoint.transform.rotation;
        }
        else
        {
         

            Vector3 follow = new Vector3(_postiontoFollow.x, _postiontoFollow.y + 1.5f, _postiontoFollow.z);
               this.transform.position = Vector3.MoveTowards(this.transform.position, follow, 0.1f);

            if (this.transform.position == follow)
            {
                var Lookat = Quaternion.LookRotation(-Camera.main.transform.forward);
                Lookat.x = 0;
                Lookat.z = 0;
                this.transform.rotation = Lookat;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _layer))
            {
                
                //Debug.Log("Mouse Click");
                _postiontoFollow = hitInfo.point;
                _followCamera = false;

               // TextToSpeech.Instance.StartSpeak("Okay, I am going there. Just tell me  'Follow me'.  If you need help. I will come bak to you.");
                //var Lookat = Quaternion.LookRotation(_postiontoFollow);
                //Lookat.x = 0;
                //Lookat.z = 0;
                //this.transform.rotation = Lookat;
            }

        }

        
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    _followCamera = true;
        //}

    }

    public void NearpointSet(Vector3 Point)
    {
        Vector3 Final = new Vector3(Point.x, -1.6f, Point.z);
        _postiontoFollow = Final;
        _followCamera = false;
    }


    public void BackPosition()
    {
        _followCamera = true;
        _back = true;
    }

    public void LoadBackScene()
    {
        TextToSpeech.Instance.StopSpeak();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
