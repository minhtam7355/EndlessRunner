using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public ObjectPool pool;
    public Transform[] spawnPoints;
    private Queue<GameObject> activeSections = new Queue<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            SpawnSection();
        }
    }

    private void SpawnSection()
    {
        GameObject newSection = pool.GetObject();
        newSection.transform.position = new Vector3(0, 0, 42); // You can adjust this to your needs
        newSection.transform.rotation = Quaternion.Euler(0, 90, 0);

        activeSections.Enqueue(newSection);

        // If more than 2 sections are active, deactivate the oldest one
        if (activeSections.Count > 2)
        {
            GameObject oldSection = activeSections.Dequeue();
            pool.ReturnObject(oldSection);
        }
    }
}
