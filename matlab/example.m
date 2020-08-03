%% UR3 Unity Simulation ROS Communication example
% This example code will go through all of the key functionalities of the
% classes in UR3Communicator, to send control commands and receive data
% from the Unity robot via ROS.

%% 1. CONNECTING MATLAB TO THE ROS NETWORK
% Modify the values below depending on your system setup
masterURI = "http://192.168.2.2:11311";
ip = "192.168.2.1";

rosshutdown
setenv('ROS_MASTER_URI',masterURI);
setenv('ROS_IP',ip);
rosinit

%% 2. GETTING THE ROBOT'S STATUS
% Create an object of the UR3StatusUpdater class
statusUpdater = UR3StatusUpdater();

% Get the current base transform (4x4 Homogeneous)
base = statusUpdater.getBaseTransform()

% Get the current joint angles (1x6 Matrix)
jointAngles = statusUpdater.getJointStates()

%% 3. SENDING A JOINT TRAJECTORY
% Create an object of the UR3TrajectoryPublisher class
trajPub = UR3TrajectoryPublisher();

%% 3a. Initialising the UR3TrajectoryPublisher with a set number of steps
% Note that this process takes a long time and should be done only once at
% the beginning
steps = 100;
trajPub.InitPublisher(steps);

%% 3b. Creating and sending a trajectory
% Getting the current joint angles and creating a trajectory
time = 2.0; % seconds
deltaTheta = deg2rad(-30)/steps;                % we are rotating each joint -30 degrees
jointAngles = statusUpdater.getJointStates();   % get the current joint positions
qMatrix = nan(steps,6);                         % this is our trajectory
qMatrix(1,:) = jointAngles;                     % we are starting from the current position
for i = 2:steps
    qMatrix(i,:) = qMatrix(i-1,:) + deltaTheta;
end
velMatrix = zeros(steps,6);                     % the velocity Matrix
deltaT = time/steps;
for i = 1:steps-1
    velMatrix(i,:) = (qMatrix(i+1,:) - qMatrix(i,:))/deltaT;
end

% Sending the trajectory to the robot
trajPub.SendTrajectory(qMatrix,velMatrix,deltaT);