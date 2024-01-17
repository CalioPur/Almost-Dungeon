using System.IO;
using UnityEngine;

public class Save
{
    private static DialogueManager dialogueManager;
    public static int currentDungeon = -1;
    public static int currentLevel = -1;
    
    // Start is called before the first frame update
    public static void LoadSave()
    {
        dialogueManager = DialogueManager._instance;
        if (PlayerPrefs.HasKey("currentDungeon"))
        {
            Debug.Log("Current dungeon: " + PlayerPrefs.GetInt("currentDungeon") + " Current level: " + PlayerPrefs.GetInt("currentLevel"));
            currentDungeon = PlayerPrefs.GetInt("currentDungeon");
        }
        else
        {
            Debug.Log("No save found");
            currentDungeon = -1;
            currentLevel = -1;
        }
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        if (PlayerPrefs.HasKey("dialoguesDB"))
        {
            dialogueManager.SetGlobalInkFile(PlayerPrefs.GetString("dialoguesDB"));
        }
        
    }

    public static void SaveAll()
    {
        PlayerPrefs.SetInt("currentDungeon", DungeonManager.currentLevel);
        PlayerPrefs.SetInt("currentLevel", DungeonManager.SelectedBiome);
        PlayerPrefs.SetString("dialoguesDB", dialogueManager.globalsInkFile.text);
        Debug.Log("Saved all" + PlayerPrefs.GetInt("currentDungeon") + " " + PlayerPrefs.GetInt("currentLevel") + " " + PlayerPrefs.GetString("dialoguesDB"));
    }
}
