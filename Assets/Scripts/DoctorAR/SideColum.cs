using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideColum : MonoBehaviour
{
    public SphereFollow _sperefollw;
    public Transform _robotPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            _sperefollw.NearpointSet(_robotPoint.position);
        }
    }
}
