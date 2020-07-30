/*
Based on work done by Dr. Martin Bischoff (martin.bischoff@siemens.com)
*/
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointJogSubscriber : UnitySubscriber<MessageTypes.Sensor.JointState>
    {

        public Robot.RobotController controller;

        private MessageTypes.Sensor.JointState mostRecentMessage;
        private Robot.JointJog jointJogCmd = new Robot.JointJog();

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

        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            mostRecentMessage = message;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            jointJogCmd.joint_names.Clear();
            jointJogCmd.velocities.Clear();
            foreach (string joint in mostRecentMessage.name)
            {
                jointJogCmd.joint_names.Add(joint);
            }
            for(int i = 0; i < jointJogCmd.joint_names.Count; i++) {
                jointJogCmd.velocities.Add(System.Convert.ToSingle(mostRecentMessage.velocity[i]));
            }
            controller.jogRobot(jointJogCmd);
        }
    }
}

