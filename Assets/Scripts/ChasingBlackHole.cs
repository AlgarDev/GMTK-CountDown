using UnityEngine;

public class ChasingBlackHole : MonoBehaviour
{
    [SerializeField] private float ascensionSpeed = 0.5f;
    [SerializeField] private Spaceship ship;
    private bool ascending = false;
    AudioSource audioSource;
    AudioClip audioClip;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [SerializeField] float speed;
    private void Start()
    {
        ship = FindObjectOfType<Spaceship>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    private void Update()
    {
        if (ascending)
        {
            transform.position += Vector3.up * ascensionSpeed * Time.deltaTime;

        }
        if (ascending && ship && ship.transform.position.y < transform.position.y)
        {
            KillShip();
        }
        if (!ship.isDocked && !ascending && !ship.isDead)
        {
            StartAcending(true);
        }
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // converts -1..1 to 0..1
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
    }
    public void StartAcending(bool value)
    {
        ascending = value;
    }
    private void KillShip()
    {
        //Kill 7 BILLION SHIPS
        ship.WasSucked();

    }
}
