using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    #region Variables
    [HideInInspector] public GameObject _detect;
    #endregion Variables

    #region Detect
    private void OnTriggerEnter(Collider other)
    {
        _detect = other.transform.gameObject;
 
    }


    private void OnTriggerExit(Collider other)
    {
        _detect = null;
    }


    #endregion Detect
}
