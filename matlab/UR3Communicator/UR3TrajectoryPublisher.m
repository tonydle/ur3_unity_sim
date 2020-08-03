classdef UR3TrajectoryPublisher < handle
    %UR3TRAJECTORYPUBLISHER Packages trajectory into JointTrajectory format
    %and sends to to the UR3 with ROS
    
    properties
        dataType;
        jointTrajTopic;
        jointTrajPub;
        jointTrajMsg;
        jointNames;
        steps;
    end
    
    methods
        function self = UR3TrajectoryPublisher()
            %UR3TRAJECTORYPUBLISHER Construct an instance of this class
            %   Initialises default properties

            self.dataType = 'trajectory_msgs/JointTrajectory';
            self.jointTrajTopic = '/ur3/follow_joint_traj';
            self.jointTrajMsg = rosmessage(self.dataType);
            
            self.jointNames = ["shoulder_link", ...
                               "upper_arm_link",...
                               "forearm_link",  ...
                               "wrist_1_link",  ...
                               "wrist_2_link",  ...
                               "wrist_3_link"];
            self.jointTrajMsg.JointNames = self.jointNames;
            self.jointTrajPub = rospublisher(self.jointTrajTopic,self.dataType);

            self.steps = 0;
        end
        
        function InitPublisher(self, interpolationSteps)
            %INITPUBLISHER Initialises the publishers and messages
            % The number of interpolation steps need to be defined first
            % because creating the JointTrajectory message takes a lot of
            % time (can be done at the start).
            
            self.steps = interpolationSteps;
            for i = 1:self.steps
                jointTrajPoint = rosmessage('trajectory_msgs/JointTrajectoryPoint');
                self.jointTrajMsg.Points = [self.jointTrajMsg.Points; jointTrajPoint];
            end
        end
        
        function success = SendTrajectory(self, qMatrix, velMatrix, deltaT)
            %SENDTRAJECTORY Sends a joint trajectory to the robot
            % Returns false if the number of steps is not similar
            
            [qMatrixSize, ~] = size(qMatrix);
            [velMatrixSize, ~] = size(velMatrix);
            if qMatrixSize ~= self.steps || velMatrixSize ~= self.steps
                disp("Number of steps not matched. Use InitPublisher() again.");
                success = false;
                return;
            end
            
            deltaT_msec = deltaT*(1000);
            for i = 1:self.steps
               self.jointTrajMsg.Points(i,1).Positions = qMatrix(i,:);
               self.jointTrajMsg.Points(i,1).Velocities = velMatrix(i,:);
               self.jointTrajMsg.Points(i,1).TimeFromStart.Nsec = round(deltaT_msec*(i-1));
            end
            
            send(self.jointTrajPub, self.jointTrajMsg); pause(0.1);
            disp("Joint Trajectory SENT to robot");
            success = true; return;
        end 
    end
end

