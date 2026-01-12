using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public partial class MidLevelController : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    [SerializeField] private TextMeshProUGUI textLabel;
    private string levelName;
    private int levelNumber;

    private void Awake()
    {
        levelName = GameManager.Instance.LastLevelName;
        string prefix = "Level";
        levelNumber = Convert.ToInt32(levelName.Substring(prefix.Length));

        Debug.Log("LABEL " + levelNumber);

        textLabel.text = "1-" + levelNumber;
    }
    public void OnToLevelMenu()
    {
        string levelMenu = "LevelMenu";
        StartCoroutine(levelController.LoadLevel(levelMenu));
    }
    public void OnRestartLevel()
    {
        StartCoroutine(levelController.LoadLevel(levelName));
    }
    public void OnToNextLevel()
    {
        string nextLevel = "Level" + (levelNumber + (int)1);
        Debug.Log("LOADING LEVEL " + nextLevel);
        StartCoroutine(levelController.LoadLevel(nextLevel));
    }
} 
