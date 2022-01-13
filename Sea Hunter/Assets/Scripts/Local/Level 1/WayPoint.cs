using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WayPoints;
public class WayPoint : MonoBehaviour
{
   [SerializeField] private WayPointCircle _waypoint;
    [SerializeField] private Transform[] List;
    // Start is called before the first frame update
    void Start()
    {
        List = _waypoint.getList();
        _waypoint = GetComponentInParent<WayPointCircle>();
    }

   




    // Update is called once per frame
    void Update()
    {
        
    }
}
