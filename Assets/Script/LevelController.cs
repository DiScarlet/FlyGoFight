using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class LevelController : MonoBehaviour
{
    //level fields
    private static int _nextLevelIndex = 1;
    private Enemy[] _enemies;

    //animation fields
    public Animator transition;
    public float transitionTime = 1f;

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

        LoadNextLevel(nextLevelName);
       // SceneManager.LoadScene(nextLevelName);
    }

    public void LoadNextLevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load
        SceneManager.LoadScene(levelName);
    }
}
