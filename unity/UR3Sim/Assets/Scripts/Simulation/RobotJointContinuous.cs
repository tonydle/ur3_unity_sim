using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointContinuous : RobotJoint
    {
        private HingeJoint _hinge_joint;

        void Start()
        {
            _continuous = true;
            _joint_type = "continuous";
            _hinge_joint = GetComponent(typeof(HingeJoint)) as HingeJoint;
            setLimits(-180, 180);
        }

        void FixedUpdate()
        {
            updatePosition();
            JointSpring spr = _hinge_joint.spring;
            spr.targetPosition = _position;
            _hinge_joint.spring = spr;
        }
    }
}