using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroStatus : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text endGamePanelText;
    // Start is called before the first frame update
    public static HeroStatus Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (hp <= 0 || hp == null)
        {
            hp = 1;
        }
        hpText.text = "HP : " + hp;
    }

    public void LooseHp(int nb)
    {
        hp -= nb;
        hpText.text = "HP : " + hp;
        if (hp > 0) return;
        Debug.Log("Hero is dead");
        Time.timeScale = 0;
        endGamePanelText.text = "You won, Hero is dead !";
        GameManager.Instance.EndGamePanel.SetActive(true);
    }
}
