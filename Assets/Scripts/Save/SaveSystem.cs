using UnityEngine;

public class SaveSystem
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
            currentDungeon = PlayerPrefs.GetInt("currentDungeon");
        }
        else
        {
            currentDungeon = -1;
            currentLevel = -1;
        }
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        if (PlayerPrefs.HasKey("dialoguesDB"))
        {
            //dialogueManager.SetGlobalInkFile(PlayerPrefs.GetString("dialoguesDB"));
        }
        
    }

    public static void SaveAll()
    {
        PlayerPrefs.SetInt("currentDungeon", DungeonManager._instance.currentLevel);
        PlayerPrefs.SetInt("currentLevel", DungeonManager.SelectedBiome);
        //PlayerPrefs.SetString("dialoguesDB", dialogueManager.globalsInkFile.text);
    }
}
