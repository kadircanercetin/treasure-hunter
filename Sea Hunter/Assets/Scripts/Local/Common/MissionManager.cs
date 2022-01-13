using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MissionManager : MonoBehaviour
{

    [SerializeField] private GameObject[] _indecator;
    [SerializeField] private GameObject label; // Press M to start mission;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gb in _indecator)
            gb.SetActive(false);
    }

    public void Select(int i)
    {
   

            foreach (GameObject gb in _indecator)
                gb.SetActive(false);


            label.SetActive(false);


        if (i == 1)
        {
            _indecator[0].SetActive(true);
           
        }else if(i == 2)
        {
            _indecator[0].SetActive(false);
        }
        
        
        else
           _indecator[int.Parse(EventSystem.current.currentSelectedGameObject.name)].SetActive(!_indecator[int.Parse(EventSystem.current.currentSelectedGameObject.name)].activeInHierarchy);
       

       
    }


    void FixedUpdate()
    {
     
    }
}
