using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    private PlayerMovement inputActions;
    public event EventHandler OnDash;
    

    private void Start()
    {
        inputActions = new PlayerMovement();
        inputActions.Player.Dash.performed += Dash_performed;
        inputActions.Player.Enable();
       
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDash?.Invoke(this,EventArgs.Empty);
          
    }

    public Vector2 Move()
    {
        Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();
        return input;

    }


}
