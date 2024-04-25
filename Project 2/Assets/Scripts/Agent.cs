using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Agent : MonoBehaviour
{

    AgentManager agentManager;

    [SerializeField]
    protected PhysicsObject physicsObject;

    [SerializeField]
    protected float futureTime = 1f;
    
    protected Vector3 totalForces = Vector3.zero;

    public PhysicsObject PhysicsObject {get {return physicsObject;}}

    float radius;

    public float Radius { get {return radius;}}

    protected List<Vector3> foundObstaclePositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        radius = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
        foundObstaclePositions.Clear();
        agentManager = AgentManager.Instance;
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

    public Vector3 SeparateFoodFixated(){
        List<Hungry> agents = agentManager.hungries;
        Vector3 steeringForce = Vector3.zero;

        for(int i = 0; i < agents.Count; i++){
            Agent agent = agents[i];
            if(agent != this){
                Vector3 distance = agent.transform.position - transform.position;
                Vector3 separateForce = Flee(agent.transform.position) * (1 / (float) Math.Pow(2, distance.magnitude - 2));
                steeringForce += separateForce;
                // Debug.Log("Sep force: " + separateForce);
            }
        }
        return steeringForce;

    }

    public Vector3 SeparateSchooling(){
        List<Schooling> agents = agentManager.schoolings;
        Vector3 steeringForce = Vector3.zero;

        for(int i = 0; i < agents.Count; i++){
            Agent agent = agents[i];
            if(agent != this){
                Vector3 distance = agent.transform.position - transform.position;
                Vector3 separateForce = Flee(agent.transform.position) * (1 / (float) Math.Pow(2, distance.magnitude - 2));
                steeringForce += separateForce;
                // Debug.Log("Sep force: " + separateForce);
            }
        }
        return steeringForce;
    }

    public Vector3 Separate(){
        List<Agent> agents = agentManager.agents;
        Vector3 steeringForce = Vector3.zero;

        for(int i = 0; i < agents.Count; i++){
            Agent agent = agents[i];
            if(agent != this){
                Vector3 distance = agent.transform.position - transform.position;
                Vector3 separateForce = Flee(agent.transform.position) * (1 / (float) Math.Pow(2, distance.magnitude - 2));
                steeringForce += separateForce;
                // Debug.Log("Sep force: " + separateForce);
            }
        }
        return steeringForce;
    }

    public Vector3 AvoidObstacles(){
        Vector3 steeringForce = Vector3.zero;
        foundObstaclePositions.Clear();
        float forwardDot, rightDot;
        Vector3 vToO = Vector3.zero;
        foreach(Obstacle obstacle in agentManager.obstacles){
            vToO = obstacle.transform.position - transform.position;
            forwardDot = Vector3.Dot(physicsObject.Direction, vToO);
            float length = Vector3.Distance(transform.position, GetFuturePosition(futureTime)) + physicsObject.radius;
            if(forwardDot > 0f && forwardDot < length){
                rightDot = Vector3.Dot(vToO, transform.right);
                if(Math.Abs(rightDot) < radius + obstacle.radius){
                    // Debug.Log("Obstacle found: forward dot " + forwardDot + " right dot " + rightDot);
                    foundObstaclePositions.Add(obstacle.transform.position);
                }
            }
        }

        foreach(Vector3 pos in foundObstaclePositions){
            vToO = pos - transform.position;
            rightDot = Vector3.Dot(vToO, transform.right);
            Vector3 desiredVelocity = physicsObject.maxSpeed * (rightDot < 0 ? transform.right : transform.right * -1);
            Vector3 avoidForce = desiredVelocity - physicsObject.Velocity;
            steeringForce += avoidForce / vToO.magnitude;
        }




        return steeringForce;
    }

    public Vector3 SeekFood(){
        Vector3 steeringForce = Vector3.zero;
        foreach(Food food in agentManager.foodList){
            steeringForce += Seek(food.transform.position) / (food.transform.position - transform.position).magnitude;
        }

        return steeringForce;
    }

    public Vector3 SeekClosestFood(){
        Vector3 steeringForce = Vector3.zero;
        if(agentManager.foodList.Count > 0){
            Food toSeek = AgentManager.Instance.foodList[0];
            float minDistance = 1000f; // it should never get this high anyway
            foreach(Food food in AgentManager.Instance.foodList){
                Vector3 fishToFood = food.transform.position - transform.position;
                if(fishToFood.magnitude < minDistance){
                    minDistance = fishToFood.magnitude;
                    toSeek = food;
                }
            }
            steeringForce = Seek(toSeek.transform.position);
        }

        return steeringForce;
    }

    public Vector3 Cohere(){
        return Seek(agentManager.SchoolingPoint);
    }

    public Vector3 FleeSchooling(){
        return Flee(agentManager.SchoolingPoint);
    }


    /*private void OnDrawGizmos(){
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, radius);

        Gizmos.color = Color.yellow;

        foreach(Vector3 pos in foundObstaclePositions){
            Gizmos.DrawLine(transform.position, pos);
        }

    
        Vector3 boxSize = Vector3.one;
        Vector3 boxCenter = Vector3.zero;
        



        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }*/

}
