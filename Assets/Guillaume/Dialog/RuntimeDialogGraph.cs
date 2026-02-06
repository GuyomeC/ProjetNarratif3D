using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RuntimeDialogGraph : ScriptableObject
{
    public string EntryNodeId;
    public List<RuntimeDialogNode> AllNodes = new List<RuntimeDialogNode>();
}


[Serializable]
public class RuntimeDialogNode
{
    public string NodeId;
    public string SpeakerNameOne;
    public string SpeakerNameTwo;
    public string DialogueText;
    public Sprite SpeakerSpriteOne;
    public Sprite SpeakerSpriteTwo;
    public List<ChoiceData> Choices = new List<ChoiceData>();
    public string NextNodeId;

    public DialogueMode Mode;
}

[Serializable]
public class  ChoiceData
{
    public string ChoiceText;
    public string DestinationNodeId;
}

public enum DialogueMode
{
    Panel,
    Popup,
    Bulle
}