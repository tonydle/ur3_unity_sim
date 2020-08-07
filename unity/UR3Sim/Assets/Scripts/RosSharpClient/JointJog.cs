/*
This the Joint Trajectory class (datatype) that simplifies the JointJog message
*/
using System.Collections.Generic;

namespace Robot
{ 
    public class JointJog
    {
        public List<string> jointNames = new List<string>();
        public List<float> velocities = new List<float>();
    }
}