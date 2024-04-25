using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Hungry : Agent
{
     [SerializeField]
    float wanderTime = 1f, wanderRadius = 1f;

    [SerializeField]
    float wanderWeight = 1f, boundsWeight = 1f, separateWeight = 1f, obstacleWeight = 1f, foodWeight = 1f;

    [SerializeField]
    bool separate = true;
    
    public float sepForce = 0;
    
    protected override Vector3 CalculateSteeringForces(){
        
        // if they go too long without eating they'll starve (this should take on average 2-3 minutes, so if the user is actively interacting it shouldn't happen at all)
        if(foodWeight >= 15){
            AgentManager.Instance.Starve(this);
        }

        Vector3 totalForce = Vector3.zero;
        Vector3 wanderForce = Wander(wanderTime, wanderRadius);
        totalForce += wanderForce * wanderWeight;
        futureTime = wanderTime;
        Vector3 boundsForce = StayInBounds();
        totalForce += boundsForce * boundsWeight;

        totalForce += AvoidObstacles() * Math.Max(obstacleWeight, foodWeight / 2);
        
        // don't spend time calculating all the distances and vectors if we're not gonna separate
        if(separate){
            Vector3 separateForce = SeparateFoodFixated();
            totalForce += separateForce * separateWeight;
            sepForce = (separateForce * separateWeight).magnitude;
        }

        totalForce += SeekClosestFood() * foodWeight;

        foodWeight += Time.deltaTime * Random.Range(0.05f, 0.15f); // The longer these guys go without eating, the stronger their desire to eat gets
        
        return totalForce;
    }

    public void resetFoodWeight(){
        foodWeight = 0; // When they've eaten, they don't get hungry again for a bit so their food motivation goes away
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.magenta;

        Vector3 futurePosition = GetFuturePosition(wanderTime);
        Gizmos.DrawWireSphere(futurePosition, wanderRadius);

        Gizmos.color = Color.cyan;
        float randAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 wanderTarget = futurePosition;

        wanderTarget.x += Mathf.Cos(randAngle) * wanderRadius;
        wanderTarget.y += Mathf.Sin(randAngle) * wanderRadius;

        Gizmos.DrawLine(transform.position, wanderTarget);


        Gizmos.color = Color.yellow;

        foreach(Vector3 pos in foundObstaclePositions){
            Gizmos.DrawLine(transform.position, pos);
        }

//
        //  Draw safe space box
        //
        Gizmos.color = Color.green;
        Vector3 futurePos = GetFuturePosition(wanderTime);

        float length = Vector3.Distance(transform.position, futurePos) + physicsObject.radius;


        Vector3 boxSize = new Vector3(physicsObject.radius * 2f, length, 1f);
        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += length / 2f;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;

    }
}
