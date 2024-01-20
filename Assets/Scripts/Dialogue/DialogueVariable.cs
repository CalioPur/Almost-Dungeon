using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using JetBrains.Annotations;
using Object = Ink.Runtime.Object;

public class DialogueVariable
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    private TextAsset asset;
    public void StartListening(Story story)
    {
        VariableToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

        
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }
    
    public DialogueVariable(string globalFilePath)
    {
        //asset = loadGlobalsJSON;
        string inkfileContents = File.ReadAllText(globalFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkfileContents);
        Story globalVariablesStory = compiler.Compile();
        
        /*var globalVariablesStory = new Story(asset.text); */

        variables = new Dictionary<string, Object>();
        foreach (var name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable" + name + " = " + value);
        }
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        Debug.Log("Variable changed: " + name + " = " + value);

        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }

        var globalVariablesStory = new Story(asset.text);
        var obj = globalVariablesStory.variablesState.GetVariableWithName(name);
        Debug.Log($"Object: {obj}");
    }

    private void VariableToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
    
}
