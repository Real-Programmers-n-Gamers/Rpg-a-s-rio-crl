using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EnemyAnimationController : MonoBehaviour
{
    [Tooltip("Enemy object that possesses the controller.")]
    public GameObject enemyControllerObject;
    private Animator animator;
    private EnemyController controller;
    private float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        waitTimer = 5f;
        animator = GetComponent<Animator>();
        controller = enemyControllerObject.GetComponent<EnemyController>();
        controller.SetSprite(GetComponent<SpriteRenderer>().sprite);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("x", controller.getVelocityVector().normalized.x);
        animator.SetFloat("y", controller.getVelocityVector().normalized.y);

        if ((waitTimer > 0 && controller.getVelocityVector().magnitude == 0))
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
