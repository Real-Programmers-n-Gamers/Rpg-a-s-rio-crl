using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp= Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, speed * Time.deltaTime);
        temp.z = transform.position.z;
        transform.position = temp;
    }
}
