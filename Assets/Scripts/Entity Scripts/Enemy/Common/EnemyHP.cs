using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private int hp;
    public int HP { 
        get => hp; 
        set
        {
            hp = value;
            OnEnemyHPChanged?.Invoke(value);
        }
    }

    public delegate void EnemyHPChanged(int newHP);
    public event EnemyHPChanged OnEnemyHPChanged;
}
