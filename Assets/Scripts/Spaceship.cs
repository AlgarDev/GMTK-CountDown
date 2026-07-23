using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private float forcePerLevel;
    [SerializeField] public Transform visual;

    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    public bool isDocked = true;
    private Vector2 directionToGo;
    private Vector2 direction;
    public int wellCount;

    // Debug
    public Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        directionToGo = transform.up;
    }
    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Alpha1) && isDocked)
        {
            StartCoroutine(Liftoff(1));

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isDocked)
        {
            StartCoroutine(Liftoff(2));

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && isDocked)
        {
            StartCoroutine(Liftoff(3));

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            print("shift left");

            float angle = 10f; // degrees to rotate
            directionToGo = Quaternion.Euler(0, 0, angle) * directionToGo;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print("shift right");

            float angle = 10f; // degrees to rotate
            directionToGo = Quaternion.Euler(0, 0, -angle) * directionToGo;
        }
        if (wellCount == 0)
        {
            RotateShip(Quaternion.FromToRotation(transform.up, currentVelocity));
        }
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.0001f)
        {
            rb.velocity = Vector3.zero;
        }
        if (isDocked && rb.velocity.magnitude < 5f && rb.velocity.magnitude > 0f)
        {
            rb.drag = 2f;
        }
        else if (!isDocked)
            rb.drag = 0;

    }
    private void Jump(int strength)
    {
        switch (strength)
        {
            case 1:
                rb.AddForce(direction * forcePerLevel);
                break;
            case 2:
                rb.AddForce(direction * forcePerLevel * 2);
                break;
            case 3:
                rb.AddForce(direction * forcePerLevel * 3);
                break;
        }
    }
    IEnumerator Liftoff(int strength)
    {
        print("prssed");
        direction = directionToGo;
        yield return new WaitForSeconds(strength);
        Jump(strength);
        yield return new WaitForSeconds(.5f);
        HasLanded(false);
    }

    public void AddForceToShip(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void RotateShip(Quaternion targetRotation)
    {
        // rb.MoveRotation(rotation);
        visual.rotation = Quaternion.Slerp(visual.rotation, targetRotation, Time.deltaTime * 5f);
    }
    public void HasLanded(bool hasLanded)
    {
        isDocked = hasLanded;
    }

    private void OnDrawGizmos()
    {
        if (rb == null) return;

        // Draw the direction arrow
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, directionToGo * 2);

        // Draw a small sphere at the tip for visibility
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + rb.velocity, 0.1f);
    }
}
