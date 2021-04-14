# import sb
# import time
import UnityEngine
from UnityEngine import *

motor_1 = sb.Motor(1)
motor_2 = sb.Motor(2)

#motor_1.sleep(True)
#print(motor_1.sleep())

#motor_2.sleep(False)
#print(motor_2.sleep())


# sleep two seconds
#UnityEngine.Debug.Log("sleeping")
time.sleep(10)

motor_1.speed(100)

#UnityEngine.Debug.Log("sleeping")
time.sleep(10)

motor_2.speed(100)

# motor_1.
