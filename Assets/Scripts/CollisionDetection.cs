using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Spaceship ss;
    private void Start()
    {
        ss = GetComponentInParent<Spaceship>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))            //insere aqui uma merda para saber se oq colide e importante
        {
            if (ss.currentVelocity.magnitude > 2f)
            {
                ss.HasCrashed();

            }
        }
    }
}
