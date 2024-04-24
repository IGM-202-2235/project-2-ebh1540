using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [SerializeField]
    float radius;

    // Start is called before the first frame update
    void Start()
    {
        radius = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 1.25f;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FoodMotivated fish in AgentManager.Instance.foodMotivateds){
            if((fish.transform.position - transform.position).magnitude < (radius + fish.Radius)){
                AgentManager.Instance.eatFood(this, fish);
                break;
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
