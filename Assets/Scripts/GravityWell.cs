using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour, IGizmosOnEditorTarget
{
    private Spaceship spaceship;
    private SphereCollider myCollider;

    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float pullRadius;

    private bool active = true;
    [SerializeField] private float timeInactive;
    [SerializeField] private float timeActive;


    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.radius = pullRadius;

        active = true;
        if (timeInactive != 0) StartCoroutine(GravityEffect());
    }

    void FixedUpdate()
    {
        if (spaceship != null)
        {
            if (active)
            {
                //Pull
                float distance = Vector2.Distance(transform.position, spaceship.transform.position);
                Vector2 direction = transform.position - spaceship.transform.position;
                spaceship.AddForceToShip(direction.normalized * pullForce * (pullRadius / distance));

                //Rotate to center
                Quaternion targetRotation = Quaternion.FromToRotation(spaceship.transform.up, -direction.normalized) * spaceship.transform.rotation;
                spaceship.RotateShip(Quaternion.Slerp(spaceship.transform.rotation, targetRotation, Time.deltaTime * 2f // rotation speed
                ));
            }

        }
    }

    private IEnumerator GravityEffect()
    {
        while (true)
        {
            active = false;
            yield return new WaitForSeconds(timeInactive);

            active = true;
            yield return new WaitForSeconds(timeActive);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Spaceship>() == true)
        {
            spaceship = other.GetComponentInParent<Spaceship>();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Spaceship>() == true)
        {
            spaceship = null;

        }
    }

    private void OnDrawGizmos()
    {

        if (spaceship != null)
        {
            Vector3 velocity = spaceship.currentVelocity;
            Vector3 toWell = (transform.position - spaceship.transform.position).normalized;

            // Project velocity onto the direction toward the gravity well
            float velocityTowardWell = Vector3.Dot(velocity, toWell);
            Vector3 velocityComponent = toWell * velocityTowardWell;

            // Draw the velocity component as a yellow line
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(spaceship.transform.position, velocityComponent);
        }
    }

    public void GizmosToDraw()
    {
        if (active) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
