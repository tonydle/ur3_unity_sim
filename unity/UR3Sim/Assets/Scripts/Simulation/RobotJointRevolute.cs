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
        private HingeJoint _hingeJoint;

        void Start()
        {
            _continuous = false;
            _jointType = "revolute";
            _hingeJoint = GetComponent(typeof(HingeJoint)) as HingeJoint;
            SetLimits(limitMin, limitMax);
            SetPosition(InitialPosition);
        }

        void FixedUpdate()
        {
            UpdatePosition();
            JointSpring spr = _hingeJoint.spring;
            float targetPos = _position;
            if(targetPos > 180) {
                targetPos = targetPos - 360;
            }
            else if(targetPos < -180) {
                targetPos = targetPos + 360;
            }
            spr.targetPosition = targetPos;
            _hingeJoint.spring = spr;
        }
    }
}