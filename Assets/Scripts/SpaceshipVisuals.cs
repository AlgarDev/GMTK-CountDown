using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipVisuals : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float maxSpeed = 5;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAnimation(string trigger, int time)
    {
        if (animator != null)
        {
            animator.SetTrigger(trigger);
            animator.speed = time;
        }
    }

    public void SetSpeed(float speed)
    {
        float value = speed / maxSpeed;
        if (animator != null) animator.SetFloat("Speed", value);

    }
}
