using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
public class UIManager : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject settingMenu;

    [SerializeField] private GameObject missionMenu;

    public GameObject levelCleared;
    public GameObject missionText;
    public Slider treasureSlider;
    public Slider milestoneSlider;
    public GameObject[] milestoneObject;
    public int milestoneCount;
    public GameObject playerUI;

    bool wait;
    public float menuDropSpeed;

    public GameObject[] hints;
    #endregion Variables


    #region Initialize
    // Start is called before the first frame update
    void Start()
    {
        milestoneCount = -1;
        settingMenu.SetActive(false);
        missionMenu.SetActive(false);
    }
    #endregion Initialize

    #region Buttons
    public void TurnMenuOn(GameObject menu)
    {
        wait = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.SetActive(true);
    }
    public void CloseTab(GameObject menu)
    {
        wait = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menu.SetActive(false);
        missionMenu.SetActive(false);
        settingMenu.SetActive(false);
    }
    #endregion Buttons

    #region Coroutine
    IEnumerator CloseBg()
    {
        yield return new WaitForSeconds(2f);
        missionMenu.SetActive(false);
        settingMenu.SetActive(false);
    }
    #endregion Coroutine

    #region Loop
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !wait)
        {
            CloseTab(missionMenu);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !wait)
        {
            CloseTab(settingMenu);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !missionMenu.activeInHierarchy)
        {
            TurnMenuOn(settingMenu);
        }
        else if (Input.GetKeyDown(KeyCode.M) && !settingMenu.activeInHierarchy)
        {
            TurnMenuOn(missionMenu);
        }
       
    }

    private void PositionMenu(GameObject gameObject)
    {
        if (wait && (gameObject.transform.localPosition.y < 1200))
        {
            gameObject.transform.localPosition -= new Vector3(0f, -1f * menuDropSpeed, 0f);
        }
        else if (!wait && (gameObject.transform.localPosition.y > 0))
        {
            gameObject.transform.localPosition += new Vector3(0f, -1f * menuDropSpeed, 0f);
        }
    }

    public void UpdateMileStone()
    {
        milestoneCount += 1;
        if (milestoneCount == 0)
        {
            milestoneObject[milestoneCount].GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }
        else
        {
            GetComponent<UIManager>().milestoneSlider.value += 33;
            milestoneObject[milestoneCount].GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }


    }

    public void TurnOffHints()
    {
        StartCoroutine(TurnOffGameObject(hints[0], 5f));
        StartCoroutine(TurnOffGameObject(hints[1], 10f));
    }

    IEnumerator TurnOffGameObject(GameObject gameObject,float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }

    #endregion Loop
}
