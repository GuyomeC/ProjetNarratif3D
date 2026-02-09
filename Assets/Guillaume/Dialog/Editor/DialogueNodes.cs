using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;
using UnityEngine.TestTools.Constraints;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("out").Build();

    }
}

[Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
    }
}

public class DialogueNode : Node
{
    public const string bgSprite = "Image Background";
    public const string modeId = "Mode";
    public const string SpeakerOptOne = "Speaker Key One";
    public const string SpeakerOptTwo = "Speaker Key Two";
    public const string DialogueOpt = "Dialogue Key";
    public const string speakerSpriteOne = "Image Speaker One";
    public const string speakerSpriteTwo = "Image Speaker Two";
    public const string IsSpeakerOnLeft = "Is Speaker On Left";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddOutputPort("out").Build();
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<Sprite>(bgSprite).WithDefaultValue(null);
        context.AddOption<DialogueMode>(modeId).WithDefaultValue(DialogueMode.Panel).Delayed();
        context.AddOption<NameKey>(SpeakerOptOne).WithDefaultValue(NameKey.None);
        context.AddOption<NameKey>(SpeakerOptTwo).WithDefaultValue(NameKey.None);
        context.AddOption<DialogKey>(DialogueOpt).WithDefaultValue(DialogKey.None);
        context.AddOption<Sprite>(speakerSpriteOne).WithDefaultValue(null);
        context.AddOption<Sprite>(speakerSpriteTwo).WithDefaultValue(null);
        context.AddOption<bool>(IsSpeakerOnLeft).WithDefaultValue(true);
    }
}

[Serializable]
public class ChoiceNode : Node
{
    public const string modeId = "Mode";

    const string portId = "Port Count";
    public const string SpeakerOpt = "Speaker Key";
    public const string DialogueOpt = "Dialogue Key";
    public const string speakerSprite = "Image Speaker";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();

        var option = GetNodeOptionByName(portId);
        option.TryGetValue(out int portCount);
        for (int i = 0; i < portCount; i++)
        {
            context.AddInputPort<DialogKey>($"Choice Text {i}").Build();
            context.AddOutputPort($"Choice {i}").Build();
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<DialogueMode>(modeId).WithDefaultValue(DialogueMode.Panel).Delayed();
        context.AddOption<int>(portId).WithDefaultValue(2).Delayed();
        context.AddOption<NameKey>(SpeakerOpt).WithDefaultValue(NameKey.None);
        context.AddOption<DialogKey>(DialogueOpt).WithDefaultValue(DialogKey.None);
        context.AddOption<Sprite>(speakerSprite).WithDefaultValue(null);
    }
}