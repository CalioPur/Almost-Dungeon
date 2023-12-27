using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LDCreator : MonoBehaviour
{
    [SerializeField] private List<CardInfo> cartes; // Liste des cartes à afficher
    [SerializeField] private Image imageCarte; // Image de la carte à afficher
    [SerializeField] private MapManager mapManager; // Image de la carte à afficher
    
    private int currentIndex = 0; // Index actuel dans la liste
    private CardInfoInstance currentInstance; // Instance actuelle de la carte

#if UNITY_EDITOR
    [CustomEditor(typeof(LDCreator))]
    public class CartesViewerEditor : Editor
    {
        private void OnSceneGUI()
        {
            Debug.Log("OnSceneGUI");
            // Vérifier si un clic gauche de la souris est détecté dans la scène Unity
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                LDCreator cartesViewer = (LDCreator)target;

                // Votre logique pour gérer le clic de la souris dans la scène
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    // Votre action à effectuer lors du clic sur un objet
                    GameObject objClique = hitInfo.collider.gameObject;
                    Debug.Log("Objet cliqué : " + objClique.name);
                    
                    TileData tile = objClique.GetComponent<TileData>();
                    if (tile != null)
                    {
                        Debug.Log("Tile cliquée : " + tile.name);
                        if (tile.PiecePlaced)
                        {
                            Debug.Log("Tile cliquée : " + tile.name + " - " + tile._instance.So.name);
                        }
                        else
                        {
                            Debug.Log("Tile cliquée : " + tile.name + " - " + "Pas de carte");
                            tile.SetInstance(cartesViewer.currentInstance);
                            tile.img.sprite = cartesViewer.currentInstance.So.imgOnMap;
                        }
                    }
                }
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LDCreator cartesViewer = (LDCreator)target;

            GUILayout.Space(10);

            EditorGUILayout.LabelField("", EditorStyles.boldLabel);

            // Boutons de navigation des cartes
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Carte Précédente"))
            {
                Debug.Log("Carte Précédente");
                cartesViewer.AfficherCartePrecedente();
                Debug.Log("Carte Précédente 2");
            }

            if (GUILayout.Button("Carte Suivante"))
            {
                Debug.Log("Carte Suivante 2");
                cartesViewer.AfficherCarteSuivante();
                Debug.Log("Carte Suivante 3");
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Generate World"))
            {
                Debug.Log("Carte Suivante 2");
                cartesViewer.mapManager.SpawnMap();
                Debug.Log("Carte Suivante 3");
            }
        }
    }
#endif

    // Afficher la carte suivante dans la liste
    public void AfficherCarteSuivante()
    {
        Debug.Log("Carte Suivante");
        if (currentIndex < cartes.Count - 1)
        {
            currentIndex++; // Passer à la carte suivante
            currentInstance = cartes[currentIndex].CreateInstance(); // Afficher la nouvelle carte
            imageCarte.sprite = cartes[currentIndex].imgOnHand;
        }
    }

    // Afficher la carte précédente dans la liste
    public void AfficherCartePrecedente()
    {
        Debug.Log("Carte Précédente 3");
        if (currentIndex > 0)
        {
            currentIndex--; // Passer à la carte précédente
            currentInstance = cartes[currentIndex].CreateInstance(); // Afficher la nouvelle carte
            imageCarte.sprite = cartes[currentIndex].imgOnHand;
        }
    }
}
