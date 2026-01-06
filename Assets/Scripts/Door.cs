using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Key keyToOpen;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (GameManager.Instance.ContainsKey(keyToOpen))
            {
                Destroy(door);
                Destroy(this.gameObject);
            }
        }
    }
}
