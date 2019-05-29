using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Player : MonoBehaviour
{
    #region Fields
    [SerializeField] WT_Car currentCar = null;
    [SerializeField] Vector3 lastCheckpoint = Vector3.zero;
    [SerializeField] bool isBooster = false;
    #endregion


    #region Events

    #endregion


    #region Unity methods
    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    #region Custom methods
    void Control()
    {

    }

    void CheckFall()
    {

    }

    void Respawn()
    {

    }

    public void SetCheckpoint(Vector3 _pos) => lastCheckpoint = _pos;

    public void BoosterSpeedUp()
    {
        StopCoroutine(BoosterDelay());
        isBooster = true;
        StartCoroutine(BoosterDelay());
    }

    IEnumerator BoosterDelay()
    {
        yield return new WaitForSeconds(currentCar.CarCharacteristics.BoostTime);
        isBooster = false;
    }
    #endregion
}
