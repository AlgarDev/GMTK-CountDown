using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private Transform target; //use Visual
    [SerializeField] private float smoothFollowSpeed = 0.125f;
    [SerializeField] private float smoothTurnSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private bool followtarget;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFollowSpeed);
            transform.position = smoothedPosition;

            if (followtarget)
            {
                Quaternion desiredRotation = target.rotation;
                Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothTurnSpeed);
                transform.rotation = smoothedRotation;
            }


        }
    }

    public void SetRotation()
    {
        if (target != null)
        {
            transform.rotation = target.rotation;
        }
    }

    public void FollowState(bool state)
    {
        followtarget = state;
    }
}
