using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class characterController : MonoBehaviour
{
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private GameController gameController;

    private void Start()
    {
        //Debug.Log("Start");
        if (!gameController)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

    }

    private void Update()
    {
        //move forward constantly
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 1) * gameController.speed * Time.deltaTime;
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * gameController.jumpIntensity, ForceMode.Impulse);
        //get down from in air
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isGrounded)
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * gameController.jumpIntensity * 2, ForceMode.Impulse);
        //Debug.Log("Update");

    }

    private void OnCollisionEnter(Collision collision)
    {
        //set player touching ground, allows for jumping
        if (collision.gameObject.tag == "Platform")
            isGrounded = true;
        //reduce health for hitting obstacle
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            Debug.Log("Obstacle Hit");
            gameController.updateHealth(-10);
        }
        if (collision.gameObject.tag == "Death")
        {
            gameController.GameOver();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //player is in the air, disallows jumping until grounded again
        if (collision.gameObject.tag == "Platform")
            isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //slows player
        if (other.gameObject.tag == "Slow")
        {
            Debug.Log("Slow Debuff");
            Destroy(other.gameObject);
            gameController.speed -= 5;
            gameController.speedText.text = "Speed : " + gameController.speed;
            gameController.StartCoroutine("slowedPopup");
        }

        //gives player an extra life
        if (other.gameObject.tag == "Health")
        {
            Debug.Log("1-UP");
            Destroy(other.gameObject);
            gameController.updateHealth(10);
            gameController.StartCoroutine("healthPopup");
        }

        //skips current game level
        if (other.gameObject.tag == "Skip")
        {
            Debug.Log("Skip Level");
            Destroy(other.gameObject);
            gameController.StartCoroutine("SkipLevel");
        }
    }

}
