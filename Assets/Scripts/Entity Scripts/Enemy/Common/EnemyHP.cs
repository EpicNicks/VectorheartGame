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

    [SerializeField]
    private int chargePercent = 5;
    public int ChargePercent => chargePercent;
}
