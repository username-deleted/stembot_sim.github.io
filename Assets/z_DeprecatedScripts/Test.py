import UnityEngine
import SIMbot


class Test:
    def __init__(self, botScript):
        self.test = 0
        self.botScript = botScript

    def helloUnity(self):
        UnityEngine.Debug.Log("Hi Unity!")
    
    def getBotScript(self):
        testScript = self.botScript.gameObject.GetComponent[SIMbot]()
        testScript.SayHi()



# class Test:
#     def __init__(self):
#       #do nothing

#     def helloUnity():
#       return 'hello unity!'

#     def getSIMBot():
#       simbotScript = self.transform.GetComponent<SIMBot>()
#       simbotScript.SayHi()