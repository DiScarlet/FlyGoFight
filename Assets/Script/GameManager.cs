using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string LastLevelName;

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
