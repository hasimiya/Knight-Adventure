using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetScoreUI();
        GetHealthUI();
    }
    public void GetScoreUI()
    {
        scoreText.text = "Score: " + Player.Instance.GetScore();
    }
    public void GetHealthUI()
    {
        healthText.text = "Health: " + Player.Instance.GetCurrentHealth();
    }
}
