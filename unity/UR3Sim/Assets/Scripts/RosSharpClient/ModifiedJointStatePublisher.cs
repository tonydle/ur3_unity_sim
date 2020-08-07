/*
Based on work by Dr. Martin Bischoff (martin.bischoff@siemens.com)
*/

using System.Collections.Generic;

namespace RosSharp.RosBridgeClient
{
    public class ModifiedJointStatePublisher : UnityPublisher<MessageTypes.Sensor.JointState>
    {
        public List<ModifiedJointStateReader> JointStateReaders;
        public string FrameId = "Unity";

        private MessageTypes.Sensor.JointState message;
        private RosConnector RosConnector;
        
        protected override void Start()
        {
            RosConnector = GetComponent(typeof(RosConnector)) as RosConnector;
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            if (JointStateReaders != null) {
                try {
                    UpdateMessage();
                } catch(System.NullReferenceException){}
            }
        }

        private void InitializeMessage()
        {
            int jointStateLength = JointStateReaders.Count;
            message = new MessageTypes.Sensor.JointState
            {
                header = new MessageTypes.Std.Header { frame_id = FrameId },
                name = new string[jointStateLength],
                position = new double[jointStateLength],
                velocity = new double[jointStateLength],
                effort = new double[jointStateLength]
            };
        }

        private void UpdateMessage()
        {
            message.header.Update();
            for (int i = 0; i < JointStateReaders.Count; i++)
                UpdateJointState(i);
            if(RosConnector.IsConnected.WaitOne(0))
            {
                Publish(message);
            }
        }

        private void UpdateJointState(int i)
        {
            JointStateReaders[i].Read(
                out message.name[i],
                out float position,
                out float velocity,
                out float effort);

            message.position[i] = position;
            message.velocity[i] = velocity;
            message.effort[i] = effort;
        }
    }
}