using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GizmosOnEditor : MonoBehaviour
{
    IGizmosOnEditorTarget target;

    void Update()
    {
        if (target == null)
        {
            target = GetComponent<IGizmosOnEditorTarget>();
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            target.GizmosToDraw();
        }
    }
}
