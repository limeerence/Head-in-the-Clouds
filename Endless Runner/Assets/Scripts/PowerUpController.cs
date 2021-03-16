using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void Start()
    {
        if (!gameController)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    private void Update()
    {
        //make powerups spin
        transform.Rotate(0,70*Time.deltaTime,0);

        //disable hearts if full health
        if (gameObject.tag == "Health" && gameController.health >= 50)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        //disable snowflakes if less than 8 speed
        if (gameObject.tag == "Slow" && gameController.speed <= 7)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
