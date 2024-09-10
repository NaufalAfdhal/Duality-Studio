using TMPro;
using UnityEngine;

[System.Serializable]
public class ThouWhoSpeaks
{
    [SerializeField] string name = "Yukihara";
    [SerializeField] string testDialogue = "Ight, we're using this as a test";
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
}
