using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointRevolute : RobotJoint
    {
        public float InitialPosition;
        public float limitMax;
        public float limitMin;
        private RosSharp.HingeJointLimitsManager _hinge_joint_limits;
        private HingeJoint _hinge_joint;

        void Start()
        {
            _continuous = false;
            _joint_type = "revolute";
            _hinge_joint = GetComponent(typeof(HingeJoint)) as HingeJoint;
            setLimits(limitMin, limitMax);
            setPosition(InitialPosition);
        }

        void FixedUpdate()
        {
            updatePosition();
            JointSpring spr = _hinge_joint.spring;
            float target_pos = _position;
            if(target_pos > 180) {
                target_pos = target_pos - 360;
            }
            else if(target_pos < -180) {
                target_pos = target_pos + 360;
            }
            spr.targetPosition = target_pos;
            _hinge_joint.spring = spr;
        }
    }
}