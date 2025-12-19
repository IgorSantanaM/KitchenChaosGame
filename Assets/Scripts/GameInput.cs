using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += InteractPerfomed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerfomed;
        playerInputActions.Player.Pause.performed += Pause_Perfomed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractPerfomed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerfomed;
        playerInputActions.Player.Pause.performed -= Pause_Perfomed;
        playerInputActions.Dispose();
    }

    private void Pause_Perfomed(CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePerfomed(CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerfomed(CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}