using System;
using UnityEngine;

public class LogFeedback : Singleton<LogFeedback>
{
    private TextMesh feedbackText;

    private void Awake()
    {
        feedbackText = GetComponent<TextMesh>();
        var meshRender = GetComponent<MeshRenderer>();
        meshRender.enabled = BattleGameManager.instance.isDebugMode;
    }

    public void AddMessage(string message)
    { 
        if (feedbackText == null) 
        { 
            return;
        }
        
        feedbackText.text += Environment.NewLine + message;
    }

    public void ReplaceMessage(string message)
    {
        feedbackText.text = message;
    }
}
