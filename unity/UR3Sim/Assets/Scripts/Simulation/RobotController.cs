/*
Author: Tony Le (tony@mechatony.com)

Controls a robot built with RobotJoints, making it follow a Joint Trajectory, with steps
skipping designed in mind to compensate for lags and latency caused by Unity / low framerate
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotController : MonoBehaviour
    {
        private RobotJoint[] _joints;
        private JointTrajectory _trajectory;
        private JointTrajectoryPoint _nextPoint;
        private int _pointIndex = 0;
        private bool _finshedTrajectory = true;
        private bool _robotDisabled = false;
        private bool _goingToPoint = false;

        public float PositionKp = 0.0f, PositionKi = 0.0f, PositionKd = 0.0f;

        void Start()
        {
            _joints = GetComponentsInChildren<RobotJoint>();
            foreach (RobotJoint joint in _joints) {
                joint.TunePosPID(PositionKp,PositionKi,PositionKd);
            }
        }

        void FixedUpdate()
        {
            if(!_robotDisabled) {
                if(!_finshedTrajectory) {
                    if(_trajectory.startTime + _nextPoint.timeFromStart <= Time.fixedTime) {
                        // while loop goes to the next point in trajectory, skipping points according to their timeFromStart value
                        while(_trajectory.startTime + _nextPoint.timeFromStart <= Time.fixedTime) {
                            _pointIndex++;
                            if(_pointIndex == _trajectory.points.Count) {
                                _finshedTrajectory = true;
                                float timeComplete = Time.fixedTime - _trajectory.startTime;
                                Debug.Log("Completed trajectory in: " + timeComplete.ToString("F3") + " seconds");
                                break;
                            }
                            _nextPoint = _trajectory.points[_pointIndex];
                        }
                        _goingToPoint = false;
                    }
                    if(!_goingToPoint) {
                        FollowNextPoint();
                        _goingToPoint = true;
                    }
                }
            }
        }

        public void StopRobot() {
            foreach (RobotJoint joint in _joints) {
                joint.SetSpeed(0.0f);
                _robotDisabled = true;
            }
            Debug.Log("Robot DISABLED");
        }

        public void StartRobot() {
            foreach (RobotJoint joint in _joints) {
                _robotDisabled = false; 
            }
            Debug.Log("Robot RE-ENABLED");
        }

        private void FollowNextPoint() {
            foreach (RobotJoint joint in _joints) {
                int joint_index = _trajectory.jointNames.LastIndexOf(joint.name);
                if (joint.GetJointType() == "prismatic")
                {
                    joint.SetDestination(_nextPoint.positions[joint_index],_nextPoint.velocities[joint_index]);
                }
                else
                {
                    joint.SetDestination(-Mathf.Rad2Deg*_nextPoint.positions[joint_index], -Mathf.Rad2Deg*_nextPoint.velocities[joint_index]);
                }
            }
        }

        // sets the current joint trajectory to follow
        public bool FollowTrajectory(JointTrajectory traj) {
            if(traj.points.Count == 0) {
                return false;
            }
            _pointIndex = 0;
            _trajectory = traj;
            _nextPoint = _trajectory.points[_pointIndex];
            _goingToPoint = false;
            _finshedTrajectory = false;
            return true;
        }

        public void JogRobot(JointJog jointJogCmd) {
            _finshedTrajectory = true;
            foreach (RobotJoint joint in _joints) {
                int joint_index = jointJogCmd.jointNames.LastIndexOf(joint.name);
                if (joint.GetJointType() == "prismatic")
                {
                    joint.SetSpeed(jointJogCmd.velocities[joint_index]);
                }
                else
                {
                    joint.SetSpeed(-Mathf.Rad2Deg*jointJogCmd.velocities[joint_index]);
                }
            }
        }

        // check if most recent trajectory has been completed
        public bool FinishedTrajectory() {
            return _finshedTrajectory;
        }

        // check current state in trajectory
        public int CurrentPointInTrajectory() {
            return _pointIndex;
        }

        // tune postions PID
        public bool TunePositionsPID(float kp, float ki, float kd) {
            if(!_finshedTrajectory) {
                return false;
            }
            PositionKp = kp; PositionKi = ki; PositionKd = kd;
            return true;
        }
    }
}