using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyController : MonoBehaviour
{
    public Sprite sprite { get; private set; }
    public enum State 
    {
        OutOfCombat,
        Combat
    }
    public State state;

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
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        state = State.OutOfCombat;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.drag = 5;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2d.freezeRotation = true;
        initialPosition = transform.position;

        rangeIndicator.transform.localScale = new Vector3(attackRange * 7, attackRange * 7,1);
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating("SetTarget",0,targetTime);
        InvokeRepeating("CheckTargetting", 0, 0.1f);
    }

    private void SetTarget() 
    {
        target = new Vector2(initialPosition.x + Random.Range(-movementRange, movementRange), 
            initialPosition.y +  Random.Range(-movementRange, movementRange));

        moveVector = -((Vector2)transform.position- target).normalized * Time.deltaTime * speed;
    }

    private void CheckTargetting() 
    {
        if (rangeIndicator.GetComponent<Trigger>().playerTriggered) 
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (player.transform.position - transform.position),attackRange+0.5f,1);
            if (ray.collider != null && ray.collider.tag == "Player") 
            {
                state = State.Combat;
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                ray.collider.GetComponent<Controller>().StartCombat(getNeighbours(),gameObject);
                rangeIndicator.SetActive(false);
                for (int i = 0; i < getNeighbours().Length; i++)
                {
                    if (getNeighbours()[i].GetComponent<EnemyController>())
                    {
                        getNeighbours()[i].GetComponent<EnemyController>().setState(State.Combat);
                        getNeighbours()[i].GetComponent<EnemyController>().rangeIndicator.SetActive(false);
                    }
                }
                target = transform.position;
                moveVector = Vector2.zero;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.OutOfCombat:
                {
                    if (!rangeIndicator.activeSelf) rangeIndicator.SetActive(true);
                    if (transform.position.x > target.x - 0.5f && transform.position.x < target.x + 0.5f &&
                        transform.position.y > target.y - 0.5f && transform.position.y < target.y + 0.5f)
                        moveVector = Vector2.zero;
                    if (moveVector.magnitude != 0) rb2d.AddForce(moveVector);
                    rb2d.velocity.Normalize();
                    break;
                }

            case State.Combat: 
                {
                    break;
                }
        }
    }

    public Vector2 getVelocityVector()
    {
        return rb2d.velocity;
    }

    public GameObject[] getNeighbours() 
    {
        Collider2D[] neighbourColliders = Physics2D.OverlapCircleAll(transform.position,attackRange);
        List<GameObject> neighbour = new List<GameObject>();

        for (int i = 0; i < neighbourColliders.Length; i++) 
        {
            if (neighbourColliders[i].gameObject.tag == "AttackIndicator")
            {
                neighbour.Add(neighbourColliders[i].transform.parent.gameObject);
            }
        }

        return neighbour.ToArray();
    }

    public void setState(State s) 
    {
        state = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveVector = Vector2.zero;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(state!=State.Combat) SetTarget();
    }

    public void SetSprite(Sprite _sprite) 
    {
        sprite = _sprite;
    }
}
