using UnityEngine;

[CreateAssetMenu(fileName = "SIMbotData", menuName = "ScriptableObjects/SIMbotScriptableObject", order = 1)]
public class SIMbotObject : ScriptableObject
{
    public int attachmentNumber = 0;
    public bool pythonBot = false;

    public void ResetAttachmentNumber()
    {
        attachmentNumber = 0;
    }

    public void SetAttachmentNumber(int number)
    {
        attachmentNumber = number;
    }
}
