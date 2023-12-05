using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private TMP_Dropdown AIType;
    
    public static UIManager _instance;

    public HeroBlackboard heroBlackboard;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AIType.options.Clear();
        foreach (var personality in Enum.GetValues(typeof(Personnalities)))
        {
            AIType.options.Add(new TMP_Dropdown.OptionData(personality.ToString()));
        }
    }

    private void Update()
    {
        AIType.onValueChanged.AddListener(delegate { ChangeAIType(); });
    }

    private void ChangeAIType()
    {
        heroBlackboard.personality = (Personnalities) AIType.value;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
