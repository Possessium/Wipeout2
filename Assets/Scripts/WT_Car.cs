using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Car : MonoBehaviour
{
    #region Fields
    [SerializeField] WT_CarCharacteristics carCharacteristics = new WT_CarCharacteristics();
        public WT_CarCharacteristics CarCharacteristics { get { return carCharacteristics; } }
    [SerializeField] float hoverStrength = 2;
    [SerializeField] LayerMask groundLayer = 0;
    [SerializeField] Transform frontPos = null;
    [SerializeField] Transform backPos = null;
    [SerializeField] float maxDistance = 2;
    [SerializeField, Range(0, 90)] float clamp = 30;
    [SerializeField] float currentSpeed = 0;
    float fall = 0;
    float distance = 0;
    #endregion


    #region Unity methods
    private void Start()
    {
        distance = maxDistance;
        WT_InputManager.OnTurn += Rotate;
        WT_InputManager.OnNoTurn += ResetTorque;
        WT_InputManager.OnAccelerate += Accelerate;
        WT_InputManager.OnBrake += Brake;
        WT_InputManager.OnNoSpeed += SlowDown;
    }

    private void Update()
    {
        Hover();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(frontPos.position, Vector3.down * distance);
        Gizmos.DrawRay(backPos.position, Vector3.down * distance);
        Gizmos.DrawRay(transform.position, Vector3.down * distance);
    }
    #endregion

    #region Custom methods
    void Hover()
    {
        RaycastHit _hitGroundFront;
        RaycastHit _hitGroundMiddle;
        RaycastHit _hitGroundBack;
        bool _frontHit = Physics.Raycast(frontPos.position, Vector3.down, out _hitGroundFront, distance, groundLayer);
        bool _middleHit = Physics.Raycast(transform.position, Vector3.down, out _hitGroundMiddle, distance, groundLayer);
        bool _backHit = Physics.Raycast(backPos.position, Vector3.down, out _hitGroundBack, distance, groundLayer);
        if (_frontHit || _middleHit || _backHit)
        {
            distance = Mathf.PingPong(Time.time * hoverStrength, maxDistance) + transform.localScale.y;
            fall = 0;
        }
        if (_middleHit)
        {
            transform.position = new Vector3(transform.position.x, _hitGroundMiddle.point.y + distance, transform.position.z);
        }
        else if (_frontHit)
        {
            transform.position = new Vector3(transform.position.x, _hitGroundFront.point.y + distance, transform.position.z);
        }
        else if (_backHit)
        {
            transform.position = new Vector3(transform.position.x, _hitGroundBack.point.y + distance, transform.position.z);
        }
        else Fall();
    }
    void Fall()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), Time.deltaTime * fall);
        fall += .5f;
        fall = Mathf.Clamp(fall, 0, 500);
    }

    void Rotate(float _x)
    {
        transform.localEulerAngles += new Vector3(0, _x, 0) * carCharacteristics.RotationSpeed;
        transform.localEulerAngles += new Vector3(0, 0, _x) * Time.deltaTime * 50;
        Vector3 _toClamp = transform.localEulerAngles;
        _toClamp.z = WT_Camera.ClampAngle(_toClamp.z, -clamp, clamp);
        transform.localEulerAngles = _toClamp;
    }
    void ResetTorque()
    {
        Vector3 _angle = AngleLerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), Time.deltaTime);
        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, _angle, Time.deltaTime * 50);
    }
    Vector3 AngleLerp(Vector3 _startAngle, Vector3 _finishAngle, float _t)
    {
        float _xLerp = Mathf.LerpAngle(_startAngle.x, _finishAngle.x, _t);
        float _yLerp = Mathf.LerpAngle(_startAngle.y, _finishAngle.y, _t);
        float _zLerp = Mathf.LerpAngle(_startAngle.z, _finishAngle.z, _t);
        Vector3 _lerped = new Vector3(_xLerp, _yLerp, _zLerp);
        return _lerped;
    }

    void Accelerate(float _speed)
    {
        currentSpeed += Time.deltaTime * carCharacteristics.Acceleration / 50;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, carCharacteristics.MaxSpeed / 50);
        transform.position += transform.forward * currentSpeed;
    }
    void Brake(float _speed)
    {
        currentSpeed -= Time.deltaTime * carCharacteristics.Acceleration / 25;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, carCharacteristics.MaxSpeed / 50);
        transform.position += transform.forward * currentSpeed;
    }
    void SlowDown()
    {
        if (currentSpeed > 0) currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * carCharacteristics.Acceleration / 50);
    }
    #endregion
}
