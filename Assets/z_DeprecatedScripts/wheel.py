class wheel():
    def __init__(self, isasleep):
        self.isasleep = True

    def sleep(self,order): #will adjust the sleeping variable
        if bool(order) == True:
            self.isasleep = True
            return 1
        elif bool(order) == False:
            self.isasleep = False
            return 1
        else: 
            return 0
            
    #def speed(self, steps, msec): #speed = steps/sec and milliseconds to get to that speed
     #   if self.isasleep:
      #      print(self+ " cannot move because it is asleep")
       # else:
    