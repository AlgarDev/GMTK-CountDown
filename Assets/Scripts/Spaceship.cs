using System.Collections;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private float forcePerLevel;
    [SerializeField] public Transform visual;
    private Rigidbody rb;


    // Debug
    public Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currentVelocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Liftoff(1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(Liftoff(2));

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(Liftoff(3));

        }
    }
    private void Jump(int strength)
    {
        switch (strength)
        {
            case 1:
                rb.AddForce(transform.up * forcePerLevel);
                break;
            case 2:
                rb.AddForce(transform.up * forcePerLevel * 2);
                break;
            case 3:
                rb.AddForce(transform.up * forcePerLevel * 3);
                break;
        }
    }
    IEnumerator Liftoff(int strength)
    {
        print("prssed");
        yield return new WaitForSeconds(strength);
        Jump(strength);
    }

    public void AddForceToShip(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void RotateShip(Quaternion rotation)
    {
        // rb.MoveRotation(rotation);
    }

    private void OnDrawGizmos()
    {
        if (rb == null) return;

        // Draw the velocity arrow
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rb.velocity);

        // Draw a small sphere at the tip for visibility
        Gizmos.DrawSphere(transform.position + rb.velocity, 0.1f);
    }
}
