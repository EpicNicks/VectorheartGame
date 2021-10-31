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
            OnHPChanged();
        }
    }

    private void OnHPChanged()
    {
        if (hp <= 0)
        {
            // Trigger death animation and stuff
            Destroy(gameObject);
        }
    }
}
