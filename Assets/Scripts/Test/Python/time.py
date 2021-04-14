import UnityEngine
from UnityEngine import *
import System.Collections
from System.Collections import *
import System.Collections.Generic
from System.Collections.Generic import *


class Time:
    def __init__(self, _cScript):
        global cScript
        cScript = _cScript
    
    def sleep(self, duration):
      cScript.GenerateTimeSleepEvent(duration)