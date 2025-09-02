using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionPanel : MonoBehaviour
{
    public Button _instructionBtn;
    private int Count = 0;

    public GameObject _Environment;
    public Transform _followPoint;
    public TrackingManager _tracking;
    public Animator _animator;

    private void OnEnable()
    {
        //InstructionBtn();
    }

    // Start is called before the first frame update
    void Start()
    {
        _instructionBtn.onClick.AddListener(InstructionBtn);
       
        
    }

    public void InstructionBtn()
    {
        //_tracking.enabled = true;
        gameObject.SetActive(false);
        InstructionBtnClick();
    }

    public void InstructionBtnClick()
    {
        Count = 0;
        //if (!_Environment.activeInHierarchy)
        //{
            LookatCamera();
        //}
        
        gameObject.SetActive(false);

        //_tracking.enabled = false;
        //_tracking._spawnObj1.SetActive(false);
    }

    public void LookatCamera()
    {
        //print(_followPoint.transform.localPosition + "   " + _followPoint.transform.position);
        //_Environment.transform.position = _followPoint.position;
        Vector3 pos = _followPoint.position;
        pos.y = Camera.main.transform.position.y - 2.5f;
        _Environment.transform.position = pos;


        var Lookat = Quaternion.LookRotation(Camera.main.transform.forward);
        Lookat.x = 0;
        Lookat.z = 0;
        _Environment.transform.rotation = Lookat;
        _Environment.SetActive(true);
        Invoke("DisableAnimator", 3f);
        
    }

    void DisableAnimator()
    {
        _animator.enabled = false;
    }

}
