using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wayPoints;
    private int currentIndex = 0;
    [SerializeField]
    private float speed = 1f;
    public GameObject[] rocks;
    public bool canMove;
    private void Start()
    {
        canMove = true;
        rocks[Random.Range(0, rocks.Length)].SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;
        if (Vector3.Distance(transform.position, wayPoints[currentIndex].transform.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= wayPoints.Length)
            {
                currentIndex = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentIndex].transform.position, speed * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "head")
        {
            canMove = false;
        }
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(gameObject.transform);
        }
    }


    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "head")
        {
            canMove = true;
        }
        if (other.transform.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
