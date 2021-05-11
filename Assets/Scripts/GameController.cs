using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    #region VARIABLES
    public static GameController _Instance;

    #region Player
    public enum Players 
    {
        PLAYER1,
        PLAYER2
    }
    public Players playerRound;
    public PlayerVars player1;
    public PlayerVars player2;
    #endregion
    
    public enum Game_Status
    {
        NONE,
        PLAYER1_PLAY,
        PLAYER2_PLAY,
        BALL_IN_GAME
    }
    public Game_Status status;
    public GameObject[] statusText;
    #endregion

    private void Awake() 
    {
        if(_Instance == null) _Instance = this; else Destroy(this);
    }

    void Update()
    {
        Game_Status_Gameplay();
    }

    void Game_Status_Gameplay()
    {
        switch(status)
        {
            case Game_Status.NONE:
            if(!statusText[0].activeInHierarchy)
                Change_Text(0);

            if(Input.GetButtonDown("Fire1"))
            {
                if(playerRound == Players.PLAYER1)
                {
                    playerRound = Players.PLAYER2;
                    status = Game_Status.PLAYER2_PLAY;
                }
                if(playerRound == Players.PLAYER2)
                {
                    playerRound = Players.PLAYER1;
                    status = Game_Status.PLAYER1_PLAY;
                }
            }

            break;
            case Game_Status.PLAYER1_PLAY:
                if(!statusText[1].activeInHierarchy)
                    Change_Text(1);

                Player_Rotation(player1.Object, 0f);

                if(Input.GetButtonDown("Fire1"))
                {
                    BallScript._Instance.launched = true;
                    status = Game_Status.BALL_IN_GAME;
                    player1.Object.transform.rotation = Quaternion.Euler(0f,0f,0f);
                }
            break;
            case Game_Status.PLAYER2_PLAY:
                if(!statusText[2].activeInHierarchy)
                    Change_Text(2);

                Player_Rotation(player2.Object, 180f);

                if(Input.GetButtonDown("Fire1"))
                {
                    BallScript._Instance.launched = true;
                    status = Game_Status.BALL_IN_GAME;
                    player2.Object.transform.rotation = Quaternion.Euler(0f,0f,0f);
                }
            break;
            case Game_Status.BALL_IN_GAME:
                if(!statusText[3].activeInHierarchy)
                    Change_Text(3);
            break;
        }
    }

    private void Player_Rotation(GameObject p, float normalAngle)
    {
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint (p.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        float maxAngle = normalAngle + 45f;
        float minAngle = normalAngle - 45f;

        if((angle > minAngle) && (angle < maxAngle))
            p.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Ball_Destroyed()
    {
        status = Game_Status.NONE;

        if(playerRound == Players.PLAYER1)
        {
            playerRound = Players.PLAYER2;
            status = Game_Status.PLAYER2_PLAY;
        }
        else
        {
            playerRound = Players.PLAYER1;
            status = Game_Status.PLAYER1_PLAY;
        }
    }

    private void Change_Text(int i)
    {
        foreach(GameObject g in statusText)
        {
            g.SetActive(false);
        }
        statusText[i].SetActive(true);
    }
}

[System.Serializable]
public class PlayerVars
{
    public GameObject Object;
    public TMP_Text lifeText;
}
