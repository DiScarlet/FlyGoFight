using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private static int _nextLevelIndex = 1;
    private Enemy[] _enemies;

    private void OnEnable()
    {
        _enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                return;
        }
        Debug.Log("You killed all enemies!");

        _nextLevelIndex++;
        string nextLevelName = "Level" + _nextLevelIndex;

        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No more levels!");
            return;
        }

        SceneManager.LoadScene(nextLevelName);
    }
}
