import clr
clr.AddReferenceToFileAndPath('..\Assets\Plugins\DLLs\Motor.dll')
import Motor
import UnityEngine
from UnityEngine import *
import System.Collections
from System.Collections import *
import System.Collections.Generic
from System.Collections.Generic import *

class SB:
    def __init__(self, motors): #if we can make this a direct reference to the gameobject 
        self.motors = motors


    def Motor(self, id):
        wheel = self.motors.transform.GetChild(id-1).gameObject
        if(wheel != False):
            obj = Motor.Motor(wheel,False,0, 0, False)
            t = type(obj)
            wheel.gameObject.AddComponent[t]()
            miniMotor = wheel.gameObject.GetComponent[t]()
            if(miniMotor != False):
                miniMotor.wheel = wheel
                miniMotor.sleeping = True
                miniMotor.sps = 0
                miniMotor.milli = 0
                miniMotor.brake = True
            else:
                print("ugh")
            return miniMotor
        else:
            return "wheel is null"

    #at the moment this simulation does not have a charger/battery life 
    #function, perhaps we could add one later as a sort of game mode
    def getBatSoc(self):
        return 100

    def getBatVoltage(self):
        return 32
    
    def getBatUAH(self):
        return 3800000
    
    def getChargerStatus(self):
        return 32
    
    def getChargerFWRev(self):
        return 32