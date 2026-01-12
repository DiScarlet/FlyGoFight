using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string LastLevelName;
    public int BirdsLeft = 3;
    public bool AllKebabsGained = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
