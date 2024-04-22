using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhysicsObject : MonoBehaviour
{

    Vector3 position;

    Vector3 direction;

    Vector3 velocity;

    public Vector3 Velocity { get { return velocity; } }

    public Vector3 Direction { get { return direction; } }

    Vector3 acceleration;

    [SerializeField]
    float mass = 1;

    [SerializeField]
    bool frictionOn;

    [SerializeField]
    float frictionStrength = 1;

    [SerializeField]
    bool gravityOn;

    [SerializeField]
    float gravityStrength = 9.8f;

    public float maxSpeed;

    public float speed; // just so i can see velocity magnitude in the editor

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = Camera.main.ScreenToWorldPoint(Camera.main.transform.position);
        leftBound = size.x;
        rightBound = size.x * -1;
        topBound = size.y * -1;
        bottomBound = size.y;
        position = transform.position;  
        speed = 0; 
        radius = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {

        if(gravityOn){
            ApplyGravity(gravityStrength);
        }
        if(frictionOn){
            ApplyFriction(frictionStrength);
        }

        // Bounce();

        velocity += acceleration * Time.deltaTime;

        direction = velocity.normalized;

        Vector3.ClampMagnitude(velocity, maxSpeed);

        speed = velocity.magnitude;

        position += velocity * Time.deltaTime;

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        acceleration = Vector3.zero;
    }


    public void ApplyForce(Vector3 force){
        acceleration += force / mass;
    }

    void ApplyFriction(float coeff){
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);
    }

    void ApplyGravity(float coeff){
        Vector3 gravity = Vector3.down;
        gravity.Normalize();
        gravity = gravity * coeff * mass; // multiply by mass to ignore it in force application
        ApplyForce(gravity);
    }




    float leftBound;
    float rightBound;
    bool xScreenCheck(){
        return position.x > rightBound || position.x < leftBound;
    }
    public bool xScreenCheck(Vector3 pos){
        return pos.x > rightBound || pos.x < leftBound;
    }

    float bottomBound;
    float topBound;
    bool yScreenCheck(){
        return position.y > topBound || position.y < bottomBound;
    }
    public bool yScreenCheck(Vector3 pos){
        return pos.y > topBound || pos.y < bottomBound;
    }


    void Bounce(){
        if(xScreenCheck()){
            float newX = Math.Clamp(position.x, leftBound, rightBound);
            position = new Vector3(newX, position.y, position.z);
            velocity = new Vector3(velocity.x * -1, velocity.y, velocity.z);
        }
        if(yScreenCheck()){
            float newY = Math.Clamp(position.y, bottomBound, topBound);
            position = new Vector3(position.x, newY, position.z);
            velocity = new Vector3(velocity.x, velocity.y * -1, velocity.z);
        }
    }

    public void newPosition(){
        Vector3 newPosition = new Vector3(Random.Range(leftBound, rightBound), Random.Range(bottomBound, topBound), 0);
        // Debug.Log("new position: " + newPosition);
        position = newPosition;
    }

}
