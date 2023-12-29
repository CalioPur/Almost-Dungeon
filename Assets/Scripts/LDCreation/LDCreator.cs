using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LDCreator : MonoBehaviour
{
    public Transform map;
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private List<CardInfo> cartes;
    [SerializeField] private Image imageCarte;
    [SerializeField] private RectTransform imageCarteTr;
    [SerializeField] private Transform floor;

    [Header("Data Button")] [SerializeField]
    private int indexCardToSelect;

    [SerializeField] private string nameWorldToSave;

    private int currentIndex = 0;
    private CardInfoInstance currentInstance;
    private LDEditorData[,] mapArray;

#if UNITY_EDITOR
    [CustomEditor(typeof(LDCreator))]
    public class CartesViewerEditor : Editor
    {
        private void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                LDCreator cartesViewer = (LDCreator)target;
                if (cartesViewer.currentInstance == null)
                    cartesViewer.currentInstance = cartesViewer.cartes[0].CreateInstance();

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject objClique = hitInfo.collider.gameObject;

                    TileData tile = objClique.GetComponent<TileData>();
                    if (tile != null)
                    {
                        if (tile.PiecePlaced)
                        {
                        }
                        else
                        {
                            tile.SetInstance(cartesViewer.currentInstance);
                            tile.img.sprite = cartesViewer.currentInstance.So.imgOnHand;
                            LDEditorData data = tile.GetComponent<LDEditorData>();
                            data.nbRotation = cartesViewer.currentInstance.Rotation;
                            data.cardInfo = cartesViewer.currentInstance.So;
                            data.isUsed = true;
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
            if (GUILayout.Button("Generate World"))
            {
                cartesViewer.SpawnMap();
            }

            if (GUILayout.Button("Clear World"))
            {
                for (int i = 0; i < cartesViewer.map.childCount; i++)
                {
                    DestroyImmediate(cartesViewer.map.GetChild(i).gameObject);
                    i--;
                }
            }

            if (GUILayout.Button("Select Card with index"))
            {
                cartesViewer.SelectWithIndex();
            }

            if (GUILayout.Button("Save World with name"))
            {
                cartesViewer.SaveWorld();
            }
            if (GUILayout.Button("Rotate"))
            {
                cartesViewer.currentInstance.AddRotation(false);
                cartesViewer.imageCarteTr.rotation = Quaternion.Euler(0, 0, cartesViewer.currentInstance.Rotation);
                
            }
        }
    }


#endif

    private void CreatePresetSO(LDEditorData data, TilePresetSO nouvelleInstance, int x, int y)
    {
        TilePresetStruct newPreset = new TilePresetStruct();
        newPreset.position = new Vector2Int(x, y);
        newPreset.cardInfo = data.cardInfo;
        newPreset.rotation = data.nbRotation / 90;
        Debug.Log("Create Preset SO : " + newPreset.position + " - " + newPreset.cardInfo.name + " - " +
                  newPreset.rotation);
        nouvelleInstance.tilePresets.Add(newPreset);
    }

    private void SaveWorld()
    {
        Debug.Log("Save World");
        TilePresetSO nouvelleInstance = ScriptableObject.CreateInstance<TilePresetSO>();
        nouvelleInstance.tilePresets = new List<TilePresetStruct>();
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j].isUsed)
                {
                    CreatePresetSO(mapArray[i, j], nouvelleInstance, i, j);
                }
            }
        }

        // Utilisez AssetDatabase pour sauvegarder l'instance nouvellement créée
    #if UNITY_EDITOR
        AssetDatabase.CreateAsset(nouvelleInstance, "Assets/" + nameWorldToSave + ".asset");
        // AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    #endif

        Debug.Log("Instance de ScriptableObject créée avec succès !");
    }

    private void SelectWithIndex()
    {
        currentInstance = cartes[indexCardToSelect].CreateInstance();
        currentIndex = indexCardToSelect;
        imageCarte.sprite = cartes[currentIndex].imgOnHand;
        imageCarteTr.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void AfficherCarteSuivante()
    {
        Debug.Log("Carte Suivante");
        if (currentIndex < cartes.Count - 1)
        {
            currentIndex++;
            currentInstance = cartes[currentIndex].CreateInstance();
            imageCarte.sprite = cartes[currentIndex].imgOnHand;
            imageCarteTr.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void AfficherCartePrecedente()
    {
        Debug.Log("Carte Précédente 3");
        if (currentIndex > 0)
        {
            currentIndex--;
            currentInstance = cartes[currentIndex].CreateInstance();
            imageCarte.sprite = cartes[currentIndex].imgOnHand;
            imageCarteTr.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SpawnMap()
    {
        mapArray = new LDEditorData[width, height];
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                Vector3 pos = new Vector3(i - ((float)(width + 1) / 2), 0, j - (float)(height + 1) / 2);
                if (!(i == 0 || j == 0))
                {
                    mapArray[i - 1, j - 1] =
                        Instantiate(floor, pos, floor.transform.rotation, map)
                            .AddComponent<LDEditorData>();
                    mapArray[i - 1, j - 1].isUsed = false;
                }
            }
        }
        indexCardToSelect = 0;
        SelectWithIndex();
    }
}