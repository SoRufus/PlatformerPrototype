using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputSystem : Singleton<PlayerInputSystem>
{
    private PlayerInputAction inputAction;
    public float moveValue;

    public UnityAction OnJumpAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }
    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Walk.Enable();
        inputAction.Player.Jump.Enable();

        inputAction.Player.Walk.performed += ctx => moveValue = ctx.ReadValue<float>();
        inputAction.Player.Walk.canceled += ctx => moveValue = 0;

        inputAction.Player.Jump.performed += GetJumpInput;
    }
    private void OnDisable()
    {
        inputAction.Player.Disable();
        inputAction.Player.Walk.Disable();
        inputAction.Player.Jump.Disable();
    }
    public float GetMoveValue()
    {
        return moveValue;
    }
    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        OnJumpAction?.Invoke();
    }
}
