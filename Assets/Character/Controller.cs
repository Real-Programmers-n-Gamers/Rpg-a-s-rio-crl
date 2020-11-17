using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Controller : MonoBehaviour
{
    public enum FightState
    {
        OutOfCombat,
        Combat
    }
    private FightState fightState;

    public float speed;
    private Vector2 moveVector;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        fightState = FightState.OutOfCombat;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        rb2d.drag = 5;
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2d.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime,Input.GetAxis("Vertical")*speed*Time.deltaTime) ;
        if (moveVector.magnitude != 0) rb2d.AddForce(moveVector);
    }

    public Vector2 getMoveVector() 
    {
        return moveVector;
    }
}
