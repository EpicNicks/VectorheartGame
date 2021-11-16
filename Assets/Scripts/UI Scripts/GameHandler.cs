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

    public int Score;

    private void Start()
    {
        fullHP = playerHP.HP;
        playerHP.OnPlayerHPChanged += PlayerHP_OnPlayerHPChanged;
        Score = 0; 
    }
    private void Update()
    {
        //here should be a function to get current score;
        //will set text as Current Score
        countScore.text = Score.ToString();
        finalScore.text = Score.ToString();
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

    public int GetScore() {
        return Score;
    }
    public void SetScore(int newScore)
    {
        Score = newScore;
    }
}
