using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointPrismatic : RobotJoint
    {
        public float InitialPosition;
        public float limitMax;
        public float limitMin;
        private ConfigurableJoint _configJoint;

        void Start()
        {
            _continuous = false;
            _jointType = "prismatic";
            _configJoint = GetComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
            SetLimits(limitMin, limitMax);
            SetPosition(InitialPosition);
        }

        void FixedUpdate()
        {
            UpdatePosition();
            Vector3 targetPos = new Vector3(_position, 0.0f, 0.0f);
            _configJoint.targetPosition = targetPos;
        }
    }
}