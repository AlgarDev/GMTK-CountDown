using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    [SerializeField] private float pullForce = 10f;
    private Rigidbody spaceshipRB;
    private SphereCollider myCollider;
    private float radius;
    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        radius = myCollider.radius;
    }
    void Update()
    {
        if (spaceshipRB != null)
        {
            float distance = Vector2.Distance(transform.position, spaceshipRB.transform.position);
            Vector2 direction = transform.position - spaceshipRB.transform.position;
            spaceshipRB.AddForce(direction.normalized * pullForce * (radius / distance));

            Quaternion targetRotation = Quaternion.FromToRotation(spaceshipRB.transform.up,-direction.normalized) * spaceshipRB.rotation;

            spaceshipRB.MoveRotation(Quaternion.Slerp(spaceshipRB.rotation,targetRotation,Time.deltaTime * 2f // rotation speed
            ));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Spaceship>() == true)
        {
            print("lol");
            spaceshipRB = other.GetComponent<Rigidbody>();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Spaceship>() == true)
        {
            spaceshipRB = null;

        }
    }

}
