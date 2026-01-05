using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
