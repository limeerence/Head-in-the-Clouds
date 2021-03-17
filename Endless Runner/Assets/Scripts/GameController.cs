using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int health = 50;
    public int speed = 10;
    public int jumpIntensity = 7;
    public int level = 1;
    public int gapUpperLimit = 8;
    public int platXVariance =7;

    private int timer = 30;

    [SerializeField] public Text speedText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text levelText;

    [SerializeField] public Image healthBar;
    [SerializeField] public Sprite health0;
    [SerializeField] public Sprite health1;
    [SerializeField] public Sprite health2;
    [SerializeField] public Sprite health3;
    [SerializeField] public Sprite health4;
    [SerializeField] public Sprite health5;

    [SerializeField] private Image gameOverImage;
    [SerializeField] private Image moveControlsImage;
    [SerializeField] private Image nextLevelImage;

    [SerializeField] private Image skipLevelImage;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image slowedImage;

    private void Start()
    {
        health = PlayerPrefs.GetInt("PlayerHealth", 50);
        speed = PlayerPrefs.GetInt("PlayerSpeed", 10);
        level = PlayerPrefs.GetInt("PlayerLevel", 1);
        gapUpperLimit = PlayerPrefs.GetInt("Gap", 8);
        platXVariance = PlayerPrefs.GetInt("PlatVariance", 7);

        speedText.text = "Speed : " + speed;
        levelText.text = "Level " + level;
        updateHealth(0);
        if (level == 1)
            moveControlsImage.gameObject.SetActive(true);   
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        while(timer > 0)
        { 
            timerText.text = "" + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<characterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent <Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        healthImage.gameObject.SetActive(false);
        slowedImage.gameObject.SetActive(false);
        nextLevelImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        ChangeLevel();
    }

    public void ChangeLevel()
    {
        PlayerPrefs.SetInt("PlayerHealth", health);
        PlayerPrefs.SetInt("PlayerSpeed", speed + 3);
        PlayerPrefs.SetInt("PlayerLevel", level + 1);
        PlayerPrefs.SetInt("Gap", level % 10 == 0 ? gapUpperLimit + 1 : gapUpperLimit);
        PlayerPrefs.SetInt("PlatVariance", level % 10 == 0 ? platXVariance + 1 : platXVariance);
        int nextLevel = (SceneManager.GetActiveScene().buildIndex + 1) % 5;
        SceneManager.LoadScene(nextLevel == 0 ? 1 : nextLevel);
    }

    private void Update()
    {
        if (timer <= 26 && level == 1)
            moveControlsImage.gameObject.SetActive(false);
    }

    public void updateHealth(int addHealth)
    {
        health += addHealth;
        switch (health)
        {
            case 0:
                GameOver();
                break;
            case 10:
                healthBar.sprite = health1;
                break;
            case 20:
                healthBar.sprite = health2;
                break;
            case 30:
                healthBar.sprite = health3;
                break;
            case 40:
                healthBar.sprite = health4;
                break;
            case 50:
                healthBar.sprite = health5;
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        healthBar.sprite = health0;
        timer = 5;
        gameOverImage.gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<characterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        Debug.Log("GAME OVER!!");
        Invoke("returnToMenu", 3);
    }

    private void returnToMenu()
    {
        PlayerPrefs.DeleteKey("PlayerHealth");
        PlayerPrefs.DeleteKey("PlayerSpeed");
        PlayerPrefs.DeleteKey("PlayerLevel");
        PlayerPrefs.DeleteKey("Gap");
        PlayerPrefs.DeleteKey("PlatVariance");
        SceneManager.LoadScene(0);
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("PlayerHealth");
        PlayerPrefs.DeleteKey("PlayerSpeed");
        PlayerPrefs.DeleteKey("PlayerLevel");
        PlayerPrefs.DeleteKey("Gap");
        PlayerPrefs.DeleteKey("PlatVariance");
    }

    IEnumerator SkipLevel()
    {
        timer = 5;
        skipLevelImage.gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<characterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(2);
        skipLevelImage.gameObject.SetActive(false);
        ChangeLevel();
    }

    IEnumerator healthPopup()
    {
        healthImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        healthImage.gameObject.SetActive(false);
    }

    IEnumerator slowedPopup()
    {
        slowedImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        slowedImage.gameObject.SetActive(false);
    }
}
