using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab40;
    [SerializeField] private GameObject platformPrefab20;
    [SerializeField] private GameObject platformPrefab10;

    [SerializeField] private GameObject deathPrefab40;
    [SerializeField] private GameObject deathPrefab20;
    [SerializeField] private GameObject deathPrefab10;

    [SerializeField] private GameObject obstacle1Prefab;
    [SerializeField] private GameObject obstacle2Prefab;
    [SerializeField] private GameObject obstacle3Prefab;

    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject snowflakePrefab;

    [SerializeField] private GameController gameController;

    private float platDistanceThreshold = 100;
    private float obsDistanceThreshold = 4;
    private Vector3 nextPlatformPos = Vector3.zero;
    private Vector3 deathPos = Vector3.zero;
    private GameObject player;
    private Vector3 nextObsPos;
    private Vector3 prevObsPos = Vector3.zero;
    private int platformLength;
    private int starCount = 0;

    private void Awake()
    {
        if (!gameController)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        GameObject plat = Instantiate(platformPrefab40, nextPlatformPos, Quaternion.identity); //Gimbal Lock - Quaternions vs Euler Angles
        
        deathPos = nextPlatformPos;
        deathPos.y -= 8;
        GameObject death = Instantiate(deathPrefab40, deathPos, Quaternion.identity);
        death.transform.parent = plat.transform;

        nextPlatformPos += new Vector3(0, 0, 25);
    }

    private void Update()
    {
        if (Vector3.Distance(nextPlatformPos,player.transform.position) < platDistanceThreshold)
        {
            GameObject plat;
            GameObject death;
            
            //spawn platforms and obstacles
            int platformVariant = Random.Range(1, 10);
            switch (platformVariant)
            {
                case 1:
                    nextPlatformPos.z += 5;
                    plat = Instantiate(platformPrefab10, nextPlatformPos, Quaternion.identity);
                    platformLength = 5;

                    deathPos = nextPlatformPos;
                    deathPos.y -= 8;
                    death = Instantiate(deathPrefab10, deathPos, Quaternion.identity);
                    death.transform.parent = plat.transform;

                    spawnPowerUp(plat, nextPlatformPos);
                    spawnObstacles(Random.Range(1, 3), plat, 3);
                    break;
                case 2:
                case 3:
                case 4:
                    nextPlatformPos.z += 10;
                    plat = Instantiate(platformPrefab20, nextPlatformPos, Quaternion.identity);
                    platformLength = 10;

                    deathPos = nextPlatformPos;
                    deathPos.y -= 8;
                    death = Instantiate(deathPrefab20, deathPos, Quaternion.identity);
                    death.transform.parent = plat.transform;

                    spawnPowerUp(plat, nextPlatformPos);
                    spawnObstacles(Random.Range(3, 5), plat, 8);
                    break;
                default:
                    nextPlatformPos.z += 20;
                    plat = Instantiate(platformPrefab40, nextPlatformPos, Quaternion.identity);
                    platformLength = 20;

                    deathPos = nextPlatformPos;
                    deathPos.y -= 8;
                    death = Instantiate(deathPrefab40, deathPos, Quaternion.identity);
                    death.transform.parent = plat.transform;

                    spawnPowerUp(plat, nextPlatformPos);
                    spawnObstacles(Random.Range(5, 7), plat, 18);
                    break;
            }

            int gap = Random.Range(3, gameController.gapUpperLimit);
            nextPlatformPos += new Vector3(Random.Range(-gameController.platXVariance, gameController.platXVariance), Random.Range(-2, 2), (platformLength + gap));
        }
    }

    private void spawnPowerUp(GameObject plat, Vector3 nextPowerupPos)
    {
        GameObject powerUp;
        nextPowerupPos.y += 5;
        int powerupVariant = Random.Range(1, 10);
        switch (powerupVariant)
        {
            case 1:

                powerUp = Instantiate(starCount < 2 ? starPrefab : heartPrefab, nextPowerupPos, Quaternion.identity);
                powerUp.transform.parent = plat.transform;
                starCount++;
                break;
            case 2:
                powerUp = Instantiate(heartPrefab, nextPowerupPos, Quaternion.identity);
                powerUp.transform.parent = plat.transform;
                break;
            case 3:
                powerUp = Instantiate(snowflakePrefab, nextPowerupPos, Quaternion.identity);
                powerUp.transform.parent = plat.transform;
                break;
            default:
                break;
        }
        
    }

    private void spawnObstacles(int numObstacles, GameObject plat, int zVal)
    {
        for (int i = 0; i < numObstacles; i++)
        {
            nextObsPos = new Vector3(
                nextPlatformPos.x + Random.Range(-4, 4), 
                nextPlatformPos.y + 1, 
                nextPlatformPos.z + Random.Range(-zVal, zVal)
            );

            if (Vector3.Distance(nextObsPos, prevObsPos) > obsDistanceThreshold)
            {
                int obstacleVariant = Random.Range(1, 100);
                GameObject obs = Instantiate(obstacleVariant == 100 && gameController.level >= 30 ? obstacle3Prefab
                    : obstacleVariant <= 50 ? obstacle1Prefab
                    : obstacle2Prefab, nextObsPos, Quaternion.identity);

                obs.transform.parent = plat.transform;
                prevObsPos = nextObsPos;
            } else
            {
                i--;
            }
            
        }
    }
}
