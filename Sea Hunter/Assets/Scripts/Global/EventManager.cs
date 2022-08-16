using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*using DG.Tweening;*/
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class EventManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private ChestDetails[] chest;
    [SerializeField] private bool[] Completed;
    [SerializeField] private GameObject _indecator, LevelUp;
    [SerializeField] private GameObject[] _flames;
    private bool _clicked,empty,flame;
    [SerializeField] private bool  LevelCompleted;
    private GameObject _Player,infoIndecator;
     private TextMeshProUGUI _playerGoldTxt;
    private int _playerGold;
    private MissionManager _mission;
    #endregion Variables

    #region Initialize

    private void Awake()

    {

        _mission = FindObjectOfType<MissionManager>().GetComponent<MissionManager>();
        GameObject[] allChests = GameObject.FindGameObjectsWithTag("Chest");
        _playerGoldTxt = GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();

        foreach (GameObject gb in allChests)
        {
            if (PlayerPrefs.GetString(gb.name) == "empty")
            {
                gb.GetComponent<Animator>().enabled = false;
                gb.GetComponent<Collider>().enabled = false;
                gb.transform.Find("SM_Prop_Chest_Lid_01").transform.localRotation = new Quaternion(-157.33f, 0, 0, 0);
                gb.transform.Find("SM_Prop_Chest_Treasure_01").gameObject.SetActive(false);
                foreach (ParticleSystem ps in gb.GetComponentsInChildren<ParticleSystem>())
                    ps.Stop();
            }
            else
            {
                gb.GetComponent<Animator>().enabled = true;

            }


        }


        if (PlayerPrefs.HasKey("PlayerGold"))
        {
            _playerGold = PlayerPrefs.GetInt("PlayerGold", 0);

        }
        else
        {
            _playerGold = 0;

        }


        infoIndecator = GameObject.FindGameObjectWithTag("info");
        _playerGoldTxt.text = _playerGold.ToString();


        if (SceneManager.GetActiveScene().buildIndex == 2)
            StartCoroutine(Fade());


        chest = GameObject.FindObjectsOfType<ChestDetails>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //_indecator.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-501f, 70f), 1f);
        _Player = GameObject.FindGameObjectWithTag("Player");


        print(_Player.name);
    }


    #endregion Initialize
    #region Loop


    // Update is called once per frame
    void Update()
    {

        if (getChest() != null && !getChest().GetComponent<ChestDetails>().getStatus())
        {
            //getChest().transform.Find("SM_Prop_Chest_Lid_01").transform.localRotation = Quaternion.Euler(-157.33f, 0, 0);
            //_indecator.GetComponent<RectTransform>().DOAnchorPos(new Vector2(25f, 70f), 1f);
            infoIndecator.SetActive(false);
            _clicked = true;
        }
        else
        {
           // _indecator.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-501f, 70f), .3f);
            infoIndecator.SetActive(true);
            _clicked = false;
        }


        if (_clicked && Input.GetKeyDown(KeyCode.E) && getChest() != null)
        {
            print("Chest Name = " + getChest().name);

            getChest().GetComponent<Animator>().SetBool("Open", true);
            getChest().GetComponent<Collider>().enabled = false;
            StopCoroutine(takeGold());
            StartCoroutine(takeGold());
            getChest().GetComponent<ChestDetails>().setStatus(true);
            _playerGold += getChest().GetComponent<ChestDetails>().getGold(); // Update the player Gold amount

            _playerGoldTxt.text = _playerGold.ToString();
            SaveData(getChest());


        }



    
    }


    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
      
            for (int i = 0; i < chest.Length; i++)
            {

           
      
                if (chest[i].empty)
                    LevelCompleted = true;
                else
                {
                    LevelCompleted = false;
                    break;
                }



               

            }

            if (LevelCompleted)
                SceneManager.LoadScene(2);


        }
    }
    #endregion Loop
    #region Buttons

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Close()
    {
        LevelUp.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerGold += 1000;
        _playerGoldTxt.text = _playerGold.ToString();
        SaveData(null);
    }
    #endregion
    #region Data

    public void SaveData(GameObject _object)
    {
       

        // Save the player gold amount 
        PlayerPrefs.SetInt("PlayerGold", _playerGold);
    }
    #endregion Data
    #region Gold
    public GameObject getChest()
    {
        GameObject chest = _Player.GetComponent<Eyes>()._detect;

        if (chest == null)
            return null;


        if (chest.CompareTag("Chest"))
        {
            return chest;
        }
        return null;
    }


    IEnumerator takeGold()
    {
        yield return new WaitForSeconds(1.5f);







        getChest().GetComponent<Animator>().enabled = false;
        getChest().GetComponent<ChestDetails>().setStatus(true);
        if (!Completed[0])
        {
            _mission.Select(1);
            Slider _slider = GameObject.FindGameObjectWithTag("Mission").GetComponent<Slider>();
            Slider _m1 = GameObject.Find("SilderM1").GetComponent<Slider>();
            _m1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _m1.value + 1 + "/3";
            _m1.value++;
            if (_slider.gameObject.name == "Gold")
            {

                switch (_slider.value)
                {
                    case 0:
                        GameObject.Find("Slider03_Icon1 (1)").GetComponent<Image>().enabled = true;
                        _slider.value += .009f;
                        break;

                    case 0.009f:
                        GameObject.Find("Slider03_Icon1 (2)").GetComponent<Image>().enabled = true;

                        _slider.value = 2f;
                        break;

                    case 2:
                        GameObject.Find("Slider03_Icon1 (3)").GetComponent<Image>().enabled = true;
                        _slider.value = 4f;
                        break;
                }



                if (_slider.value >= 3)
                {
                    Completed[0] = true;

                    LevelUp.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    _slider.value = 0;
                    _mission.Select(2);


                }




            }
        }

        foreach (Transform gb in getChest().GetComponentsInChildren<Transform>())
        {
            if (gb.name != "SM_Prop_Chest_Lid_01" && gb.name != getChest().name)
            {
                gb.gameObject.SetActive(false);



            }

        }


    }
    #endregion Gold

    #region Coroutine
    IEnumerator Fade()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            flame = !flame;
            Switch(flame);
       
        }
        
    }

    #endregion Coroutine

    #region Util
    void Switch(bool _flame)
    {
        print("Length" +_flames.Length);

        if (flame)
        {
            _flames[0].GetComponent<ParticleSystem>().Play();
            _flames[0].GetComponentInChildren<Collider>().enabled = true;

            _flames[1].GetComponent<ParticleSystem>().Stop();
            _flames[1].GetComponentInChildren<Collider>().enabled = false;


            _flames[2].GetComponentInChildren<Collider>().enabled = true;
            _flames[2].GetComponent<ParticleSystem>().Play();
          
       
        }
        else
        {
            _flames[0].GetComponent<ParticleSystem>().Stop();
            _flames[0].GetComponentInChildren<Collider>().enabled = false;

            _flames[1].GetComponent<ParticleSystem>().Play();
            _flames[1].GetComponentInChildren<Collider>().enabled = true;


            _flames[2].GetComponentInChildren<Collider>().enabled = false;
            _flames[2].GetComponent<ParticleSystem>().Stop();
        }
    }
    #endregion Util

}
