using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public partial class MidLevelController : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    [SerializeField] private TextMeshProUGUI textLabel;

    //stars
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private GameObject _starParticlesPrefab;

    private string levelName;
    private int levelNumber;
    public bool isWinMenu = false; 

    private void Awake()
    {
        //assign level number to the label
        levelName = GameManager.Instance.LastLevelName;
        string prefix = "Level";
        levelNumber = Convert.ToInt32(levelName.Substring(prefix.Length));
        textLabel.text = "1-" + levelNumber;

        //if is win menu show stars
        if (isWinMenu)
        {
            star1.gameObject.SetActive(false);
            star2.gameObject.SetActive(false);
            star3.gameObject.SetActive(false);
            StartCoroutine(ShowStarsRoutine());
        }
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
        StartCoroutine(levelController.LoadLevel(nextLevel));
    }

    private IEnumerator ShowStarsRoutine()
    {
        //1 star if passed the level, 2 stars if all birds are left at the end or kebab was gained,
        //3 stars if all birds + all kebabs
        bool allKebabsGained = GameManager.Instance.AllKebabsGained;
        int birdsLeft = GameManager.Instance.BirdsLeft;
        Debug.Log("k: " + allKebabsGained + " birds " + birdsLeft);
       
        //star 1
        yield return new WaitForSeconds(1.5f);
        star1.gameObject.SetActive(true);
        SpawnParticles(star1);  

        //star2
        if(birdsLeft == 2 || allKebabsGained)
        {
            yield return new WaitForSeconds(1.5f);
            star2.gameObject.SetActive(true);
            SpawnParticles(star2);
        }

        if (birdsLeft == 2 && allKebabsGained)
        {
            yield return new WaitForSeconds(1.5f);
            star3.gameObject.SetActive(true);
            SpawnParticles(star3);
        }
    }

    private void SpawnParticles(Image star)
    {
        //enable stars prefab in selected location
        if (_starParticlesPrefab == null)
            return;

        Instantiate(_starParticlesPrefab, star.transform.position, Quaternion.identity);
    }
} 
