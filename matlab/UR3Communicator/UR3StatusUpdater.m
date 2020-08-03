classdef UR3StatusUpdater < handle
    %UR3STATUSUPDATER Updates the UR3's current joint angles and base
    %transform
    
    properties (Access = private)
        jointStateTopic;
        jointStateSub;
        jointStates;
        
        baseTransformTopic;
        baseTransformSub;
        baseTransform;
    end
    
    methods
        function self = UR3StatusUpdater()
            %UR3STATUSUPDATER Construct an instance of this class
            %   Initialises default properties
            self.jointStates = zeros(1,6);
            self.baseTransform = transl(0,0,0);
            
            self.jointStateTopic = '/ur3/joint_states';
            self.jointStateSub = rossubscriber(self.jointStateTopic, 'sensor_msgs/JointState');

            self.baseTransformTopic = '/ur3/base';
            self.baseTransformSub = rossubscriber(self.baseTransformTopic, 'geometry_msgs/PoseStamped');
        end
        
        function transform = getBaseTransform(self)
            self.updateBasePose();
            transform = self.baseTransform;
        end
        
        function jointStates = getJointStates(self)
            self.updateJointStates();
            jointStates = self.jointStates;
        end
        
        function updateBasePose(self)
            %UPDATEBASEPOSE Updates the robot base transform
            baseTransformMsg = self.baseTransformSub.LatestMessage;
            if ~isempty(baseTransformMsg)
                self.baseTransform = UR3StatusUpdater.PoseStampedToTransform(baseTransformMsg);
            end
        end
        
        function updateJointStates(self)
            %UPDATEJOINTSTATES Updates the robot joint states
            jointStateMsg = self.jointStateSub.LatestMessage;
            if ~isempty(jointStateMsg)
                self.jointStates = jointStateMsg.Position';
            end
        end
        
        function getStatus(self)
            %UPDATEROBOT Updates the robot base transform and joint angles
            self.updateBasePose();
            self.updateJointStates();
        end
    end
    
    methods(Static)
        function transform = PoseStampedToTransform(poseStamped)
            %POSESTAMPEDTOTRANSFORM Turn a PoseStamped ROS message into a
            %Homogenneous transform (PoseStamped orientation is in Quaternion)
            transform = transl(poseStamped.Pose.Position.X ...
                              ,poseStamped.Pose.Position.Y ...
                              ,poseStamped.Pose.Position.Z);
            quat = [poseStamped.Pose.Orientation.W ...
                   ,poseStamped.Pose.Orientation.X ...
                   ,poseStamped.Pose.Orientation.Y ...
                   ,poseStamped.Pose.Orientation.Z];
            transform(1:3,1:3) = quat2rotm(quat);
            return;
        end
    end
end

