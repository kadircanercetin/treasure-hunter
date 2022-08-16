using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject FallPoint;
    private Vector3 StartPoint;
    private int currentIndex = 0;
    [SerializeField]
    private float speed = 5f;
    private bool fall = false;
    public GameObject[] rocks;


    private void Start()
    {
        StartPoint = gameObject.transform.position;
        rocks[Random.Range(0, rocks.Length)].SetActive(true);
    }
    void Update()
    {
        if (fall)
        {
            transform.position = Vector3.MoveTowards(transform.position, FallPoint.transform.position, speed * Time.deltaTime);
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(gameObject.transform);
            StartCoroutine(Shaking());
            fall = true;
        }
    }

    private void ResetPosition()
    {
        fall = false;
        transform.position = StartPoint;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(null);
            Invoke("ResetPosition", 4f);
        }
    }
    float duration = 0.1f;
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + (Random.insideUnitSphere * 0.1f);
            yield return null;
        }
        transform.position = startPosition;
    }
}
