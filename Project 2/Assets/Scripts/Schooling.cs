using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schooling : Agent
{
    [SerializeField]
    float boundsWeight = 1f, obstacleWeight = 1f, foodWeight = 0.75f, cohereWeight = 2f, separateWeight = 0.4f, wanderWeight = 1f, wanderRadius = 1f;

    protected override Vector3 CalculateSteeringForces(){
        Vector3 totalForce = Vector3.zero;

        Vector3 boundsForce = StayInBounds();
        totalForce += boundsForce * boundsWeight;
        Vector3 obstacleForce = AvoidObstacles();
        totalForce += obstacleForce * obstacleWeight;
        Vector3 foodForce = SeekClosestFood();
        totalForce += foodForce * foodWeight;
        Vector3 cohereForce = Cohere();
        totalForce += cohereForce * cohereWeight;
        Vector3 separateForce = SeparateSchooling();
        totalForce += separateForce * separateWeight;
        Vector3 wanderForce = Wander(futureTime, wanderRadius);
        totalForce += wanderForce * wanderWeight;

        return totalForce;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.magenta;

        Vector3 futurePosition = GetFuturePosition(futureTime);
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
        Vector3 futurePos = GetFuturePosition(futureTime);

        float length = Vector3.Distance(transform.position, futurePos) + physicsObject.radius;


        Vector3 boxSize = new Vector3(physicsObject.radius * 2f, length, 1f);
        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += length / 2f;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
