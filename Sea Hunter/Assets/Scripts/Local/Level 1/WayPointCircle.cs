using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WayPoints
{
    public class WayPointCircle : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform[] Points;

        void Start()
        {
            Points = GetComponentsInChildren<Transform>();
        }


       public Transform[] getList()
        {
            return Points;
        }

       public int getIndex(string name)
        {
            for (int i = 1; i < Points.Length; i++)
            {
                if (Points[i].name == name)
                    return i;

            }

            return -1;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

