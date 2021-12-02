﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    //[SerializeField] private HealthBar healthBar;

    [SerializeField] private NewHealthBar healthBar;
    [SerializeField] private EnergyBar energyBar;
    [SerializeField] private AbilitiesUI abilityMask;
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private CharacterInput characterInput;
    public TextMeshProUGUI countScore;
    public GameObject FailScreen;
    public TextMeshProUGUI finalScore;


    private int fullHP;
    private float currentHPPercent;
    private int fullEnergy;
    private float currentEnergyPercent;
    private float fullCooldown;
    private float currentCoolDown;

    public Image healthBarObject;
    public float updateSpeedSeconds = 0.5f;

    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            countScore.text = value.ToString();
            finalScore.text = value.ToString();
        }
    }

    private void Start()
    {
        fullHP = playerHP.HP;
        playerHP.OnPlayerHPChanged += PlayerHP_OnPlayerHPChanged;
        Score = 0;

        //suppose energy maximum is 100
        fullEnergy = characterInput.Psm.MaxCharge;
        characterInput.OnEnergyChanged += characterInput_OnEnergyChanged;

       
        fullCooldown = characterInput.DashAttackCooldownSeconds;
        characterInput.OnDashCooldownSecondsChanged += characterInput_OnDDashCooldownSecondsChanged; 
    }

    //private void PlayerHP_OnPlayerHPChanged(int newHP)
    //{
    //    currentHPPercent = (float)newHP / fullHP;
    //    healthBar.SetSize(currentHPPercent);

    //    if (newHP <= 0)
    //    {
    //        FailScreen.SetActive(true);
    //        Time.timeScale = 0f;
    //    }
    //}
    private void PlayerHP_OnPlayerHPChanged(int newHP)
    {
        //currentHPPercent = (float)newHP / fullHP;
        StartCoroutine(ChangeToHPPct((float)newHP));
        if (newHP <= 0)
        {
            FailScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private IEnumerator ChangeToHPPct(float pct)
    {
        float preChangePct = healthBarObject.fillAmount;
        float elapsed = 0f;
        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthBarObject.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        healthBarObject.fillAmount = pct;
    }
    private void characterInput_OnEnergyChanged(int newEnergy)
    {
        currentEnergyPercent = (float)newEnergy / fullEnergy;
        energyBar.SetSize(currentEnergyPercent);
    }
    private void characterInput_OnDDashCooldownSecondsChanged(float newCooldown)
    {
        currentCoolDown = (float)newCooldown / fullCooldown;
        abilityMask.SetSize(currentCoolDown);
    }

}
