using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LDCreatorEditor))]
public class LDCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LDCreator cartesViewer = (LDCreator)target;

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Navigation des Cartes", EditorStyles.boldLabel);

        // Boutons de navigation des cartes
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Carte Précédente"))
        {
            cartesViewer.AfficherCartePrecedente();
        }

        if (GUILayout.Button("Carte Suivante"))
        {
            cartesViewer.AfficherCarteSuivante();
        }

        EditorGUILayout.EndHorizontal();
    }
}
