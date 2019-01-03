using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Locomotion Parameters")]
    public float RotationSpeed;
    public float MoveForce;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private float _verticalAxis;
    private float _horizontalAxis;


    private Quaternion ControlOrientationAngle;


    private Quaternion GetControlOrientationAngle()
    {
        return Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, new Vector3(0, 1, 0));
    }
    private void FixedUpdate()
    {
        _verticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");

        Quaternion ControlOrientationAngle = GetControlOrientationAngle();
        
        Vector3 moveVector = new Vector3(_horizontalAxis, 0, _verticalAxis);
        Vector3 targetRotation = Quaternion.LookRotation(moveVector).eulerAngles + ControlOrientationAngle.eulerAngles;
        _rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.deltaTime * RotationSpeed);

        Vector3 moveForce = transform.forward * moveVector.magnitude * MoveForce;
        _rigidbody.AddForce(new Vector3(moveForce.x, 0, moveForce.z));
        _animator.SetFloat("BlendX", _horizontalAxis);
        _animator.SetFloat("BlendY", _verticalAxis);
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

    }
}
