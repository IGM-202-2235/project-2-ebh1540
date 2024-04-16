using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Agent : MonoBehaviour
{

    [SerializeField]
    protected PhysicsObject physicsObject;

    [SerializeField]
    protected float futureTime = 1f;

    protected Vector3 totalForces = Vector3.zero;

    public PhysicsObject PhysicsObject {get {return physicsObject;}}

    float radius;

    public float Radius { get {return radius;}}


    // Start is called before the first frame update
    void Start()
    {
        radius = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        totalForces = Vector3.zero;

        // Vector3 steeringForce = CalculateSteeringForces();

        totalForces += CalculateSteeringForces();

        // Vector3.ClampMagnitude(totalForces, 1);

        physicsObject.ApplyForce(totalForces);


    }

    protected abstract Vector3 CalculateSteeringForces();

    public Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * physicsObject.maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce;
    }

    public Vector3 Seek(Agent target){
        return Seek(target.transform.position);
    }

    public Vector3 Flee(Vector3 targetPos){
        // Calculate desired velocity
        Vector3 desiredVelocity = transform.position - targetPos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * physicsObject.maxSpeed;

        // Calculate seek steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return fleeingForce;
    }

    public Vector3 Flee(Agent target){
        return Flee(target.transform.position);
    }

    public Vector3 Evade(Agent target){
        return Flee(target.GetFuturePosition(5f));
    }

    public Vector3 Wander(float time, float radius){
        Vector3 futurePosition = GetFuturePosition(time);

        float randAngle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 wanderTarget = futurePosition;
        
        wanderTarget.x += Mathf.Cos(randAngle) * radius;
        wanderTarget.y += Mathf.Sin(randAngle) * radius;

        return Seek(wanderTarget);
    }

    public Vector3 GetFuturePosition(float futureTime){
        return transform.position + (physicsObject.Velocity * futureTime);
    }

    public Vector3 StayInBounds(){
        Vector3 steeringForce = Vector3.zero;
        Vector3 futurePosition = GetFuturePosition(futureTime);


        if(physicsObject.xScreenCheck(futurePosition) || physicsObject.yScreenCheck(futurePosition)){
            steeringForce += Seek(Vector3.zero);
        }

        return steeringForce;

    }

    public Vector3 Separate(){
        List<Agent> agents = AgentManager.Instance.agents;
        Vector3 steeringForce = Vector3.zero;

        for(int i = 0; i < agents.Count; i++){
            Agent agent = agents[i];
            if(agent != this){
                Vector3 distance = agent.transform.position - transform.position;
                Vector3 separateForce = Flee(agent.transform.position) * (1 / distance.magnitude);
                steeringForce += separateForce;
                // Debug.Log("Sep force: " + separateForce);
            }
        }
        return steeringForce;
    }



    private void OnDrawGizmos(){
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(transform.position, radius);
    }

}
