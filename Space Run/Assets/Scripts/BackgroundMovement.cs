using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        transform.position += transform.up * -gameManager.bgSpeed * Time.deltaTime;

        if (transform.position.y < -14f)
        {
            transform.position = new Vector2(0f, 13.9f);
        }
    }
}
