using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallScript : MonoBehaviour
{
    #region VARIABLES
    public static BallScript _Instance;

    [Header("Damage Vars")]
    public int defenseDamage;
    public int fullDamage;
    public int canBounseTimes;
    public TMP_Text bounceTimesText;

    [Header("Launch Vars")]
    public bool isLaunchBall;
    public bool launched;

    [Header("Move Vars")]
    public float vel;
    public float launchVel;
    public int x = -1;
    public int y = 1;
    #endregion

    private void Awake() 
    {
        //Set the Instance!
        if(_Instance == null) _Instance = this; else Destroy(this);
    }
    
    void Update()
    {
        if(canBounseTimes >= 0) bounceTimesText.text = ""+canBounseTimes;
        switch(GameController._Instance.status)
        {
            case GameController.Game_Status.PLAYER1_PLAY:
                isLaunchBall = true;
            break;
            case GameController.Game_Status.PLAYER2_PLAY:
                isLaunchBall = true;
            break;
            case GameController.Game_Status.BALL_IN_GAME:
                if(!isLaunchBall)
                {
                    AutoMoveBall();
                }
                else
                {
                    if(launched)
                        LaunchMoveBall();
                }
            break;
        }
    }


    private void OnCollisionEnter2D(Collision2D c)
    {
        if(isLaunchBall)
        {
            isLaunchBall = false;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        switch(c.gameObject.tag)
        {
            case "Up_Border":
                if(canBounseTimes > 0) canBounseTimes--;
                y = -1;
            break;
            case "Down_Border":
                if(canBounseTimes > 0) canBounseTimes--;
                y = 1;
            break;
            case "P1_Border":
                GameController._Instance.Ball_Destroyed("Player1", fullDamage);
                Destroy(gameObject);
            break;
            case "P2_Border":
                GameController._Instance.Ball_Destroyed("Player2", fullDamage);
                Destroy(gameObject);
            break;
            case "Player1":
                if(canBounseTimes > 0 || canBounseTimes < 0)
                {
                    canBounseTimes--;
                    x = 1;
                }
                else if(canBounseTimes == 0)
                {
                    GameController._Instance.Ball_Destroyed("Player1", defenseDamage);
                    Destroy(gameObject);
                }
            break;
            case "Player2":
                if(canBounseTimes > 0 || canBounseTimes < 0)
                {
                    canBounseTimes--;
                    x = -1;
                }
                else if(canBounseTimes == 0)
                {
                    GameController._Instance.Ball_Destroyed("Player2", defenseDamage);
                    Destroy(gameObject);
                }
            break;
        }
    }

    private void LaunchMoveBall()
    {
        float oldx = transform.position.x;
        float oldy = transform.position.y;
        transform.Translate(Vector2.right * launchVel * Time.deltaTime);
        if(oldx < transform.position.x) x = 1; else if(oldx > transform.position.x) x = -1;
        if(oldy < transform.position.y) y = 1; else if(oldy > transform.position.y) y = -1;
    }

    private void AutoMoveBall()
    {
        //x
        float velx = vel*x;
        transform.Translate(Vector2.right * velx * Time.deltaTime);

        //y
        float vely = vel*y;
        transform.Translate(Vector2.up * vely * Time.deltaTime);
    }
}
