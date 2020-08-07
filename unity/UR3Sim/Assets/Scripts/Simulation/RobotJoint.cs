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
        protected float _limitMin = -45;
        protected float _limitMax = 45;
        protected bool _continuous = false;
        protected string _jointType = "revolute";

        // CONTROL STATUS
        protected float _position = 0;
        protected float _speed = 0;
        protected float _desiredPosition = 0;
        protected float _desiredVelocity = 0;
        protected bool _pidControlled = false;

        // POSITION PID TUNING
        protected float _posPIDKp = 0;
        protected float _posPIDKi = 0;
        protected float _posPIDKd = 0;
        protected float _posPIDCurrentError = 0;
        protected float _posPIDSumOfErrors = 0;
        protected float _posPIDPreviousError = 0;

        // WATCHDOG
        protected float lastCommandTime;
        protected float watchDogTimeout = 1.0f; //secs

        public bool SetLimits(float min, float max) {
            _limitMin = min + offset;
            _limitMax = max + offset;
            return true;
        }

        public bool SetSpeed(float speed) {
            _pidControlled = false;
            _speed = speed;
            lastCommandTime = Time.fixedTime;
            return true;
        }

        public string GetJointType() {
            return _jointType;
        }

        public float GetSpeed() {
            return _speed;
        }

        public bool SetDestination(float desiredPosition, float desiredVelocity)
        {
            _desiredPosition = desiredPosition + offset;
            _desiredVelocity = desiredVelocity;
            _posPIDCurrentError = 0;
            _posPIDPreviousError = 0;
            _posPIDSumOfErrors = 0;
            lastCommandTime = Time.fixedTime;
            _pidControlled = true;
            return true;
        }

        public bool SetDesiredPosition(float desiredPosition)
        {
            // do nothing, for now
            return true;
        }

        public float GetPosition()
        {
            return _position;
        }

        public bool SetPosition(float position)
        {
            _position = position;
            return true;
        }

        public bool TunePosPID(float kp, float ki, float kd) {
            _posPIDKp = kp;
            _posPIDKi = ki;
            _posPIDKd = kd;
            return true;
        }

        // Checks whether a PID control has been sent recently
        // This was added to prevent the robot from keep moving if the final joint velocity is > 0
        protected void WatchDogCheck() {
            if(Time.fixedTime - lastCommandTime > watchDogTimeout) {
                _pidControlled = false;
                _speed = 0.0f;
            }
        }

        protected void UpdatePosition() {
            WatchDogCheck();

            float tempPosition;
            if(_pidControlled) {
                _speed = _desiredVelocity;
                tempPosition = _position + _speed*Time.fixedDeltaTime;
                _posPIDCurrentError = _desiredPosition - tempPosition;
                _posPIDSumOfErrors += _posPIDCurrentError;
                float _posPIDErrorDelta = _posPIDCurrentError - _posPIDPreviousError;
                
                // Position PID Calculation
                // Calculate speed again
                _speed += _posPIDKp*_posPIDCurrentError + _posPIDKi*_posPIDSumOfErrors*Time.fixedDeltaTime + _posPIDKd*(_posPIDErrorDelta/Time.fixedDeltaTime);
                _posPIDPreviousError = _posPIDCurrentError;
                tempPosition = _position + _speed*Time.fixedDeltaTime;
            }
            else
            {
                tempPosition = _position + _speed*Time.fixedDeltaTime;
            }
            
            if(tempPosition >= _limitMax) {
                if(_continuous) {
                    tempPosition = _limitMin + (tempPosition - _limitMax);
                }
                else tempPosition = _limitMax;
            }
            else if(tempPosition <= _limitMin) {
                if(_continuous) {
                    tempPosition = _limitMax - (_limitMin - tempPosition);
                }
                else tempPosition = _limitMin;
            }
            _position = tempPosition;
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }
}