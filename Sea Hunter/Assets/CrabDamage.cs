using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(50));
            GetComponent<SkinnedMeshRenderer>().enabled = false;
            var par = GetComponentsInChildren<ParticleSystem>();
            foreach (var pS in par)
            {
                pS.Play();
            }
            Destroy(transform.parent.gameObject, 1f);
        }
    }
}
