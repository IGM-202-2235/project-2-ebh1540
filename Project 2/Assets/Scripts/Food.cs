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
        foreach(Agent fish in AgentManager.Instance.foodMotivateds){
            if((fish.transform.position - transform.position).magnitude < (radius + fish.Radius)){
                // hungry fish won't eat if they're full, allowing other hungry fish to swoop in and collect food if there's two pieces very close together
                if(fish.GetType() == typeof(Hungry) && ((Hungry) fish).Hunger > 1){ 
                    AgentManager.Instance.EatFood(this, (Hungry) fish);
                }
                else{
                    AgentManager.Instance.EatFood(this, (Schooling) fish);
                }
                break;
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
