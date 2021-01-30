using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIMbot : MonoBehaviour
{
    public GameObject[] attachments;
    public int attachmentNumber = 0;
    private int MAX_ATTACHMENT_INDEX;


    private void Start()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
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
    }
}
