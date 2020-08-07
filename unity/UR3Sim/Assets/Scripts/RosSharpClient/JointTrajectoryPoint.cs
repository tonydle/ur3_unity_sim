/*
This the Joint Trajectory Point class (datatype) that simplifies the JointTrajectoryPoint message
*/
using System.Collections.Generic;

namespace Robot
{
    public class JointTrajectoryPoint
    {
        public List<float> positions = new List<float>();
        public List<float> velocities = new List<float>();
        public float timeFromStart;  // seconds
    }
}