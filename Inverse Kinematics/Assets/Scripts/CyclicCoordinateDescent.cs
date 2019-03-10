using System.Collections.Generic;
using UnityEngine;

public class CyclicCoordinateDescent : MonoBehaviour
{
    public int MaxAttempts;
    public float Threshold;

    public GameObject Goal;
    public GameObject EndEffector;
    //Include the Base in the list of joints since we use it like one in the cyclic coordinate descent process
    public List<GameObject> Joints;

    private Vector3 _currentGoalPosition;
    private int _numAttempts;

    private void Start()
    {

    }

    private void Update()
    {
        if (Vector3.Distance(EndEffector.transform.position, Goal.transform.position) > Threshold &&  _numAttempts <= MaxAttempts)
        {
            for (int i = Joints.Count - 1; i >= 0; i--)
            {
                var pathToEndEffector = EndEffector.transform.position - Joints[i].transform.position;
                var pathToGoal = Goal.transform.position - Joints[i].transform.position;

                float theta = Mathf.Acos(Vector3.Dot(pathToEndEffector, pathToGoal) / (pathToEndEffector.magnitude * pathToGoal.magnitude));
                theta = SimplifyAngle(theta) * Mathf.Rad2Deg;

                var rotationAxis = Vector3.Cross(pathToEndEffector, pathToGoal) / (pathToEndEffector.magnitude * pathToGoal.magnitude);

                Joints[i].transform.Rotate(rotationAxis, theta, Space.World);
            }
            _numAttempts++;
        }

        /*
         * Doing this based on goal position was the easiest way of handling not moving when the goal
         * went out of the reachable workspace. I could have calculated if the goal were in the 
         * reachable workspace and then just had an early return at the top of this method, but this 
         * method wasn't give me any issues.
         */
        if (Goal.transform.position != _currentGoalPosition)
        {
            _currentGoalPosition = Goal.transform.position;
            _numAttempts = 0;
        }
    }

    /*
     * This method (slightly modified) from:
     * Copyright (c) 2008-2009 Ryan Juckett http://www.ryanjuckett.com/
     */
    private static float SimplifyAngle(float theta)
    {
        theta = theta % (2.0f * Mathf.PI);
        theta += theta < -Mathf.PI ? 2.0f * Mathf.PI : -2.0f * Mathf.PI;
        return theta;
    }
}
