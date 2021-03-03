#import SB

leftMotor = sb.Motor(1)
rightMotor = sb.Motor(2)
batSoc = sb.getBatSoc()
leftMotor.sleep(False)
rightMotor.sleep(False)
rightMotor.distance(10, 10, 20, False)
leftMotor.distance(5, 10, 20, False)



