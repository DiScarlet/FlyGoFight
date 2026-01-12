using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelController : MonoBehaviour
{
    //level fields
    public int NextLevelIndex = 1;
    private Enemy[] _enemies;
    private KebabReputation[] _kebabs;

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
        _kebabs = FindObjectsOfType<KebabReputation>();

        NextLevelIndex++;
        string nextLevelName = "Level" + NextLevelIndex;

        if (NextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            return;
        }

        LoadNextLevel(nextLevelName);
    }

    public void LoadNextLevel(string levelName)
    {
        GameManager.Instance.LastLevelName = SceneManager.GetActiveScene().name;
        GameManager.Instance.AllKebabsGained = _kebabs.Length == 0 ? true : false;

        string winMenu = "WinMenu";
        StartCoroutine(LoadLevel(winMenu));
        return;
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