using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private Controller controller;
    private float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        waitTimer = 2f;
        animator = GetComponent<Animator>();
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("x", controller.getMoveVector().normalized.x);
        animator.SetFloat("y", controller.getMoveVector().normalized.y);

        if (controller.getMoveVector().normalized.x == 0 && controller.getMoveVector().normalized.y == 0)
        {
            if (Input.GetButtonDown("Fire1")) { animator.SetTrigger("Surprised"); waitTimer = 5f; }
            if (Input.GetButtonDown("Fire2")) { animator.SetTrigger("Yes"); waitTimer = 5f; }
            if (Input.GetButtonDown("Fire3")) { animator.SetTrigger("No"); waitTimer = 5f; }
        }

        if ((waitTimer > 0 && controller.getMoveVector().magnitude == 0))
        {
            animator.SetBool("Iddle", true);
            waitTimer -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("Iddle", false);
            waitTimer = 5f;
        }

        if (waitTimer <= 0) animator.SetTrigger("IddleAnim");
    }
}