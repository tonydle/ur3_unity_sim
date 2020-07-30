using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJointPrismatic : RobotJoint
    {
        public float InitialPosition;
        private RosSharp.PrismaticJointLimitsManager _pris_joint_limits;
        private ConfigurableJoint _config_joint;

        void Start()
        {
            _continuous = false;
            _joint_type = "prismatic";
            _pris_joint_limits = GetComponent(typeof(RosSharp.PrismaticJointLimitsManager)) as RosSharp.PrismaticJointLimitsManager;
            _config_joint = GetComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;

            // _pris_joint_limits.PositionLimitMin += offset;
            // _pris_joint_limits.PositionLimitMax += offset;

            setLimits(_pris_joint_limits.PositionLimitMin, _pris_joint_limits.PositionLimitMax);
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