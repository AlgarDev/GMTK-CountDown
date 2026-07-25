using UnityEngine;

public class ChasingBlackHole : MonoBehaviour
{
    [SerializeField] private float ascensionSpeed = 0.5f;
    [SerializeField] private Spaceship ship;
    private bool ascending = false;
    private void Start()
    {
        ship = FindObjectOfType<Spaceship>();
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
        if (!ship.isDocked)
        {
            StartAcending();
        }
    }
    public void StartAcending()
    {
        ascending = true;
    }
    private void KillShip()
    {
        //Kill 7 BILLION SHIPS
        ship.WasSucked();

    }
}
