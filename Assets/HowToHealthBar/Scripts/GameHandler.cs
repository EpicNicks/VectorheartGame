using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PlayerHP playerHP;
    private int fullHP;
    private float currentHPPercent;

    private void Start()
    {
        fullHP = playerHP.HP;
        playerHP.OnPlayerHPChanged += PlayerHP_OnPlayerHPChanged;
        

    }

    private void PlayerHP_OnPlayerHPChanged(int newHP)
    {
        currentHPPercent = newHP / fullHP;
        healthBar.SetSize(newHP);
    }


}
