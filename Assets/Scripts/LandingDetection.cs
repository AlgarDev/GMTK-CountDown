using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingDetection : MonoBehaviour
{
    Spaceship ss;
    private void Start()
    {
       ss = GetComponentInParent<Spaceship>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Planet"))            //insere aqui uma merda para saber se oq colide e importante
        {
            if (ss.currentVelocity.magnitude < 1f)
            {
                ss.HasLanded(true);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Planet"))            //insere aqui uma merda para saber se oq colide e importante
        //{
        //    ss.HasLanded(false);
        //}
    }
}
