using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointRevolute : RobotJoint
    {
        public float InitialPosition;
        private RosSharp.HingeJointLimitsManager _hinge_joint_limits;
        private HingeJoint _hinge_joint;

        void Start()
        {
            _continuous = false;
            _joint_type = "revolute";
            _hinge_joint_limits = GetComponent(typeof(RosSharp.HingeJointLimitsManager)) as RosSharp.HingeJointLimitsManager;
            _hinge_joint = GetComponent(typeof(HingeJoint)) as HingeJoint;

            // _hinge_joint_limits.LargeAngleLimitMin += offset;
            // _hinge_joint_limits.LargeAngleLimitMax += offset;

            setLimits(_hinge_joint_limits.LargeAngleLimitMin, _hinge_joint_limits.LargeAngleLimitMax);
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