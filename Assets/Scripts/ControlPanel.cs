using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private Transform leverPivot; // parent (rotation point)
    [SerializeField] private Camera cam;
    [SerializeField] private float joystickSensitivity = 0.5f;
    float currentAngle = 0;
    public void LiftOff()
    {
        print("liftoff");
    }
    public void ShiftRight()
    {
        print("right");
    }
    public void ShiftLeft()
    {
        print("left");
    }
    public void LaunchForceUp()
    {
        print("up");
    }
    public void LaunchForceDown()
    {
        print("down");
    }
    public void DragLever(PointerEventData eventData)
    {
        float mouseX = eventData.delta.x;
        // Sensitivity
        currentAngle += mouseX * joystickSensitivity;

        // Limit lever movement
        currentAngle = Mathf.Clamp(currentAngle, -45f, 45f);

        // Rotate around Z axis
        leverPivot.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
