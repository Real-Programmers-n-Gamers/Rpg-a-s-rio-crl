using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyController : MonoBehaviour
{
    public float speed;
    private Vector2 moveVector;
    private Rigidbody2D rb2d;
    private Vector2 target;
    [Tooltip("Time before it changes position to move to.")]
    public float targetTime;
    [Tooltip("Range of random movement. minRange<Current cell<maxRange")]
    public float movementRange;
    private Vector2 initialPosition;

    public GameObject rangeIndicator;
    public float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.drag = 5;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2d.freezeRotation = true;
        initialPosition = transform.position;

        rangeIndicator.transform.localScale = new Vector3(attackRange * 10, attackRange * 10,1);

        InvokeRepeating("SetTarget",0,targetTime);
    }

    private void SetTarget() 
    {
        target = new Vector2(initialPosition.x + Random.Range(-movementRange, movementRange), 
            initialPosition.y +  Random.Range(-movementRange, movementRange));

        moveVector = -((Vector2)transform.position- target).normalized * Time.deltaTime * speed;

        Debug.Log(transform.position + "---->" + target + "    set. move--->"+moveVector);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > target.x-0.5f&&transform.position.x<target.x+0.5f&&
            transform.position.y>target.y-0.5f&&transform.position.y<target.y+0.5f) 
            moveVector = Vector2.zero;
        if (moveVector.magnitude != 0) rb2d.AddForce(moveVector);
        rb2d.velocity.Normalize();
    }

    public Vector2 getVelocityVector()
    {
        return rb2d.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveVector = Vector2.zero;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        SetTarget();
    }
}
