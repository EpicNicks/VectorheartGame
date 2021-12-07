using System.Collections;
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
    public TextMeshProUGUI countWave;
    public SpawnerMaster spawner;
    public GameObject FailScreen;

    public TextMeshProUGUI finalScore;


    private int fullHP;
    private float currentHPPercent;
    private int fullEnergy;
    private float currentEnergyPercent;
    private float fullCooldown;
    private float currentCoolDown;

    public Image healthBarObject;
    public Image energyBarObject;
    public Image AbilityMaskObject;
    public GameObject UltMaskObject;
    public float updateSpeedSeconds = 0.5f;

    private int score;
    private int preWave;
    public Animator WaveAnimator;
    private Color preColor;
    public ParticleSystem particle;
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
        WaveAnimator.SetBool("newWave", false);
        preWave = 0;
        preColor = healthBarObject.GetComponent<Image>().color;


        var main = particle.main;
        main.maxParticles = 0;
    }
    private void Update()
    {
        countWave.text = (spawner.getWave() + 1) + "";
        if (preWave != (spawner.getWave() + 1)){
            WaveAnimator.SetBool("newWave", true);
            preWave = (spawner.getWave() + 1);
        }
        else
        {
            WaveAnimator.SetBool("newWave", false);
        }
    }

    private void PlayerHP_OnPlayerHPChanged(int oldHP, int newHP)
    {
        currentHPPercent = (float)newHP / fullHP;
        healthBarObject.fillAmount = currentHPPercent;
        StartCoroutine(changeHPcolor());
        if(newHP<= 20)
        {
            StartCoroutine(fastChangeHPcolor());
        }
        if (newHP <= 0)
        {
            FailScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }
    private IEnumerator changeHPcolor() {
        
        healthBarObject.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(.5f);
        healthBarObject.GetComponent<Image>().color = preColor;
        yield return new WaitForSeconds(.5f);
    }

    private IEnumerator fastChangeHPcolor()
    {
        while(Time.timeScale != 0) { 
        healthBarObject.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        healthBarObject.GetComponent<Image>().color = preColor;
        yield return new WaitForSeconds(.1f);
        }
    }

    //private void PlayerHP_OnPlayerHPChanged(int newHP)
    //{
    //    //currentHPPercent = (float)newHP / fullHP;
    //    StartCoroutine(ChangeToHPPct((float)newHP));
    //    if (newHP <= 0)
    //    {
    //        FailScreen.SetActive(true);
    //        Time.timeScale = 0f;
    //    }
    //}
    //private IEnumerator ChangeToHPPct(float pct)
    //{
    //    float preChangePct = healthBarObject.fillAmount;
    //    float elapsed = 0f;
    //    while (elapsed < updateSpeedSeconds)
    //    {
    //        elapsed += Time.deltaTime;
    //        healthBarObject.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
    //        yield return null;
    //    }
    //    healthBarObject.fillAmount = pct;
    //    Debug.Log(pct);
    //}



    private void characterInput_OnEnergyChanged(int newEnergy)
    {
        var main = particle.main;
        currentEnergyPercent = (float)newEnergy / fullEnergy;
        energyBarObject.fillAmount = currentEnergyPercent;
        main.maxParticles = (int)(currentEnergyPercent * 1000);
        if (newEnergy >= fullEnergy)
        {
            UltMaskObject.SetActive(true);
            UltMaskObject.GetComponent<Animator>().Play("UltReady",0,0.25f);
        }
        else
        {
            UltMaskObject.SetActive(false);
        }
    }

    //private void characterInput_OnEnergyChanged(int newEnergy)
    //{
    //    //currentEnergyPercent = (float)newEnergy / fullEnergy;
    //    StartCoroutine(ChangeToEnergyPct((float)newEnergy));
    //    if (newEnergy >= fullEnergy)
    //    {
    //        UltMaskObject.SetActive(true);
    //    }
    //    else
    //    {
    //        UltMaskObject.SetActive(false);
    //    }
    //}
    //private IEnumerator ChangeToEnergyPct(float pct)
    //{
    //    float preChangePct = energyBarObject.fillAmount;
    //    float elapsed = 0f;
    //    while (elapsed < updateSpeedSeconds)
    //    {
    //        elapsed += Time.deltaTime;
    //        energyBarObject.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
    //        yield return null;
    //    }
    //    energyBarObject.fillAmount = pct;
    //}
    private void characterInput_OnDDashCooldownSecondsChanged(float newCooldown)
    {
        currentCoolDown = (float)newCooldown / fullCooldown;
        AbilityMaskObject.fillAmount = currentCoolDown;
        Debug.Log("test");
    }

}
