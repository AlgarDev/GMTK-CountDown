using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform skull;
    [SerializeField] private Transform heart;
    [SerializeField] private Transform arrow;

    void Update()
    {
        if (target != null && skull != null && arrow != null && heart != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = targetPosition;

            arrow.rotation = target.rotation;

            heart.rotation = Quaternion.identity;
            skull.rotation = Quaternion.identity;
        }
    }
}
