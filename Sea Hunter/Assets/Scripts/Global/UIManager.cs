using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
public class UIManager : MonoBehaviour
{

    #region Variables
    private GameObject _settingMenu;

    [SerializeField] private GameObject background;
    bool wait;

    #endregion Variables


    #region Initialize
    // Start is called before the first frame update
    void Start()
    {
      
        _settingMenu = GameObject.FindGameObjectWithTag("Setting");
        background.SetActive(false);
        print("Position  "+_settingMenu.GetComponent<RectTransform>().position);
        //_settingMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1112f), 1.5f);
    }
    #endregion Initialize

    #region Buttons
    public void Setting()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       
        background.SetActive(true);
        background.GetComponent<Animator>().SetBool("Active", true);
        //_settingMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 28f), 1.5f);
    }
    public void Close()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //_settingMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 1112f), 1.5f);
        background.GetComponent<Animator>().SetBool("Active", false);


        StopCoroutine(CloseBg());
        StartCoroutine(CloseBg());
    }
    #endregion Buttons

    #region Coroutine
    IEnumerator CloseBg()
    {
        yield return new WaitForSeconds(.7f);
        background.SetActive(false);

    }
    #endregion Coroutine


    #region Loop
    // Update is called once per frame
    void Update()
    {


        if (!background.activeInHierarchy && Input.GetKeyDown(KeyCode.M))
        {
            Setting();
            wait = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && background.activeInHierarchy && !wait)
        {
            wait = true;
            Close();
        }
    }

    #endregion Loop
}
