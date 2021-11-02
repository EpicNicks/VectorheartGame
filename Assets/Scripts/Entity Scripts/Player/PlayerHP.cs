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
            OnPlayerHPChanged?.Invoke(hp = value);
        }
    }

    public delegate void PlayerHPChanged(int newHP);
    public event PlayerHPChanged OnPlayerHPChanged;
}
