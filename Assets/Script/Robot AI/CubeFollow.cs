using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFollow : MonoBehaviour
{

    public bool continuecheck;

    private void OnEnable()
    {
        if (!continuecheck)
        {
            this.transform.position = new Vector3(Camera.main.transform.position.x,
        this.transform.position.y,
        Camera.main.transform.position.z);

            var Lookat = Quaternion.LookRotation(Camera.main.transform.forward);
            Lookat.x = 0;
            Lookat.z = 0;
            this.transform.rotation = Lookat;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRobotPos()
    {
        this.transform.position = new Vector3(Camera.main.transform.position.x,
this.transform.position.y,
Camera.main.transform.position.z);

        var Lookat = Quaternion.LookRotation(Camera.main.transform.forward);
        Lookat.x = 0;
        Lookat.z = 0;
        this.transform.rotation = Lookat;
    }
}
