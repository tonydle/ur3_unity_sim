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
        private JointTrajectory _traj;
        private JointTrajectoryPoint _next_point;
        private int _traj_index = 0;
        private bool _finished_traj = true;
        private bool _robot_disabled = false;
        private bool _going_to_point = false;

        public float PositionKp = 0.0f, PositionKi = 0.0f, PositionKd = 0.0f;

        void Start()
        {
            _joints = GetComponentsInChildren<RobotJoint>();
            foreach (RobotJoint joint in _joints) {
                joint.tunePosPID(PositionKp,PositionKi,PositionKd);
            }
        }

        void FixedUpdate()
        {
            if(!_robot_disabled) {
                if(!_finished_traj) {
                    if(_traj.start_time + _next_point.time_from_start <= Time.fixedTime) {
                        // while loop goes to the next point in trajectory, skipping points according to their time_from_start value
                        while(_traj.start_time + _next_point.time_from_start <= Time.fixedTime) {
                            _traj_index++;
                            if(_traj_index == _traj.points.Count) {
                                _finished_traj = true;
                                float time_complete = Time.fixedTime - _traj.start_time;
                                Debug.Log("Completed trajectory in: " + time_complete.ToString("F3") + " seconds");
                                break;
                            }
                            _next_point = _traj.points[_traj_index];
                        }
                        _going_to_point = false;
                    }
                    if(!_going_to_point) {
                        followNextPoint();
                        _going_to_point = true;
                    }
                }
            }
        }

        public void stopRobot() {
            foreach (RobotJoint joint in _joints) {
                joint.setSpeed(0.0f);
                _robot_disabled = true;
            }
            Debug.Log("Robot DISABLED");
        }

        public void startRobot() {
            foreach (RobotJoint joint in _joints) {
                _robot_disabled = false; 
            }
            Debug.Log("Robot RE-ENABLED");
        }

        private void followNextPoint() {
            foreach (RobotJoint joint in _joints) {
                int joint_index = _traj.joint_names.LastIndexOf(joint.name);
                if (joint.getJointType() == "prismatic")
                {
                    joint.setDestination(_next_point.positions[joint_index],_next_point.velocities[joint_index]);
                }
                else
                {
                    joint.setDestination(-Mathf.Rad2Deg*_next_point.positions[joint_index], -Mathf.Rad2Deg*_next_point.velocities[joint_index]);
                }
            }
        }

        // sets the current joint trajectory to follow
        public bool followTrajectory(JointTrajectory traj) {
            if(traj.points.Count == 0) {
                return false;
            }
            _traj_index = 0;
            _traj = traj;
            _next_point = _traj.points[_traj_index];
            _going_to_point = false;
            _finished_traj = false;
            return true;
        }

        public void jogRobot(JointJog jointJogCmd) {
            _finished_traj = true;
            foreach (RobotJoint joint in _joints) {
                int joint_index = jointJogCmd.joint_names.LastIndexOf(joint.name);
                if (joint.getJointType() == "prismatic")
                {
                    joint.setSpeed(jointJogCmd.velocities[joint_index]);
                }
                else
                {
                    joint.setSpeed(-Mathf.Rad2Deg*jointJogCmd.velocities[joint_index]);
                }
            }
        }

        // check if most recent trajectory has been completed
        public bool finishedTrajectory() {
            return _finished_traj;
        }

        // check current state in trajectory
        public int currentPointInTrajectory() {
            return _traj_index;
        }

        // tune postions PID
        public bool tunePositionsPID(float kp, float ki, float kd) {
            if(!_finished_traj) {
                return false;
            }
            PositionKp = kp; PositionKi = ki; PositionKd = kd;
            return true;
        }
    }
}