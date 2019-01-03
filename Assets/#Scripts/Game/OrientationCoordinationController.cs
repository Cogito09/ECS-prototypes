using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationCoordinationController : MonoBehaviour
{
    public Transform ThirdPersonViewCoordinationPoint;
    public float FollowPlayerSpeed;
    [HideInInspector]

    public Quaternion OrientationControllerRotation;
    private Transform PlayerTransform;
    private float mouseHorizontalmovement;
    private float mouseVerticalmovement;

    private void Update()
    {
        mouseHorizontalmovement += Input.GetAxis("Mouse X");
        mouseVerticalmovement += Input.GetAxis("Mouse Y");

        OrientationControllerRotation = Quaternion.Euler(mouseVerticalmovement, mouseHorizontalmovement, 0);
        transform.rotation = OrientationControllerRotation;
        Vector3 moveVEctor = PlayerTransform.position - transform.position;
        transform.position += moveVEctor * Time.deltaTime * FollowPlayerSpeed;
    }
    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
