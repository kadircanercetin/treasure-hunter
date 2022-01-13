using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class LevelSelection : MonoBehaviour
{

    #region Level
    public void SelectLevel()
    {
        SceneManager.LoadSceneAsync(int.Parse(EventSystem.current.currentSelectedGameObject.name));
    }
    #endregion Level


}
