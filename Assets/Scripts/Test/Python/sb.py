# import clr

# clr.AddReferenceToFileAndPath("PythonBot.cs")
import UnityEngine
from UnityEngine import *
import System.Collections
from System.Collections import *
import System.Collections.Generic
from System.Collections.Generic import *


class SB:
    def __init__(self, _cScript):
        global cScript
        cScript = _cScript

    class Motor:
        def __init__(self, id):
            self.id = id
            self.sleeping = False
            self.brakeMode = True
            self.motorSpeed = 0
            self.motor = cScript.CreateMotor(self.id)
            # if id == 1:
            #     self.motor_1 = self.motors.transform.GetChild(id - 1).gameObject
            #     return self.motor_1
            # elif id == 2:
            #     self.motor_2 = self.motors.transform.GetChild(id - 1).gameObject
            #     return self.motor_2
            # else:
            #     print("no such motor")
            #     return self

        def sleep(self, sleeping=None):
            if sleeping == None:
                return self.motor.sleep()
            else:
                self.sleeping = sleeping
                # do Unity code setting the motor to sleep in unity
                self.motor.sleep(sleeping)

        def brake_mode(self, brakeMode=None):
            if brakeMode == None:
                return self.brakeMode
            else:
                self.brakeMode = brakeMode
                # do Unity code setting the brake mode

        def speed(self, motorSpeed=None):
            if motorSpeed == None:
                return self.motor.speed()
            else:
                self.motorSpeed = motorSpeed
                # do Unity code setting the speed of the motor
                self.motor.speed(motorSpeed)

        def distance(self, steps, motorSpeed, acceleration, blocking):
            print("nothing")
            # do Unity code that moves the bot the necessary distance

        def helloWorld(self):
            return "Hello Unity!"