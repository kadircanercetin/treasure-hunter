using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    #region Variables

    [SerializeField] private string ID;
    [SerializeField] private float speed;

    private Transform Target;
    bool startChasing,exploid, _attack,dead, charging;


    #endregion Variables
  

    #region Loop
    // Update is called once per frame
    void FixedUpdate()
    {

     

        switch (ID)
        {
            case "0":

                if (startChasing)
                    chase(Target);
                else
                    StopChasing();

                break;

            case "1":
                if (exploid)
                {
                    Exploid();
                }
               
                break;
        }

       
    }


    #endregion Loop


    #region Attack
    public void Attack()
    {
        if (!dead)
        {
            startChasing = false;
            _attack = true;
            if (this.gameObject.CompareTag("Demon"))
            {
                GetComponent<Animator>().SetTrigger("Melee Attack 01");


            }else if(this.gameObject.CompareTag("Spider"))
                GetComponent<Animator>().SetTrigger("Claw Attack");

            

        }


    }

    public void Damage(int _damage)
    {

     

      
            GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(_damage));
    }

    public void EndAttack()
    {
        _attack = false;
        print("EndAttack");

        startChasing = true;
    }

    public void Exploid()
    {
        if(!charging)
        {
            Target.GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(70));
            transform.GetComponentInChildren<ParticleSystem>().Play();
            transform.GetComponentInChildren<AudioSource>().Play();
            charging = true;
            StopCoroutine(_charging());
            StartCoroutine(_charging());
        }
     


    }


    IEnumerator _charging()
    {
        yield return new WaitForSeconds(5f);
        charging = false;
    }
    #endregion Attack



    #region Chase And Define

    public void chase(Transform _target)
    {
        if (!_attack && !dead)
        {
            Vector3 TargetY = _target.position;
            TargetY.y = transform.position.y;
            this.gameObject.transform.position = Vector3.Lerp(transform.position, TargetY, speed);
            GetComponent<Animator>().SetBool("Walk", true);
            StartCoroutine(_stopChaing());



            transform.LookAt(_target);
        }


    }

    IEnumerator _stopChaing()
    {
        yield return new WaitForSeconds(40f);
        startChasing = false;
      //  Target = null;

    }

   
    public void Die()
    {
        startChasing = false;
        //Target = null;
        speed = 0;
        dead = true;
        GetComponent<Animator>().SetTrigger("Die");

        Destroy(this.gameObject, 6f);
    }

   

    public void StopChasing()
    {
        GetComponent<Animator>().SetBool("Walk", false);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = other.transform;
           
            startChasing = true;
            exploid = true; 
           
        }



        
          
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exploid = false;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")&& ID == "1" )
        {
            Damage(1);
        }

     
    }

    #endregion Chase And Define

}
