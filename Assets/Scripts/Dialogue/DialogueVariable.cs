using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using Object = Ink.Runtime.Object;

public class DialogueVariable
{
    private Dictionary<string, Object> variables;
    public void StartListening(Story story)
    {
        VariableToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public DialogueVariable(TextAsset loadGlobalsJSON)
    {
        var globalVariablesStory = new Story(loadGlobalsJSON.text);

        variables = new Dictionary<string, Object>();
        foreach (var name in globalVariablesStory.variablesState)
        {
            var value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable" + name + " = " + value);
        }
    }
    
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        Debug.Log("Variable changed: " + name + " = " + value);
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariableToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
    
}
