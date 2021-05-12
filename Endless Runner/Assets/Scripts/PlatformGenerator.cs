using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs = new GameObject[3];
    [SerializeField] private GameObject[] obstaclePrefabs = new GameObject[3];

    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject snowflakePrefab;

    [SerializeField] private GameController gameController;

    private float platDistanceThreshold = 100;
    private float obsDistanceThreshold = 4;
    private Vector3 nextPlatformPos = Vector3.zero;
    private GameObject player;
    private Vector3 nextObsPos;
    private Vector3 prevObsPos = Vector3.zero;
    private int[] platformLengths = new int[] { 5, 10, 20 };
    private int[,] obstacleRanges = { { 1, 3 }, { 3, 5 }, { 5, 7 } };
    private int starCount = 0;

    private void Awake()
    {
        if (!gameController)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        Instantiate(platformPrefabs[2], nextPlatformPos, Quaternion.identity); //Gimbal Lock - Quaternions vs Euler Angles

        nextPlatformPos += new Vector3(0, 0, 25);
    }

    private void Update()
    {
        if (Vector3.Distance(nextPlatformPos,player.transform.position) < platDistanceThreshold)
        {
            spawnPlatform();
        }
    }

    private void spawnPlatform()
    {
        GameObject plat;
        int platVariant = Random.Range(0, 2);

        nextPlatformPos.z += platformLengths[platVariant];
        plat = Instantiate(platformPrefabs[platVariant], nextPlatformPos, Quaternion.identity);

        spawnPowerUp(plat, nextPlatformPos);
        spawnObstacles(Random.Range(obstacleRanges[platVariant, 0], obstacleRanges[platVariant, 1]), plat, platformLengths[platVariant] - 2);

        int gap = Random.Range(3, gameController.gapUpperLimit);
        nextPlatformPos += new Vector3(Random.Range(-gameController.platXVariance, gameController.platXVariance), Random.Range(-2, 2), (platformLengths[platVariant] + gap));
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
                GameObject obs = Instantiate(obstacleVariant == 100 && gameController.level >= 30 ? obstaclePrefabs[2]
                    : obstacleVariant <= 50 ? obstaclePrefabs[0]
                    : obstaclePrefabs[1], nextObsPos, Quaternion.identity);

                obs.transform.parent = plat.transform;
                prevObsPos = nextObsPos;
            } else
            {
                i--;
            }
            
        }
    }
}
