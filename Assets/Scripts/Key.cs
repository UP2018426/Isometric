using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] float magnetDistance = 1f;
    [SerializeField] float collectionDistance = 0.2f;
    [SerializeField] float collectionSpeed = 0.3f;

    float collectionTime;

    bool isWithinDistance = false;
    Transform playerTransform;
    
    void Awake()
    {
        playerTransform = FindAnyObjectByType<ThirdPersonController>().gameObject.transform;
    }

    void Update()
    {
        if ((transform.position - playerTransform.position).magnitude < magnetDistance)
        {
            isWithinDistance = true;
        }

        if (isWithinDistance == true)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, collectionTime);
            collectionTime += collectionSpeed * Time.deltaTime;
        }

        if ((transform.position - playerTransform.position).magnitude < collectionDistance)
        {
            KeyCollected();
        }
    }

    void KeyCollected()
    {
        GameManager.Instance.CollectKey(this);
        
        Destroy(this.gameObject);
    }
}
