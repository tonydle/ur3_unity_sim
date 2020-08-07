/*
Based on work done by Dr. Martin Bischoff (martin.bischoff@siemens.com)
*/
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class EStopSubscriber : UnitySubscriber<MessageTypes.Std.Bool>
    {
        public Robot.RobotController controller;
        private MessageTypes.Std.Bool mostRecentMessage;
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

        protected override void ReceiveMessage(MessageTypes.Std.Bool message)
        {
            mostRecentMessage = message;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            if(mostRecentMessage.data == true) {
                controller.StopRobot();
            }
            else {
                controller.StartRobot();
            }
        }
    }
}