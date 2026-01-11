using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public partial class MidLevelController : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    [SerializeField] private TextMeshProUGUI textLabel;
    private string levelName;

    private void Awake()
    {
        levelName = GameManager.Instance.LastLevelName;
        string prefix = "Level";
        string levelNumber = levelName.Substring(prefix.Length);

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
} 
