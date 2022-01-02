using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gameManager.gameRunning)
        {
            transform.position += transform.up * -gameManager.obstacleSpeed * Time.deltaTime;

            if (transform.position.y < -7f)
            {
                Destroy(gameObject);
            }
        }
    }
}
