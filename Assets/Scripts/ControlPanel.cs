using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] public Spaceship ship;
    [SerializeField] private Transform leverPivot;
    [SerializeField] private Camera cam;
    [SerializeField] private float joystickSensitivity = 0.5f;
    [Header("Ship")]
    [SerializeField] private int maxStrength = 5;
    [SerializeField] private int minStrength = 1;
    [SerializeField] private TextMeshProUGUI strengthText;
    public Coroutine countdownCoroutine;
    private Coroutine returnStrengthCoroutine;
    private bool grabbed = false;
    float currentAngle = 0;
    private int currentStrength = 1;
    private float previousLeverAngle = 0f;

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;
    private void Start()
    {
        strengthText.text = currentStrength.ToString();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!grabbed)
        {
            leverPivot.localRotation = Quaternion.Lerp(leverPivot.localRotation, Quaternion.identity, Time.deltaTime * 5);
            currentAngle = leverPivot.localRotation.y;
        }
    }
    public void SendIt()
    {
        print("pressed liftoff button");
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
        if (returnStrengthCoroutine != null)
            StopCoroutine(returnStrengthCoroutine);

        countdownCoroutine = StartCoroutine(CountdownText(currentStrength));
        ship.PressButton(currentStrength);
        currentAngle = 0;
        currentStrength = minStrength;
        PlayUISound(0.5f, .7f, true);

    }
    public void LaunchForceUp()
    {
        if (returnStrengthCoroutine != null)
            StopCoroutine(returnStrengthCoroutine);
        currentStrength++;
        if (currentStrength > maxStrength)
            currentStrength = minStrength;
        strengthText.text = currentStrength.ToString();

        PlayUISound(0.8f, 1.2f, true);
    }
    public void LaunchForceDown()
    {
        if (returnStrengthCoroutine != null)
            StopCoroutine(returnStrengthCoroutine);
        currentStrength--;
        if (currentStrength < minStrength)
            currentStrength = maxStrength;
        strengthText.text = currentStrength.ToString();
        PlayUISound(0.8f, 1.2f, true);

    }
    public float stepSize = 2f;
    public void DragLever(PointerEventData eventData)
    {

        float mouseX = eventData.delta.x;
        // Sensitivity
        currentAngle += mouseX * joystickSensitivity;

        currentAngle = Mathf.Round(currentAngle / stepSize) * stepSize;

        // Limit lever movement
        currentAngle = Mathf.Clamp(currentAngle, -30f, 30f);

        // Rotate around Z axis
        leverPivot.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
        //rotate ship
        float angleDifference = currentAngle - previousLeverAngle;

        if (ship != null && angleDifference != 0)
        {
            //ship.RotateDirection(leverPivot.localRotation.eulerAngles.y);
            ship.AimRotation(-leverPivot.localRotation.eulerAngles.y);
        }

        previousLeverAngle = currentAngle;
    }
    public void SmoothLeverReset(bool value)
    {
        grabbed = value;
    }
    private IEnumerator CountdownText(int time)
    {
        int remaining = time;

        while (remaining > 0)
        {
            strengthText.text = remaining.ToString();
            PlayUISound(0.9f, 1.1f, false);

            yield return new WaitForSeconds(1f);

            remaining--;
        }

        strengthText.text = "LIFT OFF";

        yield return new WaitForSeconds(2f);

        strengthText.text = "CRUISING";
        countdownCoroutine = null;

    }
    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        strengthText.text = "LANDED";
        returnStrengthCoroutine = StartCoroutine(ReturnStrengthText());

    }
    IEnumerator ReturnStrengthText()
    {
        yield return new WaitForSeconds(1f);
        strengthText.text = currentStrength.ToString();
    }
    public static T GetRandom<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            return default;

        return list[Random.Range(0, 1)];
    }
    public void PlayUISound(float minPitch, float maxPitch, bool isRandom)
    {
        if (isRandom)
        {
            AudioClip randomClip = GetRandom(audioClips);
            audioSource.clip = randomClip;
        }
        else
            audioSource.clip = audioClips[2];

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
}
