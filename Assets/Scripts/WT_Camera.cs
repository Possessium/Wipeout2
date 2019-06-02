using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Camera : MonoBehaviour
{
    #region Fields
    [SerializeField] WT_Car target = null;

    [SerializeField, Range(1, 20)] float cameraDepth = 10;
    [SerializeField, Range(-20, 20)] float cameraHeight = 10;
    [SerializeField, Range(-20, 20)] float cameraOffsetX = 0;
    [SerializeField, Range(.1f, 20)] float cameraUpdateTime = 2;
    [SerializeField, Range(.1f, 50)] float cameraRotateUpdateTime = 30;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    [SerializeField] float x = 0.0f;
    [SerializeField] float y = 0.0f;

    [SerializeField] bool isInput = false;
    #endregion

    public bool IsReady => target;

    #region Unity methods
    private void Start()
    {
        WT_InputManager.OnCamera += MoveOnInput;
        WT_InputManager.OnCameraNoInput += SetUnInput;
    }

    private void LateUpdate()
    {
        if (!IsReady) return;
        SetCameraLookAt();
        if(!isInput) Move();
    }


    #endregion

    #region Custom methods
    void MoveOnInput(float _x, float _y)
    {
        if (!IsReady) return;
        isInput = true;
        x += _x * xSpeed * cameraDepth * 0.02f;
        y -= _y * ySpeed * 0.02f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -cameraDepth);
        Vector3 position = rotation * negDistance + target.transform.position;

        transform.rotation = rotation;
        transform.position = Vector3.Slerp(transform.position, position, Time.deltaTime * cameraUpdateTime);
    }
    void SetUnInput() => isInput = false;
    void Move()
    {
        Vector3 _cameraPos = target.transform.position - (target.transform.forward * cameraDepth) + new Vector3(cameraOffsetX, cameraHeight, 0);
        _cameraPos.y = transform.position.y;
        transform.position = Vector3.Slerp(transform.position, _cameraPos, Time.deltaTime * cameraUpdateTime);
        y = transform.localEulerAngles.x;
        x = transform.localEulerAngles.y;
    }
    void SetCameraLookAt()
    {
        Quaternion _lookAt = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookAt, Time.deltaTime * (cameraRotateUpdateTime * target.CarCharacteristics.MaxSpeed));
        Vector3 _camTorque = AngleLerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, target.transform.localEulerAngles.z), Time.deltaTime * 10);
        _camTorque.z = ClampAngle(_camTorque.z, -10, 10);
        transform.localEulerAngles = _camTorque;
    }
    Vector3 AngleLerp(Vector3 _startAngle, Vector3 _finishAngle, float _t)
    {
        float _xLerp = Mathf.LerpAngle(_startAngle.x, _finishAngle.x, _t);
        float _yLerp = Mathf.LerpAngle(_startAngle.y, _finishAngle.y, _t);
        float _zLerp = Mathf.LerpAngle(_startAngle.z, _finishAngle.z, _t);
        Vector3 _lerped = new Vector3(_xLerp, _yLerp, _zLerp);
        return _lerped;
    }
    public static float ClampAngle(float _angle, float _angleMin, float _angleMax)
    {
        if (_angle < 90 || _angle > 270)
        {
            if (_angle > 180) _angle -= 360;
            if (_angle > 180) _angleMax -= 360;
            if (_angleMin > 180) _angleMin -= 360;
        }
        _angle = Mathf.Clamp(_angle, _angleMin, _angleMax);
        return _angle;
    }
    #endregion
}
