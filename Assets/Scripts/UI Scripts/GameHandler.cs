using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour {

    //[SerializeField] private HealthBar healthBar;

    [SerializeField] private NewHealthBar healthBar;
    [SerializeField] private PlayerHP playerHP;
    private int fullHP;
    private float currentHPPercent;
    public TextMeshProUGUI countScore;
    public GameObject FailScreen;
    public TextMeshProUGUI finalScore;

    private void Start()
    {
        fullHP = playerHP.HP;
        playerHP.OnPlayerHPChanged += PlayerHP_OnPlayerHPChanged;
    }
    private void Update()
    {
        //here should be a function to get current score;
        //will set text as Current Score
        countScore.text = "1";
        finalScore.text = "1";
    }

    private void PlayerHP_OnPlayerHPChanged(int newHP)
    {
        currentHPPercent = (float)newHP / fullHP;
        healthBar.SetSize(currentHPPercent);

        if (newHP <= 0)
        {
            FailScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
