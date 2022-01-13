using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ChestDetails : EventManager
    {
    [Tooltip("Gold value indicating the amount of Gold that the player will take when he find this specific chest")]
      [SerializeField]  private int GoldAmount;


    public bool empty; // Check if this chest is empty or not;
    [SerializeField] private string ChestName,Type;

       // Start is called before the first frame update
       public void Start()
        {

        }

    public bool getStatus()
    {
        return empty;
    }

    public void setStatus(bool empty)
    {
        this.empty = empty;
    }


    public int getGold()
    {
        return this.GoldAmount;
    }
        // Update is called once per frame
        void Update()
        {

        }
    }

