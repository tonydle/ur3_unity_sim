/*
Based on work done by Dr. Martin Bischoff (martin.bischoff@siemens.com)
*/
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointTrajectorySubscriber : UnitySubscriber<MessageTypes.Trajectory.JointTrajectory>
    {

        public Robot.RobotController controller;

        private MessageTypes.Trajectory.JointTrajectory mostRecentMessage;
        private Robot.JointTrajectory trajectory = new Robot.JointTrajectory();

        private bool isMessageReceived;

        protected override void Start()
		{
			base.Start();
		}

        private void FixedUpdate()
        {
            if (isMessageReceived)
            {
                ProcessMessage();
                isMessageReceived = false;
            }
        }

        protected override void ReceiveMessage(MessageTypes.Trajectory.JointTrajectory message)
        {
            mostRecentMessage = message;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            trajectory.points.Clear();
            trajectory.jointNames.Clear();
            trajectory.startTime = Time.fixedTime;

            foreach (string joint in mostRecentMessage.joint_names)
            {
                trajectory.jointNames.Add(joint);
            }

            foreach (MessageTypes.Trajectory.JointTrajectoryPoint point in mostRecentMessage.points)
            {
                Robot.JointTrajectoryPoint jointTrajPoint = new Robot.JointTrajectoryPoint();
                for(int i = 0; i < trajectory.jointNames.Count; i++) {
                    jointTrajPoint.positions.Add(System.Convert.ToSingle(point.positions[i]));
                    jointTrajPoint.velocities.Add(System.Convert.ToSingle(point.velocities[i]));
                }
                jointTrajPoint.timeFromStart = ((float)point.time_from_start.nsecs) / 1000.0f;
                trajectory.points.Add(jointTrajPoint);
            }
            controller.FollowTrajectory(trajectory);
        }
    }
}