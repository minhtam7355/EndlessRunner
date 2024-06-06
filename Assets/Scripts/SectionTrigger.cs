using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public ObjectPool pool;
    public Transform[] spawnPoints;
    public Transform Player; // Reference to the Trigger transform
    public float distanceAhead ; // Distance to spawn the new section ahead of the player
    public GameObject[] obstaclesAtMinus5_5; // 3 obstacles
    public GameObject[] obstaclesAtMinus0_5; // 2 obstacles
    public GameObject[] obstaclesAt5_5; // 1 obstacle
    public GameObject[] obstaclesAtMinus7; // 2 obstacles
    public GameObject[] roofObstacle;
    private Queue<GameObject> activeSections = new Queue<GameObject>();
    private Dictionary<GameObject, List<GameObject>> sectionObstacles = new Dictionary<GameObject, List<GameObject>>();

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
        newSection.transform.position = new Vector3(0, 0, Player.position.z + distanceAhead); // You can adjust this to your needs
        newSection.transform.rotation = Quaternion.Euler(0, 90, 0);

        activeSections.Enqueue(newSection);

        // Add obstacles to the new section
        List<GameObject> obstacles = AddObstacles(newSection);
        sectionObstacles.Add(newSection, obstacles);

        // If more than 2 sections are active, deactivate the oldest one
        if (activeSections.Count > 3)
        {
            GameObject oldSection = activeSections.Dequeue();
            List<GameObject> oldObstacles;
            if (sectionObstacles.TryGetValue(oldSection, out oldObstacles))
            {
                foreach (GameObject obstacle in oldObstacles)
                {
                    Destroy(obstacle);
                }
                sectionObstacles.Remove(oldSection);
            }
            pool.ReturnObject(oldSection);
        }
    }

    private List<GameObject> AddObstacles(GameObject section)
    {
        List<GameObject> obstacles = new List<GameObject>();

        // Define the possible positions for the obstacles
        List<Vector3> possiblePositions = new List<Vector3>
    {
        new Vector3(-5.5f, 0, Player.position.z+distanceAhead+35),
        new Vector3(-5.5f, 0, Player.position.z+distanceAhead+15),
        new Vector3(-0.5f, 0, Player.position.z+distanceAhead+35),
        new Vector3(-0.5f, 0, Player.position.z+distanceAhead+15),
        new Vector3(5.5f, 0, Player.position.z+distanceAhead+35),
        new Vector3(5.5f, 0, Player.position.z+distanceAhead+15),
        new Vector3(-7f, 0, Player.position.z+distanceAhead+35),
        new Vector3(-7f, 0, Player.position.z+distanceAhead+35),
        new Vector3(-8.5f, 7, Player.position.z+distanceAhead+15),
        new Vector3(-8.5f, 7, Player.position.z+distanceAhead+35)
    };

        // Shuffle the list to randomize the order of positions
        ShuffleList(possiblePositions);

        int obstacleCount = 0;
        HashSet<float> usedZPositions = new HashSet<float>(); // To store already used z positions

        // Place obstacles at their respective x positions
        foreach (Vector3 position in possiblePositions)
        {
            // If the current position has already been used, skip it
            if (usedZPositions.Contains(position.z))
            {
                continue;
            }

            GameObject[] obstacleArray = null;

            if (position.x == -5.5f)
            {
                obstacleArray = obstaclesAtMinus5_5;
            }
            else if (position.x == -0.5f && position.y == 0)
            {
                obstacleArray = obstaclesAtMinus0_5;
            }
            else if (position.x == -8.5f && position.y == 7)
            {
                obstacleArray = roofObstacle;
                
            }
            else if (position.x == 5.5f)
            {
                obstacleArray = obstaclesAt5_5;
            }
            else if (position.x == -7f)
            {
                obstacleArray = obstaclesAtMinus7;
            }

            if (obstacleArray != null && obstacleArray.Length > 0)
            {
                ShuffleArray(obstacleArray); // Shuffle the obstacle array to choose a random obstacle for this position
                GameObject obstacle = Instantiate(obstacleArray[0], position, Quaternion.Euler(0, 90, 0));
                obstacle.transform.parent = section.transform; // Parent the obstacle to the section
                obstacles.Add(obstacle);
                obstacleCount++;
                usedZPositions.Add(position.z); // Mark the used z position
                if (obstacleCount >= 2) break; // Only place two obstacles per section
            }
        }

        return obstacles;
    }
    private void ShuffleArray(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void ShuffleList(List<Vector3> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
