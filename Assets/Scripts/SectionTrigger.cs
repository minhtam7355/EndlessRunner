using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public ObjectPool sectionPool;
    public CoinPool coinPool;
    public EnemyPool enemyPool; // Reference to the enemy pool
    public Transform Player; // Reference to the player transform
    public float distanceAhead; // Distance to spawn the new section ahead of the player
    public GameObject[] obstaclesAtMinus5_5; // 3 obstacles
    public GameObject[] obstaclesAtMinus0_5; // 2 obstacles
    public GameObject[] obstaclesAt5_5; // 1 obstacle
    public GameObject[] obstaclesAtMinus7; // 2 obstacles
    public GameObject[] roofObstacle; // Roof obstacles

    private Queue<GameObject> activeSections = new Queue<GameObject>();
    private Dictionary<GameObject, List<GameObject>> sectionObstacles = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, List<GameObject>> sectionCoins = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, List<GameObject>> sectionEnemies = new Dictionary<GameObject, List<GameObject>>(); // To track enemies

    private void Start()
    {
        sectionPool = GameObject.FindGameObjectWithTag("SectionPool").GetComponent<ObjectPool>();
        coinPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<CoinPool>();
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<EnemyPool>(); // Initialize enemy pool
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        InitializeObstacles();
    }

    private void InitializeObstacles()
    {
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        obstaclesAtMinus5_5 = allObstacles.Where(go => go.name == "ObstacleMiddleJump" || go.name == "ObstacleLeftJump" || go.name == "ObstacleRightJump").ToArray();
        obstaclesAtMinus0_5 = allObstacles.Where(go => go.name == "ObstacleRollMiddle").ToArray();
        obstaclesAt5_5 = allObstacles.Where(go => go.name == "ObstacleRollRight").ToArray();
        obstaclesAtMinus7 = allObstacles.Where(go => go.name == "ObstacleRollLeft" || go.name == "ObstacleJump").ToArray();
        roofObstacle = allObstacles.Where(go => go.name == "RoofObstacle").ToArray();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            SpawnSection();
        }
    }

    private void SpawnSection()
    {
        GameObject newSection = sectionPool.GetObject();
        if (newSection == null)
        {
            Debug.LogError("Failed to get a new section from the pool.");
            return;
        }

        newSection.transform.position = new Vector3(0, 0, Player.position.z + distanceAhead);
        newSection.transform.rotation = Quaternion.Euler(0, 90, 0);

        if (activeSections.Contains(newSection))
        {
            Debug.LogWarning("Section already exists in the queue!");
            return;
        }

        activeSections.Enqueue(newSection);

        List<GameObject> obstacles = AddObstacles(newSection);
        sectionObstacles[newSection] = obstacles;

        List<GameObject> coins = SpawnCoins(newSection);
        sectionCoins[newSection] = coins ?? new List<GameObject>();

        List<GameObject> enemies = SpawnEnemies(newSection); // Spawn enemies
        sectionEnemies[newSection] = enemies ?? new List<GameObject>();

        // Deactivate oldest section if more than 4 sections are active
        if (activeSections.Count > 4)
        {
            GameObject oldSection = activeSections.Dequeue();
            CleanupOldSection(oldSection);
        }
    }

    private void CleanupOldSection(GameObject oldSection)
    {
        if (sectionObstacles.TryGetValue(oldSection, out List<GameObject> oldObstacles))
        {
            foreach (GameObject obstacle in oldObstacles)
            {
                if (obstacle != null)
                {
                    Destroy(obstacle);
                }
            }
            sectionObstacles.Remove(oldSection);
        }

        if (sectionCoins.TryGetValue(oldSection, out List<GameObject> oldCoins))
        {
            foreach (GameObject coin in oldCoins)
            {
                if (coin != null)
                {
                    CoinPool.Instance.ReturnObject(coin);
                }
            }
            sectionCoins.Remove(oldSection);
        }

        if (sectionEnemies.TryGetValue(oldSection, out List<GameObject> oldEnemies))
        {
            foreach (GameObject enemy in oldEnemies)
            {
                if (enemy != null)
                {
                    EnemyPool.Instance.ReturnObject(enemy); // Return enemy to pool
                }
            }
            sectionEnemies.Remove(oldSection);
        }

        if (oldSection != null)
        {
            oldSection.SetActive(false);
            sectionPool.ReturnObject(oldSection);
        }
    }

    private List<GameObject> SpawnCoins(GameObject section)
    {
        if (section == null)
        {
            Debug.LogError("Section is null, cannot spawn coins.");
            return null;
        }

        List<GameObject> coins = new List<GameObject>();
        float[] xPositions = { -6.0f, -1.0f, 4.0f };
        float[] zPositions = { 4.0f, 6.0f, 8.0f, 10.0f, 12.0f, 17.0f, 19.0f, 21.0f, 23.0f, 25.0f, 27.0f, 29.0f, 31.0f, 33.0f, 38.0f, 40.0f, 42.0f };

        int coinCount = 0;
        while (coinCount < 10)
        {
            float randomZ = zPositions[Random.Range(0, zPositions.Length)];
            float randomX = xPositions[Random.Range(0, xPositions.Length)];

            Vector3 coinPosition = new Vector3(randomX, 1.0f, Player.position.z + distanceAhead + randomZ);
            GameObject coin = coinPool.GetObject();

            if (coin == null)
            {
                Debug.LogError("Failed to get a coin from the pool.");
                continue;
            }

            coin.transform.position = coinPosition;
            coin.transform.parent = section.transform;

            coins.Add(coin);
            coinCount++;
        }

        return coins;
    }

    private List<GameObject> AddObstacles(GameObject section)
    {
        if (section == null)
        {
            Debug.LogError("Section is null, cannot add obstacles.");
            return new List<GameObject>();
        }

        List<GameObject> obstacles = new List<GameObject>();
        List<Vector3> possiblePositions = new List<Vector3>
        {
            new Vector3(-5.5f, 0, Player.position.z + distanceAhead + 35),
            new Vector3(-5.5f, 0, Player.position.z + distanceAhead + 15),
            new Vector3(-0.5f, 0, Player.position.z + distanceAhead + 35),
            new Vector3(-0.5f, 0, Player.position.z + distanceAhead + 15),
            new Vector3(5.5f, 0, Player.position.z + distanceAhead + 35),
            new Vector3(5.5f, 0, Player.position.z + distanceAhead + 15),
            new Vector3(-7f, 0, Player.position.z + distanceAhead + 35),
            new Vector3(-7f, 0, Player.position.z + distanceAhead + 15),
            new Vector3(-8.5f, 8, Player.position.z + distanceAhead + 15),
            new Vector3(-8.5f, 8, Player.position.z + distanceAhead + 35)
        };

        ShuffleList(possiblePositions);

        Dictionary<Vector3, GameObject[]> positionToObstacleArray = new Dictionary<Vector3, GameObject[]>
        {
            { new Vector3(-5.5f, 0, 0), obstaclesAtMinus5_5 },
            { new Vector3(-0.5f, 0, 0), obstaclesAtMinus0_5 },
            { new Vector3(5.5f, 0, 0), obstaclesAt5_5 },
            { new Vector3(-7f, 0, 0), obstaclesAtMinus7 },
            { new Vector3(-8.5f, 8, 0), roofObstacle }
        };

        int obstacleCount = 0;
        HashSet<float> usedZPositions = new HashSet<float>();

        foreach (Vector3 position in possiblePositions)
        {
            if (usedZPositions.Contains(position.z)) continue;

            positionToObstacleArray.TryGetValue(new Vector3(position.x, position.y, 0), out GameObject[] obstacleArray);
            if (obstacleArray != null && obstacleArray.Length > 0)
            {
                ShuffleArray(obstacleArray);
                GameObject obstacle = Instantiate(obstacleArray[0], position, Quaternion.Euler(0, 90, 0));
                obstacle.transform.parent = section.transform;
                obstacles.Add(obstacle);
                usedZPositions.Add(position.z);
                obstacleCount++;
                if (obstacleCount >= 2) break;
            }
        }

        return obstacles;
    }

    private List<GameObject> SpawnEnemies(GameObject section)
    {
        if (section == null)
        {
            Debug.LogError("Section is null, cannot spawn enemies.");
            return null;
        }

        List<GameObject> enemies = new List<GameObject>();
        float[] xPositions = { -6.0f, -1.0f, 4.0f }; // Possible x positions for enemies
        float[] zPositions = { 10.0f, 20.0f, 30.0f }; // Possible z positions ahead of the section

        int enemyCount = Random.Range(1, 3); // Random number of enemies to spawn per section

        for (int i = 0; i < enemyCount; i++)
        {
            float randomX = xPositions[Random.Range(0, xPositions.Length)];
            float randomZ = zPositions[Random.Range(0, zPositions.Length)];
            Vector3 enemyPosition = new Vector3(randomX, 0, Player.position.z + distanceAhead + randomZ);

            GameObject enemy = enemyPool.GetObject();
            if (enemy == null)
            {
                Debug.LogError("Failed to get an enemy from the pool.");
                continue;
            }

            enemy.transform.position = enemyPosition;
            enemy.transform.parent = section.transform;

            enemies.Add(enemy);
        }

        return enemies;
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
