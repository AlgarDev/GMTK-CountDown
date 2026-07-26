using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform skull;
    [SerializeField] private Transform heart;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform directions;

    void Update()
    {
        if (target != null && skull != null && arrow != null && heart != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = targetPosition;

            arrow.localRotation = target.localRotation;

            //directions.rotation = target.rotation;
            heart.localRotation = target.localRotation;
            skull.localRotation = target.localRotation;


            /*
            heart.rotation = target.rotation;
            heart.position = new Vector3(0, 0, heart.position.z);
            skull.rotation = target.rotation;
            skull.position = new Vector3(0, 0, skull.position.z);
            */
        }
    }
}
