using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManagerNew : MonoBehaviour
{
    public float _speed = 3f;
    private bool canChase = false;
    private Rigidbody _rigidbody;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canChase)
        {
            transform.LookAt(target);
            _rigidbody.AddRelativeForce(Vector3.forward * _speed, ForceMode.Force);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !canChase)
        {
            canChase = true;
            //target = other.transform;
            GetComponent<Animator>().SetBool("Moving", true);
            target = other.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine).transform;
            StartCoroutine(Chase());
        }

    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<Animator>().SetBool("Moving", false);
        canChase = false;
        target = null;
    }
}
