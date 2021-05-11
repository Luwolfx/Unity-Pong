using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    #region VARIABLES
    public static BallScript _Instance;

    [Header("Launch Vars")]
    public bool isLaunchBall;
    public bool launched;

    [Header("Move Vars")]
    public float vel;
    public int x = -1;
    public int y = 1;
    #endregion

    private void Awake() 
    {
        if(_Instance == null) _Instance = this; else Destroy(this);
    }

    void Update()
    {
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
                y = -1;
            break;
            case "Down_Border":
                y = 1;
            break;
            case "P1_Border":
                x = 1;
            break;
            case "P2_Border":
                x = -1;
            break;
            case "Player1":
                x = 1;
            break;
            case "Player2":
                x = -1;
            break;
            default:

            break;
        }
    }

    private void LaunchMoveBall()
    {
        float oldx = transform.position.x;
        float oldy = transform.position.y;
        transform.Translate(Vector2.right * vel * Time.deltaTime);
        if(oldx < transform.position.x) x = 1; else if(oldx > transform.position.x) x = -1;
        if(oldy < transform.position.y) x = 1; else if(oldy > transform.position.y) x = -1;
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
