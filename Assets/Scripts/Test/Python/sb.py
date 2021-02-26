# import clr

# clr.AddReferenceToFileAndPath("PythonBot.cs")
import UnityEngine
from UnityEngine import *
import System.Collections
from System.Collections import *
import System.Collections.Generic
from System.Collections.Generic import *


class SB:
    def __init__(self, motorObj):
        self.motor = motorObj
        self.sleeping = False
        self.brakeMode = True
        self.motorSpeed = 0

    def Motor(self, id):
        wheel = self.motor.transform.GetChild(id - 1).gameObject
        return self

    def sleep(self, sleeping=None):
        if sleeping == None:
            return self.sleeping
        else:
            self.sleeping = sleeping
            # do Unity code setting the motor to sleep in unity

    def brake_mode(self, brakeMode=None):
        if brakeMode == None:
            return self.brakeMode
        else:
            self.brakeMode = brakeMode
            # do Unity code setting the brake mode

    def speed(self, motorSpeed=None):
        if motorSpeed == None:
            return self.motorSpeed
        else:
            self.motorSpeed = motorSpeed
            # do Unity code setting the speed of the motor

    def distance(self, steps, motorSpeed, acceleration, blocking):
        print("nothing")
        # do Unity code that moves the bot the necessary distance

    def helloWorld(self):
        return "Hello Unity!"