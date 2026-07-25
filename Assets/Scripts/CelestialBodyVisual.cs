using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyVisual : MonoBehaviour
{
    [SerializeField] Transform atmoParent;
    [SerializeField] Transform surfaceParent;

    void Start()
    {
        //SetAtmosphereVisual(3);
        if (transform.localScale != Vector3.one || surfaceParent.localScale == Vector3.one)
        {
            surfaceParent.localScale = Vector3.Scale(surfaceParent.localScale, transform.localScale);
            transform.localScale = Vector3.one;
        }
    }

    public void SetAtmosphereVisual(float radius)
    {
        if (atmoParent != null)
        {
            atmoParent.localScale = Vector3.one * radius;
            atmoParent.gameObject.SetActive(true);
        }
    }

}