using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Movement Parameters")]
    public float CameraMoveSpeed;
    public float CameraRotationSpeed;

    private OrientationCoordinationController _orientationCoordinationController;

    void Awake()
    {
        _orientationCoordinationController = GameObject.FindGameObjectWithTag("OrientationController").GetComponentInChildren<OrientationCoordinationController>(); 
        if(_orientationCoordinationController == null) { Debug.Log("OrientationCoordinationController not found , Ensure that you spawn Player that contains OrientationCoordinationController in Children Hierarchy ");}
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(_orientationCoordinationController.OrientationControllerRotation, transform.rotation,  1f * CameraRotationSpeed);
        Vector3 movePosition = _orientationCoordinationController.ThirdPersonViewCoordinationPoint.position - transform.position;
        transform.position += movePosition * Time.deltaTime * CameraMoveSpeed;
    }
}
