using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;
    
    public Checkpoint[] allCheckpoints;

    public int totalLaps;

    public CarControllers playerCar;
    public List<CarControllers> allAICars = new List<CarControllers>();
    public int playerPosition;
    public float timeBetweenPosCheck = .2f;
    private float posChkCounter;

    public float aiDefaultSpeed = 30f, playerDefaultSpeed = 30f, rubberBandSpeedMod = 3.5f, rubBandAccel = .5f;

    private void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< allCheckpoints.Length; i++)
        {
            allCheckpoints[i].cpNumber = i;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        posChkCounter -= Time.deltaTime;
        if(posChkCounter <= 0)
        {
            playerPosition =1;

            foreach (CarControllers aiCar in allAICars)
            {
                if(aiCar.currentLap > playerCar.currentLap)
                {
                    playerPosition++;
                }    
                else if(aiCar.currentLap == playerCar.currentLap)
                {
                    if(aiCar.nextCheckpoint > playerCar.nextCheckpoint)
                    {
                        playerPosition++;
                    } 
                    else if(aiCar.nextCheckpoint == playerCar.nextCheckpoint)
                    {
                        if(Vector3.Distance(aiCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position) < Vector3.Distance(playerCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position))
                        {
                            playerPosition++;
                        }
                    }
                }    
            }

            posChkCounter = timeBetweenPosCheck;
            UIManager.instance.positionText.text = playerPosition + "/" + (allAICars.Count + 1);
        }

        //manage rubber banding
        if(playerPosition == 1)
        {
            foreach(CarControllers aiCar in allAICars)
            {
                aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefaultSpeed - rubberBandSpeedMod, rubBandAccel * Time.deltaTime);
            }

            playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed - rubberBandSpeedMod, rubBandAccel * Time.deltaTime);
        }
        
    }
}
