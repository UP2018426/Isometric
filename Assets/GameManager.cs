using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Transform playerTransform;

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

    // Keys
    [Header("ItemCollection"), SerializeField] private float maxKeyCollectionDistance;
    [SerializeField] private GameObject callToAction;

    [SerializeField] private Key[] keysInSceneList;
    private List<Key> collectedKeysList = new List<Key>();

    [Header("Input Actions")]
    public InputActionReference interactAction;

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
        FindAllKeysInCurrentScene();
        FindPlayer();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        KeyUpdate();
    }

    private void UpdateAmmoCounter()
    {
        if(ammoText == null)
        {
            Debug.LogWarning("Ammo counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        ammoText.text = Ammunition.ToString();
    }

    private void UpdateScoreCounter()
    {
        if (scoreText == null)
        {
            Debug.LogWarning("Score counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        scoreText.text = Score.ToString();
    }

    private void UpdateHealthCounter()
    {
        if (healthText == null)
        {
            Debug.LogWarning("Score counter TextMeshPro component has not been found. \nMake sure it has been assigned on the GameManager!");
            return;
        }

        healthText.text = Health.ToString();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        FindPlayer();
        FindAllKeysInCurrentScene();
    }

    private void FindPlayer()
    {
        playerTransform = FindAnyObjectByType<ThirdPersonController>().gameObject.transform;
    }

    private void FindAllKeysInCurrentScene()
    {
        // Find all keys in the scene
        keysInSceneList = (Key[])GameObject.FindObjectsByType(typeof (Key), FindObjectsSortMode.None);
    }

    private Key FindClosestKey()
    {
        if (keysInSceneList.Length <= 0)
        {
            Debug.Log("No Keys can be found in scene");
            return null;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < keysInSceneList.Length; i++)
        {
            float distanceToKey = (keysInSceneList[i].transform.position - playerTransform.transform.position).magnitude;
            if (distanceToKey < closestDistance)
            {
                closestDistance = distanceToKey;
                closestIndex = i;
            }
        }

        return keysInSceneList[closestIndex];
    }

    private void KeyUpdate()
    {
        Key closestKey = FindClosestKey();
        if (closestKey == null)
        {
            return;
        }

        float distanceToClosestKey = (closestKey.transform.position - playerTransform.transform.position).magnitude;
        if (distanceToClosestKey < maxKeyCollectionDistance)
        {
            // Show CTA
            callToAction.SetActive(true);

            // Update the position of the CTA to make sure that it shows on the collectible item.
            callToAction.transform.position = closestKey.transform.position;

            if (interactAction.action.WasPressedThisFrame())
            {
                CollectKey(closestKey);
            }
        }
        else
        {
            // Hide the CTA
            callToAction.SetActive(false);
        }
    }

    public void CollectKey(Key keyToAdd)
    {
        collectedKeysList.Add(keyToAdd);

        Destroy(keyToAdd.gameObject);

        FindAllKeysInCurrentScene(); // We update the keys in the scene to make sure there isnt a null object in the array
    }

    public bool ContainsKey(Key keyToCheck)
    {
        if (collectedKeysList.Contains(keyToCheck))
        {
            return true;
        }

        return false;
    }
}
