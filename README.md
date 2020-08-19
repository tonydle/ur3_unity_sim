# UR3 Unity Simulation

An open-sourced Unity Simulation for Universal Robot's UR3 Manipulator
## ![UR3Sim logo](https://github.com/tonydle/ur3_unity_sim/blob/master/unity/UR3Sim/Assets/Graphics/logo.png)

## Dependencies ##
If you do not wish to modify the simulation, please refer to our [Releases](https://github.com/tonydle/ur3_unity_sim/releases)
and [Wiki](https://github.com/tonydle/ur3_unity_sim/wiki) for usage instructions

The MATLAB examples require [Peter Corke's Robotics Toolbox](https://github.com/petercorke/robotics-toolbox-matlab)

For Unity developers:
* Please install [Unity 2019.4](https://store.unity.com/download) - Older or later versions of Unity have not been tested
* The simulation communicates with ROS using [ROS#](https://github.com/siemens/ros-sharp).
However it has been included in [unity/UR3Sim/Assets](https://github.com/tonydle/ur3_unity_sim/tree/master/unity/UR3Sim/Assets) already

## Contents ##

* [matlab](https://github.com/tonydle/ur3_unity_sim/tree/master/matlab): MATLAB examples to communicate with the simulated robot via ROS.
* [ros](https://github.com/tonydle/ur3_unity_sim/tree/master/ros):
includes the [file_server](https://github.com/tonydle/ur3_unity_sim/tree/master/ros/file_server) package
from [ROS#](https://github.com/siemens/ros-sharp)
* [unity](https://github.com/tonydle/ur3_unity_sim/tree/master/unity): includes
the [UR3Sim](https://github.com/tonydle/ur3_unity_sim/tree/master/unity/UR3Sim) Unity project, which has:
    - **UR3** Scene: the base UR3 that can be modified for your application
    - **UR3 World** Scene: the UR3 on a table with blocks
    
---
Developed by Tony Le (tony@mechatony.com)
