using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{
     [SerializeField]
    float wanderTime = 1f, wanderRadius = 1f;

    [SerializeField]
    float wanderWeight = 1f, boundsWeight = 1f, separateWeight = 1f;

    [SerializeField]
    bool separate = true;
    
    public float sepForce = 0;

    
    protected override Vector3 CalculateSteeringForces(){
        Vector3 totalForce = Vector3.zero;
        Vector3 wanderForce = Wander(wanderTime, wanderRadius);
        totalForce += wanderForce * wanderWeight;
        futureTime = wanderTime;
        Vector3 boundsForce = StayInBounds();
        totalForce += boundsForce * boundsWeight;
        
        // don't spend time calculating all the distances and vectors if we're not gonna separate
        if(separate){
            Vector3 separateForce = Separate();
            totalForce += separateForce * separateWeight;
            sepForce = (separateForce * separateWeight).magnitude;
        }
        return totalForce;
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
    }
}
