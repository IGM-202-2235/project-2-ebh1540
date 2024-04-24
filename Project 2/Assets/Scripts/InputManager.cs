using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    const float foodCooldown = 0.25f;
    float cooldownValue;
    // Start is called before the first frame update
    void Start()
    {
        cooldownValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownValue = Math.Clamp(cooldownValue - Time.deltaTime, 0, foodCooldown);
    }

    public void OnFire(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            if(cooldownValue == 0){
                Debug.Log("Fire!");
                AgentManager.Instance.SpawnFood(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                cooldownValue = foodCooldown;
            }
            else{
                Debug.Log("Cooling down!");
            }
        }
    }
}
