using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
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

    [SerializeField]
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
}
