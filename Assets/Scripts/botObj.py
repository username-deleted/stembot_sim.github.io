class botObj():
    def __init__(self, motor):
        self.motor = motor
        #self.lights = lights
    
    def hello(self):
        #self.motor.transform.Translate(1,1,1) #moves the wheel objects over
        returnstmt = ""
        for child in self.motor.transform:
            returnstmt += ("wheel: "+ child.name + "| ")
        return "Hello! I'm STEMBot!" + returnstmt

    def Motor(self, index):
        return self.motor.transform[index-1]

