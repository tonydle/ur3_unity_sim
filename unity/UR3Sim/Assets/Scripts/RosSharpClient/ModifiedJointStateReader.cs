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
        private Robot.RobotJoint _robot_joint;

        private void Start()
        {
            urdfJoint = GetComponent<UrdfJoint>();
            _robot_joint = GetComponent(typeof(Robot.RobotJoint)) as Robot.RobotJoint;
        }

        public void Read(out string name, out float position, out float velocity, out float effort)
        {
            name = urdfJoint.JointName;
            if(_robot_joint.getJointType() == "prismatic") {
                position = urdfJoint.GetPosition() + _robot_joint.offset;
            }
            else {
                position = urdfJoint.GetPosition() + _robot_joint.offset*Mathf.Deg2Rad;
            }
            
            velocity = urdfJoint.GetVelocity();
            effort = urdfJoint.GetEffort();
        }
    }
}
