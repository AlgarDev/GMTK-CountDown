using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;

public class CameraControlPanel : MonoBehaviour
{
    public static CameraControlPanel Instance;

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

    public void Shake(float strength = 3)
    {
        CameraShaker.Presets.Explosion2D(strength);
    }
}
