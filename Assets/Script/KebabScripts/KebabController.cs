using UnityEngine;
using TMPro;

public class KebabController : MonoBehaviour
{
    public static KebabController Instance;

    [SerializeField] private TextMeshProUGUI kebabCountText;

    private int kebabCount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        kebabCount = 0;
        UpdateUI();
    }

    public void AddKebab(int amount = 1)
    {
        kebabCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
       kebabCountText.text = kebabCount.ToString();
    }
}
