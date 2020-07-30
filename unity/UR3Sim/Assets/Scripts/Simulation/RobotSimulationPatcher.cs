using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSimulationPatcher : MonoBehaviour
{
    public int positionIterations = 200;
    public int velocityIterations = 40;
    void Start()
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.solverIterations = positionIterations;
            rb.solverVelocityIterations = velocityIterations;
        }
    }

    void Update()
    {
        
    }
}