using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelController : MonoBehaviour
{
    //level fields
    public int NextLevelIndex = 1;
    private Enemy[] _enemies;
    
    //animation fields
    public Animator transition;
    public float transitionTime = 1f;

    [SerializeField] private bool isMenu = false;

    private void OnEnable()
    {
        _enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenu)
            return;

        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                return;
        }
        Debug.Log("You killed all enemies!");

        NextLevelIndex++;
        string nextLevelName = "Level" + NextLevelIndex;

        if (NextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No more levels!");
            return;
        }

        LoadNextLevel(nextLevelName);
    }

    public void LoadNextLevel(string levelName)
    {
        Debug.Log("LEVELED UP!! " + NextLevelIndex);
        StartCoroutine(LoadLevel(levelName));
    }

    public IEnumerator LoadLevel(string levelName)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load
        SceneManager.LoadScene(levelName);
    }
}