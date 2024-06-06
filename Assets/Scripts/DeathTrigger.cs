using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public bool IsDead { get; private set; }
    // Start is called before the first frame update
    public LogicScript logic;
    [SerializeField]
    private float maxZPosition = -8f; // Set your threshold value
    void Start()
    {
        IsDead = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.z < maxZPosition)
        {
            Die();
            logic.gameOver();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
        {
            Die();
            logic.gameOver();
        }
    }
    public void Die()
    {
        if (IsDead) return;

        IsDead = true;

        Debug.Log("Player died!");
        Time.timeScale = 0;
        // Perform actions such as playing death animation, reducing health, restarting level, etc.
    }
}
