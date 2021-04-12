# SIMbot | UNO Capstone Project
### About
Unity simulation build project for Prairie STEM and STEMbot for UNO Capstone.

Team Members:
- Mason Fleming
- Cassie Miller
- Jackson Henery

Description:
 - This simulation is a virtual representation of the Physical STEMbot 2. 
 - It has simulated physics and a 3D model of the STEMbot 2. 
 - The focus of this simulation is to allow K-12 graders to learn and practice STEM skills with the STEMbot 2 remotely.
This will help ease the learning curve if and when they use the real physical STEMbot.

How to run simulation in Unity Editor: (A Standalone Executable will be created as our final deliverable of the simulation)
  - Verify Unity Version 2020.1.20f1 is installed.
  - Import project into Unity.
  - Make sure the 'FinalMainMenu' scene is loaded.
  - Press play button at top to run simulation.

Controls: 
  - Tank Control Scheme:
    W - Left Wheel Forward
    S - Left Wheel Backward
    I - Right Wheel Forward
    K - Right Wheel Backward
    
  - Classis Keyboard Control Scheme:
    W/S - Forwards and Backwards
    A/D - Turn Left and Right
    
  - Controller Scheme (Classic):
    Left Stick - Fowards/Backwards
    Start - Pause
  - Controller Scheme (Tank):
    Not yet implemented.
    
### What’s new in milestone 4:

- Added a health bar, HealthBar script, Health script, and updated the SIMbot script to store the health. The health script keeps track of health while the HealthBar script manages the UI health bar component. Also added the STEMBoT logo to the health bar.

- Updates to Level 5
  - Obstacle Updates
  - Scenery Updates
- Fixes/Tweaks
  - Found and fixed wheel inconsistencies leading to graphical issues. This may have been causing some driving issues as well.
- First pass wheel physics tweak on SIMbot in Level 4.
  - Things should feel less slippery when driving.
- More camera tweaks.

- Level 1 will now end when all 12 coins are collected, displaying the user's time taken and giving the user an option to go back to the main menu, restart the level, or quit.
- Level 4 will now end when the user reaches the finish line of the parkour course.
- SIMbot is now responding to events thrown by PythonBot in a test level, this is not publicly available yet.


