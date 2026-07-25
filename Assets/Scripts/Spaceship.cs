using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private float forcePerLevel;
    [SerializeField] public Transform visual;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private ControlPanel controlPanel;
    private Rigidbody rb;
    public bool isDocked = true;
    private Vector2 directionToGo;
    private Vector2 direction;
    public int wellCount;
    private Vector3 landingRotation;
    //Visuals
    [SerializeField] private SpaceshipVisuals spaceshipVisuals;

    // Debug
    public Vector3 currentVelocity;



    private void Start()
    {
        landingRotation = visual.rotation.eulerAngles;
        rb = GetComponent<Rigidbody>();
        directionToGo = transform.up;
        spaceshipVisuals = GetComponentInChildren<SpaceshipVisuals>();
    }
    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.velocity;
        spaceshipVisuals.SetSpeed(currentVelocity.magnitude);
        //print("Current speed : " + currentVelocity.magnitude);

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    print("shift left");

        //    float angle = 10f; // degrees to rotate
        //    directionToGo = Quaternion.Euler(0, 0, angle) * directionToGo;
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    print("shift right");

        //    float angle = 10f; // degrees to rotate
        //    directionToGo = Quaternion.Euler(0, 0, -angle) * directionToGo;
        //}
        if (wellCount == 0 && !isDocked)
        {
            RotateShip(Quaternion.Euler(currentVelocity.normalized));
            print("rotating to direction of movement");
        }
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.0001f)
        {
            rb.velocity = Vector3.zero;
        }
        if (isDocked && rb.velocity.magnitude < 1f && rb.velocity.magnitude > 0f)
        {
            rb.drag = 2f;
        }
        else if (!isDocked && wellCount == 0)
            rb.drag = 0f;
        else if (!isDocked && wellCount != 0)
            rb.drag = 0.2f;

    }
    private void Jump(int strength)
    {
        rb.AddForce(direction * forcePerLevel * strength);
    }
    public void PressButton(int strength)
    {
        StartCoroutine(Liftoff(strength));
    }
    IEnumerator Liftoff(int strength)
    {
        print("prssed");
        direction = directionToGo;
        spaceshipVisuals.TriggerAnimation("Charge", 1);
        yield return new WaitForSeconds(strength);
        Jump(strength);
        spaceshipVisuals.TriggerAnimation("Launch", 1);
        yield return new WaitForSeconds(.5f);
        HasLanded(false);
        //yield return new WaitForSeconds(1f);
        //spaceshipvisuals.TriggerAnimation("Idle");
    }

    public void AddForceToShip(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void RotateShip(Quaternion targetRotation)
    {
        // rb.MoveRotation(rotation);
        visual.rotation = Quaternion.Slerp(visual.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    public void HasLanded(bool hasLanded)
    {
        if (isDocked == hasLanded)
            return;
        isDocked = hasLanded;
        if (isDocked)
        {
            landingRotation = visual.rotation.eulerAngles;
            controlPanel.StopCountdown();
            directionToGo = visual.up;
        }
    }
    public void AimRotation(float angle)
    {
        visual.localEulerAngles = landingRotation + new Vector3(0,0,angle);
        directionToGo = visual.up;
    }

    public void RotateDirection(float angle)
    {
        //directionToGo = Quaternion.Euler(0, 0, angle) * directionToGo;
        visual.rotation *= Quaternion.Euler(0, 0, angle);
    }
    private void OnDrawGizmos()
    {
        if (rb == null) return;

        // Draw the direction arrow
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, directionToGo * 5);

        // Draw a small sphere at the tip for visibility
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + rb.velocity, 0.1f);
    }
    public bool IsDocked()
    {
        return isDocked;
    }
}
