using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Controller : MonoBehaviour
{
    public Sprite sprite { get; private set; }
    public enum FightState
    {
        OutOfCombat,
        Combat
    }
    private FightState fightState;

    public float speed;
    private Vector2 moveVector;
    private Rigidbody2D rb2d;
    public bool canMove;

    //combat
    private CombatManager combatManager;

    // Start is called before the first frame update
    void Start()
    {
        fightState = FightState.OutOfCombat;
        canMove=true;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.drag = 5;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2d.freezeRotation = true;

        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (fightState)
        {
            case FightState.Combat:
                {
                    Move();
                    break;
                }
            case FightState.OutOfCombat:
                {
                    Move();
                    break;
                }
        }
    }

    private void Move()
    {
        if (canMove)
        {
            moveVector = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime);
            if (moveVector.magnitude != 0) rb2d.AddForce(moveVector);
        }
        else
        {
            rb2d.velocity = Vector3.zero;
            moveVector = Vector2.zero;
        }
    }

    public Vector2 getMoveVector() 
    {
        return moveVector;
    }

    public void StartCombat(GameObject[] enemies,GameObject triggeredEnemy) 
    {
        fightState = FightState.Combat;
        moveVector = Vector3.zero;
        rb2d.velocity = Vector3.zero;
        //canMove = false;
        GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>().StartCombat(enemies,triggeredEnemy);
    }

    public void setSprite(Sprite _sprite) 
    {
        sprite = _sprite;
    }
}
