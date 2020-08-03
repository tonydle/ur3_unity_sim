classdef UR3JointVelocityJogger < handle
    %UR3JOINTVELOCITYJOGGER Sends joint velocities jogging commands to the
    %UR3 with ROS
    
    properties
        jointJogTopic;
        jointJogPub;
        jointJogMsg;
        jointNames;
    end
    
    methods
        function self = UR3JointVelocityJogger()
            %UR3JOINTVELOCITYJOGGER Construct an instance of this class
            %   Initialises default properties
            
            self.jointJogTopic = '/ur3/joint_jog';
            self.jointJogMsg = rosmessage('sensor_msgs/JointState');
            self.jointNames = ["shoulder_link", ...
                               "upper_arm_link",...
                               "forearm_link",  ...
                               "wrist_1_link",  ...
                               "wrist_2_link",  ...
                               "wrist_3_link"];
            self.jointJogMsg.Name = self.jointNames;
            self.jointJogPub = rospublisher(self.jointJogTopic,'sensor_msgs/JointState');
        end
        
        function jog(self, velocities)
            %JOGROBOT Jogs the robot's joint velocities
            self.jointJogMsg.Velocity = velocities;
            send(self.jointJogPub, self.jointJogMsg);
        end
    end
end