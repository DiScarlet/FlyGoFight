using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelMenuController : MonoBehaviour
{
    public void OnLevelClick()
    {
        const int prefixLength = 6;

        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        if (clickedButton == null)
            return;

        string levelName = clickedButton.name;
        Debug.Log("NAME " + levelName);
        levelName = levelName.Substring(prefixLength, levelName.Length - prefixLength);

        Debug.Log("RELOCATING TO " + levelName);
        SceneManager.LoadScene(levelName);
    }//2d secene transitions Unity
}
