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
        private RosSharp.PrismaticJointLimitsManager _pris_joint_limits;
        private ConfigurableJoint _config_joint;

        void Start()
        {
            _continuous = false;
            _joint_type = "prismatic";
            _config_joint = GetComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
            setLimits(limitMin, limitMax);
            setPosition(InitialPosition);
        }

        void FixedUpdate()
        {
            updatePosition();
            Vector3 target_pos = new Vector3(_position, 0.0f, 0.0f);
            _config_joint.targetPosition = target_pos;
        }
    }
}