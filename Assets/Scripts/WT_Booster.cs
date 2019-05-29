using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WT_Booster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WT_Player>()) other.GetComponent<WT_Player>().SetCheckpoint(transform.position);
    }
}
