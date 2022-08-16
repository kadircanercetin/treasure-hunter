using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WayPoints;

public class SharkManagerNew : MonoBehaviour
{
    // Start is called before the first frame update
    public float _speed = 1f, damping;
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private int _target;
    public Transform target;
    public Transform targetOld;

    public bool canChase = true;
    void Awake()
    {
        _wayPoints = gameObject.transform.GetComponentInChildren<WayPointCircle>().Points;
        gameObject.transform.GetComponentInChildren<WayPointCircle>().transform.SetParent(null);
        _target = 0;
        target = _wayPoints[0].transform;
        InvokeRepeating("SwitchTarget", 1f, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target.transform.position, _speed * Time.deltaTime);

        var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void SwitchTarget()
    {
        if (!canChase)
            return;
        _target++;
        if (_target >= _wayPoints.Length)
        {
            _target = 0;
        }
        target = _wayPoints[_target];
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && canChase)
        {
            canChase = false;
            targetOld = target;
            //target = other.transform;
            target = other.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine).transform;
            _speed = 2f;
            //  other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(200));
            StartCoroutine(chase());
        }

    }


    IEnumerator chase()
    {
        yield return new WaitForSeconds(5f);
        _speed = 1f;
        target = targetOld;
        canChase = true;

    }
}
