using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowLine : MonoBehaviour
{
    public GameObject _targetObj;
    public LineRenderer _line;
    public float _YPos;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Update()
    {
        SetLine();
    }

    public void SetLine()
    {
        Vector3 Campos = transform.position;
        Campos.y = _YPos;
        

        Vector3 tarPos = _targetObj.transform.position;
        tarPos.y = _YPos;

        _line.SetPosition(0, Campos);
        _line.SetPosition(1, tarPos);
    }
}
