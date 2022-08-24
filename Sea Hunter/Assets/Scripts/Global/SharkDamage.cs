using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(100));
            SharkManagerNew manager = GetComponentInParent<SharkManagerNew>();
            if (!manager.canChase)
            {
                manager._speed = 1f;
                GetComponentInParent<SharkManagerNew>().target = GetComponentInParent<SharkManagerNew>().targetOld;
            }
        }
    }

}
