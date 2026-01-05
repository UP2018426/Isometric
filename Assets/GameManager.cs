using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Ammo"), SerializeField]
    private int ammunition;
    public int Ammunition
    {
        get
        {
            return ammunition;
        }
        set
        {
            ammunition = value;
            UpdateAmmoCounter();
        }
    }

    public TextMeshProUGUI ammoText;

    [Header("Score"), SerializeField]
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UpdateScoreCounter();
        }
    }

    public TextMeshProUGUI scoreText;

    [Header("Health"), SerializeField]
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            UpdateHealthCounter();
        }
    }

    public TextMeshProUGUI healthText;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UpdateAmmoCounter();
        UpdateScoreCounter();
        UpdateHealthCounter();
    }

    public void UpdateAmmoCounter()
    {
        if(ammoText == null)
        {
            Debug.LogWarning("Ammo counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        ammoText.text = Ammunition.ToString();
    }

    public void UpdateScoreCounter()
    {
        if (scoreText == null)
        {
            Debug.LogWarning("Score counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        scoreText.text = Score.ToString();
    }

    public void UpdateHealthCounter()
    {
        if (healthText == null)
        {
            Debug.LogWarning("Score counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        healthText.text = Health.ToString();
    }
}
