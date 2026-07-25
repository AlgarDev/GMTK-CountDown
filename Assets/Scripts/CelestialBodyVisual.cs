using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyVisual : MonoBehaviour
{
    [SerializeField] Transform atmoParent;

    void Start()
    {
        SetAtmosphereVisual(3);
    }

    public void SetAtmosphereVisual(float radius)
    {
        if (atmoParent != null)
        {
            atmoParent.localScale = Vector3.one * radius;
            atmoParent.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
}