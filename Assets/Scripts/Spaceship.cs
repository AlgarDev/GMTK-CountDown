using System.Collections;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
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
                rb.AddForce(transform.up * 500);
                break;
            case 2:
                rb.AddForce(transform.up * 2000);
                break;
            case 3:
                rb.AddForce(transform.up * 3000);
                break;
        }
    }
    IEnumerator Liftoff(int strength)
    {
        print("prssed");
        yield return new WaitForSeconds(strength);
        Jump(strength);
    }
}
