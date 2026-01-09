using UnityEngine;

public class Door : InteractableItem
{
    [SerializeField] private Key keyToOpen;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] objectsToSpawnOnUnlock;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (GameManager.Instance.ContainsKey(keyToOpen))
            {
                for (int i = 0; i < objectsToSpawnOnUnlock.Length; i++)
                {
                    Instantiate(objectsToSpawnOnUnlock[i], door.transform.position, Quaternion.identity);
                }
                
                Destroy(door);
                Destroy(this.gameObject);
            }
        }
    }*/

    public void OpenDoor()
    {
        if (GameManager.Instance.ContainsKey(keyToOpen))
        {
            for (int i = 0; i < objectsToSpawnOnUnlock.Length; i++)
            {
                Instantiate(objectsToSpawnOnUnlock[i], door.transform.position, Quaternion.identity);
            }

            DestroyImmediate(door);
            DestroyImmediate(this.gameObject);
        }
    }
}
