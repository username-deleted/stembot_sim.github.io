using System.IO;
using UnityEngine;

public class SIMbot : MonoBehaviour
{
    public GameObject[] attachments;
    public int attachmentNumber = 0;
    public bool pythonBot = false;
    public SIMbotData SBData;
    private int MAX_ATTACHMENT_INDEX;


    private void Start()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
        //initialize bot data
        InitSBData();
    }

    private void InitSBData()
    {
        //if there exists data for our bot, load it
        if (File.Exists(SaveSystem.SAVE_FOLDER + "/data.txt"))
        {
            SBData = JsonUtility.FromJson<SIMbotData>(SaveSystem.Load());
        }
        else
        {
            SBData = new SIMbotData(attachmentNumber, pythonBot);
        }
    }

    public void NextAttachment()
    {
        if(attachmentNumber == MAX_ATTACHMENT_INDEX)
        {
            attachmentNumber = 0;
        }
        else
        {
            attachmentNumber++;
        }
        //Save it in SBData to persist between scenes.
        SBData.attachmentNumber = attachmentNumber;
    }

    public void PreviousAttachment()
    {
        if(attachmentNumber == 0)
        {
            attachmentNumber = MAX_ATTACHMENT_INDEX;
        }
        else
        {
            attachmentNumber--;
        }
        //Save it in SBData to persist between scenes.
        SBData.attachmentNumber = attachmentNumber;
    }

    public void TogglePythonBot()
    {
        pythonBot = !pythonBot;
        SBData.pythonBot = pythonBot;
    }

    public void UpdateSBData(SIMbotData data)
    {
        SBData.attachmentNumber = data.attachmentNumber;
        SBData.pythonBot = data.pythonBot;
    }

    public class SIMbotData
    {
        public int attachmentNumber = 0;
        public bool pythonBot = false;
        public SIMbotData()
        {
            attachmentNumber = 0;
            pythonBot = false;
        }

        public SIMbotData(int attachmentNumber, bool pythonBot)
        {
            this.attachmentNumber = attachmentNumber;
            this.pythonBot = pythonBot;
        }
    }
}
