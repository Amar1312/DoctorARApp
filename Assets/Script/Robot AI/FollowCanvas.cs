using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var Lookat = Quaternion.LookRotation(Camera.main.transform.forward);
        Lookat.x = 0;
        Lookat.z = 0;
        this.transform.rotation = Lookat;
    }
}
