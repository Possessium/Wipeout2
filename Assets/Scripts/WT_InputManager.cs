using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WT_InputManager : MonoBehaviour
{
    #region Events
    public static event Action<float, float> OnCamera = null;
    public static event Action OnCameraNoInput = null;
    public static event Action<float> OnTurn = null;
    public static event Action<float> OnAccelerate = null;
    public static event Action<float> OnBrake = null;
    public static event Action OnNoSpeed = null;
    public static event Action OnNoTurn = null;
    #endregion


    #region Unity methods
    private void Update()
    {
        if (Input.GetAxisRaw("CamX") != 0 || Input.GetAxisRaw("CamY") != 0) OnCamera?.Invoke(Input.GetAxis("CamX"), Input.GetAxis("CamY"));
        else OnCameraNoInput?.Invoke();
        if (Input.GetAxisRaw("Horizontal") != 0) OnTurn?.Invoke(Input.GetAxis("Horizontal"));
        else OnNoTurn?.Invoke();
        if (Input.GetAxisRaw("Vertical") > 0) OnBrake?.Invoke(Input.GetAxis("Vertical"));
        else if (Input.GetAxisRaw("Vertical") < 0) OnAccelerate?.Invoke(Input.GetAxis("Vertical"));
        else OnNoSpeed?.Invoke();
    }
    #endregion
}
