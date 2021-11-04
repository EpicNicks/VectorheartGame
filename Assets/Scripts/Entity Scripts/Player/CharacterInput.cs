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
    public Animator Anim => anim;

    #region Inspector Data
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    [SerializeField]
    private float sprintSpeed;
    public float SprintSpeed => sprintSpeed;

    [SerializeField]
    private float idleSecondsToPlayIdleAnim = 3.0f;
    public float IdleSecondsToPlayIdleAnim => idleSecondsToPlayIdleAnim;

    [SerializeField]
    private float dashDistance;
    public float DashDistance => dashDistance;

    [SerializeField]
    private float dashSeconds;
    public float DashSeconds => dashSeconds;
    #endregion

    private PlayerStateManager psm;

    private void Awake()
    {
        psm = new PlayerStateManager(this);
        anim ??= GetComponent<Animator>();
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
