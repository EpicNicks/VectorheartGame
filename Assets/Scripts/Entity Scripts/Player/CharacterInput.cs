using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private PlayerStateManager psm;

    private void Awake()
    {
        psm = new PlayerStateManager(this);
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        psm.Update();
    }

    /// <summary>
    /// All input events from PlayerInput go here
    /// </summary>
    /// <param name="ctx"></param>
    public void ConsumeInput(InputAction.CallbackContext ctx)
    {
        psm.ConsumeInput(ctx);
    }

}
