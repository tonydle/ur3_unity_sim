/*
Author: Tony Le (tony@mechatony.com)

Base class for other joint types (Revolute, Prismatic, Continuous)
Keeps track of position, speed, limits, and their relationships. Derived classes (joints) use this position
to control their respective Unity Joints.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robot
{
    public class RobotJoint : MonoBehaviour
    {
        // FIXED
        public float offset = 0.0f;
        protected float _limit_min = -45;
        protected float _limit_max = 45;
        protected bool _continuous = false;
        protected string _joint_type = "revolute";

        // CONTROL STATUS
        protected float _position = 0;
        protected float _speed = 0;
        protected float _desired_position = 0;
        protected float _desired_velocity = 0;
        protected bool _pid_control = false;

        // POSITION PID TUNING
        protected float _pos_pid_kp = 0;
        protected float _pos_pid_ki = 0;
        protected float _pos_pid_kd = 0;
        protected float _pos_pid_cur_err = 0;
        protected float _pos_pid_sum_err = 0;
        protected float _pos_pid_pre_err = 0;

        // WATCHDOG
        protected float last_command_time;
        protected float watch_dog_timeout = 1.0f; //secs

        public bool setLimits(float min, float max) {
            _limit_min = min + offset;
            _limit_max = max + offset;
            return true;
        }

        public bool setSpeed(float speed) {
            _pid_control = false;
            _speed = speed;
            last_command_time = Time.fixedTime;
            return true;
        }

        public string getJointType() {
            return _joint_type;
        }

        public float getSpeed() {
            return _speed;
        }

        public bool setDestination(float desired_position, float desired_velocity)
        {
            _desired_position = desired_position + offset;
            _desired_velocity = desired_velocity;
            _pos_pid_cur_err = 0;
            _pos_pid_pre_err = 0;
            _pos_pid_sum_err = 0;
            last_command_time = Time.fixedTime;
            _pid_control = true;
            return true;
        }

        public bool setDesiredPosition(float desired_position)
        {
            // do nothing, for now
            return true;
        }

        public float getPosition()
        {
            return _position;
        }

        public bool setPosition(float position)
        {
            _position = position;
            return true;
        }

        public bool tunePosPID(float kp, float ki, float kd) {
            _pos_pid_kp = kp;
            _pos_pid_ki = ki;
            _pos_pid_kd = kd;
            return true;
        }

        // Checks whether a PID control has been sent recently
        // This is added to prevent the robot from keep moving if the final joint velocity is > 0
        protected void watchDogCheck() {
            if(Time.fixedTime - last_command_time > watch_dog_timeout) {
                _pid_control = false;
                _speed = 0.0f;
            }
        }

        protected void updatePosition() {
            watchDogCheck();

            float temp_position;
            if(_pid_control) {
                _speed = _desired_velocity;
                temp_position = _position + _speed*Time.fixedDeltaTime;
                _pos_pid_cur_err = _desired_position - temp_position;
                _pos_pid_sum_err += _pos_pid_cur_err;
                float _pos_pid_dif_err = _pos_pid_cur_err - _pos_pid_pre_err;
                
                // Position PID Calculation
                // Calculate speed again
                _speed += _pos_pid_kp*_pos_pid_cur_err + _pos_pid_ki*_pos_pid_sum_err*Time.fixedDeltaTime + _pos_pid_kd*(_pos_pid_dif_err/Time.fixedDeltaTime);
                _pos_pid_pre_err = _pos_pid_cur_err;
                temp_position = _position + _speed*Time.fixedDeltaTime;
            }
            else
            {
                temp_position = _position + _speed*Time.fixedDeltaTime;
            }
            
            if(temp_position >= _limit_max) {
                if(_continuous) {
                    temp_position = _limit_min + (temp_position - _limit_max);
                }
                else temp_position = _limit_max;
            }
            else if(temp_position <= _limit_min) {
                if(_continuous) {
                    temp_position = _limit_max - (_limit_min - temp_position);
                }
                else temp_position = _limit_min;
            }
            _position = temp_position;
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }
}