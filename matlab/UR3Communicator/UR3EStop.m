classdef UR3EStop
    %UR3ESTOP Sends E-Stop commands (stop/restart) to the UR3 with ROS
    
    properties
        eStopTopic;
        eStopPub;
        eStopMsg;
    end
    
    methods
        function self = UR3EStop()
            %UR3ESTOP Construct an instance of this class
            %   Initialises default properties
            self.eStopTopic = '/ur3/estop';
            self.eStopMsg = rosmessage('std_msgs/Bool');
            self.eStopPub = rospublisher(self.eStopTopic,'std_msgs/Bool');
        end
        
        function eStop(self)
            %ESTOPROBOT Stops the robot
            self.eStopMsg.Data = true;
            send(self.eStopPub, self.eStopMsg); pause(0.1);
            disp("Sent E-Stop command");
        end
        
        function restart(self)
            %ESTOPROBOT Restarts (un-pauses) the robot
            self.eStopMsg.Data = false;
            send(self.eStopPub, self.eStopMsg); pause(0.1);
            disp("Sent restart command");
        end
    end
end

