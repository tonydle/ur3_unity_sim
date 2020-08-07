/*
Based on work by Dr. Martin Bischoff (martin.bischoff@siemens.com)
Takes the joint offset into account, to fix a known bug with Unity Joints' springs
*/

using RosSharp.Urdf;
using UnityEngine;
using Joint = UnityEngine.Joint;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(Joint)), RequireComponent(typeof(UrdfJoint))]
    public class ModifiedJointStateReader : MonoBehaviour
    {
        private UrdfJoint urdfJoint;
        private Robot.RobotJoint _robotJoint;

        private void Start()
        {
            urdfJoint = GetComponent<UrdfJoint>();
            _robotJoint = GetComponent(typeof(Robot.RobotJoint)) as Robot.RobotJoint;
        }

        public void Read(out string name, out float position, out float velocity, out float effort)
        {
            name = urdfJoint.JointName;
            if(_robotJoint.GetJointType() == "prismatic") {
                position = urdfJoint.GetPosition() + _robotJoint.offset;
            }
            else {
                position = urdfJoint.GetPosition();
                float robotJointPosition = -(_robotJoint.GetPosition()) * Mathf.Deg2Rad; 
                if (position - robotJointPosition > Mathf.PI)
                {
                    while (position - robotJointPosition > Mathf.PI)
                        position -= 2*Mathf.PI;
                }
                else if (position - robotJointPosition < -Mathf.PI)
                {
                    while (position - robotJointPosition < -Mathf.PI)
                        position += 2*Mathf.PI;
                }
                position += _robotJoint.offset*Mathf.Deg2Rad;
            }
            
            velocity = urdfJoint.GetVelocity();
            effort = urdfJoint.GetEffort();
        }
    }
}