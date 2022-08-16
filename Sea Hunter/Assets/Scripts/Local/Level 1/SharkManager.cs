using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed, damping;
    [SerializeField] private GameObject[] _wayPoints;
    [SerializeField] private int _target;

    private Transform target;
    void Awake()
    {
        _wayPoints = GameObject.FindGameObjectsWithTag("Point");
        _speed = 0.004f;
        _target = 1;
        target = _wayPoints[1].transform;
    }







    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.position = Vector3.Lerp(transform.position, target.position, _speed);


        var rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    // transform.LookAt(_wayPoints[_target].transform);



    void OnTriggerEnter(Collider other)
    {

        print("OK");

        if (other.CompareTag("Point"))
        {
            if (_target < _wayPoints.Length)
            {
                _target = int.Parse(other.name) + 1;
                target = _wayPoints[_target].transform;
            }
            else
            {
                _target = 0;
                target = _wayPoints[_target].transform;
            }

            _speed = 0.004f;


        }

        if (other.CompareTag("Player"))
        {
            target = other.transform;


            _speed = 0.01f;
            other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(20));
            StopCoroutine(chase());
            StartCoroutine(chase());
        }

    }


    IEnumerator chase()
    {
        yield return new WaitForSeconds(5f);

        target = _wayPoints[0].transform;

    }
}