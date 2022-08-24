using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIController : MonoBehaviour
{
    public GameObject Title;
    public GameObject Settings;
    public GameObject Quit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey && Title.activeInHierarchy)
        {
            Title.SetActive(false);
            Settings.SetActive(true);
        }
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitYes()
    {
        Application.Quit();
    }
}
