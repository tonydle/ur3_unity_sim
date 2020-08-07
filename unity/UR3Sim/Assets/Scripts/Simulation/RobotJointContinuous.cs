using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointContinuous : RobotJoint
    {
        private HingeJoint _hingeJoint;

        void Start()
        {
            _continuous = true;
            _jointType = "continuous";
            _hingeJoint = GetComponent(typeof(HingeJoint)) as HingeJoint;
            SetLimits(-180, 180);
        }

        void FixedUpdate()
        {
            UpdatePosition();
            JointSpring spr = _hingeJoint.spring;
            spr.targetPosition = _position;
            _hingeJoint.spring = spr;
        }
    }
}