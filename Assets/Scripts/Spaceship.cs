using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Spaceship : MonoBehaviour
{
    private Spaceship mySpaceship;
    [SerializeField] private float forcePerLevel;
    [SerializeField] public Transform visual;
    [SerializeField] public Transform parts;


    [SerializeField] private float rotationSpeed;
    [SerializeField] private ControlPanel controlPanel;

    private Rigidbody rb;
    public bool isDocked = true;
    private Vector2 directionToGo;
    private Vector2 direction;
    public int wellCount;
    private Vector3 landingRotation;
    public float dragToApply = 0.2f;
    //Visuals
    private SpaceshipVisuals spaceshipVisuals;

    // Debug
    public Vector3 currentVelocity;

    private void Awake()
    {
        mySpaceship = GetComponent<Spaceship>();
    }

    private void Start()
    {
        landingRotation = visual.rotation.eulerAngles;
        rb = GetComponent<Rigidbody>();
        directionToGo = transform.up;
        spaceshipVisuals = GetComponentInChildren<SpaceshipVisuals>();
        CameraController.Instance.FollowState(false);
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
            CameraController.Instance.FollowState(true);
            //RotateShip(Quaternion.Euler(currentVelocity.normalized));
            //visual.rotation = Quaternion.LookRotation(currentVelocity.normalized);
            //visual.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg);
            RotateShip(Quaternion.Euler(0f, 0f, Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90f));
            print("rotating to direction of movement " + Quaternion.Euler(currentVelocity.normalized));
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
            rb.drag = dragToApply;

    }
    private void Jump(int strength)
    {
        rb.AddForce(direction * forcePerLevel * strength);
        CameraControlPanel.Instance.Shake();
    }
    public void PressButton(int strength)
    {
        StartCoroutine(Liftoff(strength));
    }
    IEnumerator Liftoff(int strength)
    {
        print("prssed");
        CameraController.Instance.FollowState(true);
        direction = directionToGo;
        spaceshipVisuals.TriggerAnimation("Charge", 1);
        yield return new WaitForSeconds(strength);
        Jump(strength);
        spaceshipVisuals.TriggerAnimation("Launch", 1);
        yield return new WaitForSeconds(.5f);
        HasLanded(false, Vector3.zero);
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
    public void HasLanded(bool hasLanded, Vector3 planetPosition)
    {
        if (isDocked == hasLanded)
            return;
        isDocked = hasLanded;
        if (isDocked)
        {
            CameraControlPanel.Instance.Shake();
            landingRotation = visual.rotation.eulerAngles;
            controlPanel.StopCountdown();
            directionToGo = visual.up;

            //bit too fast, se calhar é melhor aumentar a turn speed momentaneamente em vez disto idk
            Vector3 direction = visual.position - planetPosition;
            visual.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
            CameraController.Instance.SetRotation();
            CameraController.Instance.FollowState(false);
        }
    }
    public void HasCrashed()
    {
        CameraControlPanel.Instance.Shake();
        visual.gameObject.SetActive(false);
        parts.gameObject.SetActive(true);
        Destroy(rb);
        rb = null;
        for (int i = 0; i < parts.transform.childCount; i++)
        {
            Transform Go = parts.transform.GetChild(i);
            Go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.value, Random.value, 0));
        }

    }
    public void WasSucked()
    {
        print("she suck me till my ship count down");
    }
    public void AimRotation(float angle)
    {
        visual.localEulerAngles = landingRotation + new Vector3(0, 0, angle);
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
        Gizmos.DrawRay(transform.position, currentVelocity * 10f);

        // Draw a small sphere at the tip for visibility
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, visual.localEulerAngles * 10f);
    }
    public bool IsDocked()
    {
        return isDocked;
    }
}
