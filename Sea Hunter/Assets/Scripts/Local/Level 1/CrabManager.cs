using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    [SerializeField] private string ID;
    private bool exploid,follow;
    private float speed;
    private Transform _target;
    private Collider _eyes;

    [SerializeField] private float index;
    // Start is called before the first frame update
    void Awake()
    {

    

        switch (ID)
        {
            case "0":
                exploid = true;

                break;

            case "1":
                follow = true;
                speed = 0;
               
                break;

        }

       



      
    }

    // Update is called once per frame
    void Update()
    {

    

        if (_target!= null)
        {
            this.transform.position = Vector3.Lerp(transform.position, new Vector3(_target.position.x, _target.position.y + index, _target.position.z), speed);
           // GetComponent<Animator>().SetBool("Following", true); ;
            transform.LookAt(_target);
        }
        else
        {
            speed = 0;
            GetComponent<Animator>().SetBool("Following", false);
        }
        

    }







    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Exploid");
            if (exploid)
            {
                print("Exploid");
                foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                    if(ps.transform.name == "Bomb")
                    {
                        ps.Play();
                        ps.GetComponent<AudioSource>().Play();
                    }
                       
                


                other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(150));
                // end foreach;


            }else if (follow)
            {
                speed = 0.005f;
               
                _target = other.transform;
                other.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(50));
                StopCoroutine(chase());
                StartCoroutine(chase());
            
            
            }
            else
            {
                foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                    if (ps.transform.name == "Happy")
                        ps.Play();
            }
        }
    }
    

    IEnumerator chase()
    {
        yield return new WaitForSeconds(7f);
        speed = 0;
    }

}
