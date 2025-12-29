using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] int scoreValue;
    [SerializeField] float magnetDistance;
    [SerializeField] float collectionDistance;
    [SerializeField] float collectionSpeed = 0.7f;

    float collectionTime;

    bool isWithinDistance = false;
    Transform playerTransform;

    void Awake()
    {
        playerTransform = FindAnyObjectByType<ThirdPersonController>().gameObject.transform;
    }

    // Update is called once per frame
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
            CoinCollected();
        }
    }

    void CoinCollected()
    {
        // Modify this as needed for future use with a score system
        Destroy(this.gameObject);
    }
}
