using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour, IGizmosOnEditorTarget
{
    private Spaceship spaceship;
    private SphereCollider myCollider;

    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float pullRadius;
    [SerializeField] private AnimationCurve forceCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private bool active = true;
    [SerializeField] private float timeInactive;
    [SerializeField] private float timeActive;

    private Vector2 direction;
    private float forceToApply;


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
                direction = transform.position - spaceship.transform.position;
                float distance = Vector2.Distance(transform.position, spaceship.transform.position);

                //Rotate to center
                Quaternion targetRotation = Quaternion.FromToRotation(spaceship.transform.up, -direction.normalized) * spaceship.transform.rotation;
                spaceship.RotateShip(targetRotation);

                if (!spaceship.isDocked)
                {
                    // Calculate normalized distance (0 at center, 1 at radius)
                    float normalizedDistance = Mathf.Clamp01(distance / pullRadius);

                    // Get force multiplier from curve
                    float forceMultiplier = forceCurve.Evaluate(normalizedDistance);
                    forceToApply = pullForce * forceMultiplier;

                    // Apply force with curve multiplier
                    spaceship.AddForceToShip(direction.normalized * forceToApply);
                }
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
        if (other.GetComponent<Spaceship>() == true)
        {
            print(other.transform);
            spaceship = other.GetComponent<Spaceship>();
            spaceship.wellCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Spaceship>() == true)
        {
            spaceship.wellCount--;
            spaceship = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (spaceship != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(spaceship.transform.position, spaceship.transform.position + (Vector3)(direction * forceToApply));

            Gizmos.color = Color.green;
            //Gizmos.DrawLine(spaceship.transform.position, transform.position);


        }
    }

    public void GizmosToDraw()
    {
        if (active) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, pullRadius);





        // Draw curve visualization (optional)

    }
}