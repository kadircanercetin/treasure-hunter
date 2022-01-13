using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Local_CharacterManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject originalPrefab;
    private GameObject SpawnPos, parent;
    [SerializeField] private Vector2 timing ;
    



    [SerializeField] private Vector3 _direction;
    #endregion Variables

    #region Initialize
    // Start is called before the first frame update
    void Start()
    {
        timing.x = .5f;
        timing.y = 1.5f;

        SpawnPos = GameObject.Find("VFX_Source").gameObject;
       
        parent = GameObject.Find("Global_Clone_Parent").gameObject;
        if (originalPrefab != null && SpawnPos != null)
            StartCoroutine(init_bubble());

    }


    IEnumerator init_bubble()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timing.x, timing.y));
            GameObject bubble = Instantiate(originalPrefab, SpawnPos.transform);
            bubble.transform.parent = parent.transform;

        }

    }
    #endregion Initialize

    #region Detect
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            GetComponent<Invector.vCharacterController.vThirdPersonController>().TakeDamage(new Invector.vDamage(100));
        }


        if (other.CompareTag("Jump"))
        {
            print("Jump");

            //transform.Translate(0, force, 0);

            this.gameObject.GetComponent<Rigidbody>().AddForce(_direction,ForceMode.Force);
        }
    }
    #endregion Detect

}
