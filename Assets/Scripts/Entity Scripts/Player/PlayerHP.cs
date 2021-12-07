using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            OnPlayerHPChanged?.Invoke(hp, value);
            hp = value;
        }
    }

    public delegate void PlayerHPChanged(int oldHP, int newHP);
    public event PlayerHPChanged OnPlayerHPChanged;
}
