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
        levelName = levelName.Substring(prefixLength, levelName.Length - prefixLength);

        SceneManager.LoadScene(levelName);
    }
}
