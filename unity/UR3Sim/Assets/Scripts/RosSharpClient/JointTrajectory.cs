/*
This the Joint Trajectory class (datatype) that simplifies the JointTrajectoryPoint message
*/
using System.Collections.Generic;

namespace Robot
{ 
    public class JointTrajectory
    {
        public float startTime;
        public List<string> jointNames = new List<string>();
        public List<JointTrajectoryPoint> points = new List<JointTrajectoryPoint>();
    }
}
