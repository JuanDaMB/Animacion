using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public GameObject reference;
    public LayerMask Mask;

    public NavMeshAgent Agent;

    private Vector3 _mouseCoords, _agentTarget;
    private bool _moving;
    private float _speed;
    public float MaxSpeed;
    public PlayerAnimationController animationController;

    void Update()
    {
        _mouseCoords = MousePosition();
        reference.transform.position = _mouseCoords;
        _speed = Agent.velocity.magnitude / MaxSpeed;
        _moving = _speed > 0;
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            animationController.SetDead();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animationController.SetDamage();
        }
        
        if (animationController.Alive())
        { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animationController.SetJump();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                animationController.SetAttack();
            }
        }
        
        if (animationController.OnAnimation())
        {
            Agent.speed = 0f;
            _agentTarget = Agent.transform.position;
            Agent.destination = _agentTarget;
            animationController.ControlMovement(0f);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_moving)
            {
                if (Vector3.Distance(_mouseCoords, _agentTarget) < 0.5f)
                {
                    Agent.speed = MaxSpeed;
                }
                else
                {
                    Walk();
                }
            }
            else
            {
                Walk();
            }
        }
        
        animationController.ControlMovement(_speed);
    }

    private void Walk()
    {
        Agent.speed = MaxSpeed/5f;
        _agentTarget = _mouseCoords;
        Agent.destination = _mouseCoords;
    }

    public Vector3 MousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, Mask))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("hit");
            return hit.point;
        }
        return Vector3.zero;
    }
}
