using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
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

    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private Canvas menuCanvas;
    private Rigidbody rb;
    public bool isDocked = true;
    private Vector2 directionToGo;
    private Vector2 direction;
    public int wellCount;
    private Vector3 landingRotation;
    public float dragToApply = 0.2f;
    public bool isDead = false;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;
    Dictionary<string, AudioClip> clipDict;
    private Coroutine cruisingSoundCoroutine;
    //Visuals
    private SpaceshipVisuals spaceshipVisuals;

    // Debug
    public Vector3 currentVelocity;

    private void Awake()
    {
        mySpaceship = GetComponent<Spaceship>();
        clipDict = audioClips.ToDictionary(c => c.name, c => c);
    }

    private void Start()
    {
        landingRotation = visual.rotation.eulerAngles;
        rb = GetComponent<Rigidbody>();
        directionToGo = transform.up;
        spaceshipVisuals = GetComponentInChildren<SpaceshipVisuals>();
        CameraController.Instance.FollowState(false);
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (rb == null)
            return;
        currentVelocity = rb.velocity;
        spaceshipVisuals.SetSpeed(currentVelocity.magnitude);

        if (Input.GetKeyDown(KeyCode.Escape) && !isDead && MenuManager.Instance.beginPlay)
        {
            menuCanvas.gameObject.SetActive(!menuCanvas.gameObject.activeSelf);
        }

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
        if (rb == null)
            return;
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
        if (cruisingSoundCoroutine != null)
            StopCoroutine(cruisingSoundCoroutine);
        PlayShipSound("ShipLaunch", false, 1, .5f);

    }
    public void PressButton(int strength)
    {
        StartCoroutine(Liftoff(strength));
    }
    IEnumerator Liftoff(int strength)
    {
        CameraController.Instance.FollowState(true);
        direction = directionToGo;
        spaceshipVisuals.TriggerAnimation("Charge", 1);
        yield return new WaitForSeconds(strength);
        Jump(strength);
        cruisingSoundCoroutine = StartCoroutine(CruisingSound());
        spaceshipVisuals.TriggerAnimation("Launch", 1);
        yield return new WaitForSeconds(.5f);
        HasLanded(false, Vector3.zero);
        //yield return new WaitForSeconds(1f);
        //spaceshipvisuals.TriggerAnimation("Idle");
    }
    IEnumerator CruisingSound()
    {
        yield return new WaitForSeconds(2f);
        PlayShipSound("ShipInMovementLoop", true, 1, .8f);

    }
    public void AddForceToShip(Vector2 force)
    {
        if (rb != null)
        {

        rb.AddForce(force);
        }
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
            if(cruisingSoundCoroutine != null)
                StopCoroutine(cruisingSoundCoroutine);
            PlayShipSound("ShipCollide", false, 1, 0.5f);
        }

    }
    public void HasCrashed()
    {
        CameraControlPanel.Instance.Shake();
        visual.gameObject.SetActive(false);
        parts.gameObject.SetActive(true);
        Destroy(rb);
        rb = null;
        CameraController.Instance.FollowState(false);
        List<Transform> children = new List<Transform>();
        int childCount = parts.transform.childCount;
        isDead = true;
        for (int i = 0; i < childCount; i++)
        {
            children.Add(parts.transform.GetChild(i));
        }
        for (int i = 0; i < childCount; i++)
        {
            Transform piece = children[i];

            Rigidbody pieceRb = piece.GetComponentInChildren<Rigidbody>();

            piece.parent = null;
            if (pieceRb != null)
            {
                Vector3 explosionDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;
                print(explosionDirection);

                pieceRb.AddForce(explosionDirection * 500f);
            }
        }
        print("crashed");
        if (cruisingSoundCoroutine != null)
            StopCoroutine(cruisingSoundCoroutine);
        PlayShipSound("ShipDeath", false, 1, 0.5f);
        StartCoroutine(Lose());

    }
    public void WasSucked()
    {
        isDead = true;
        if (cruisingSoundCoroutine != null)
            StopCoroutine(cruisingSoundCoroutine);
        PlayShipSound("ShipDeath", false, 1, 0.5f);
        StartCoroutine(Lose());
        print("she suck me till my ship count down");
    }
    IEnumerator Lose()
    {
        yield return new WaitForSeconds(2f);
        deathCanvas.gameObject.SetActive(true);

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
    public void PlayShipSound(string clip, bool loop, float pitch, float volume)
    {
        audioSource.clip = clipDict[clip];
        audioSource.loop = loop;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();
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
