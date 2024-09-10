using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sentences
{
    public string speaker;

    [TextArea(3, 8)]
    public string dialogues;
}

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "Dialogue/Dialogue Asset")]
public class DialogueAsset : ScriptableObject
{
    public Sentences[] Sentences;
}
