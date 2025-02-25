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
    
    #region Game_Status
    public enum Game_Status
    {
        NONE,
        PLAYER1_PLAY,
        PLAYER2_PLAY,
        BALL_IN_GAME
    }
    public Game_Status status;
    public GameObject[] statusText;
    public GameObject[] balls;
    public GameObject ball;
    public GameObject win;
    private int ballInUse = 0;
    private int ballSelected = 0;
    #endregion
    
    #endregion

    private void Awake() 
    {
        //Set the instance!
        if(_Instance == null) _Instance = this; else Destroy(this);
    }
    
    void Start() 
    {
        //Seta a vida inicial!
        player1.life = player2.life = 100;
    }

    void Update()
    {
        Game_Status_Gameplay();
    }

    void Game_Status_Gameplay()
    {
        switch(status)
        {
            #region GS_NONE
            case Game_Status.NONE:
            //ATUALIZA OS TEXTOS DA HUD
            if(!statusText[0].activeInHierarchy)
            {
                Change_Text(0);
                player1.lifeText.text = "Life: "+player1.life+"%";
                player2.lifeText.text = "Life: "+player2.life+"%";
            }

            //SETA A BOLA COMO NULA!
            ball = null;
            ballInUse = ballSelected = 0;

            //DESMARCA O JOGADOR ATUAL
            if(player1.playerText.color != Color.white) player1.playerText.color = Color.white;
            if(player2.playerText.color != Color.white) player2.playerText.color = Color.white;

            //AO APERTAR ESPAÇO SETA O NOVO JOGADOR ATUAL E MUDA PARA O STATUS DO JOGADOR ATUAL!
            if(Input.GetButtonDown("Jump"))
            {
                if(playerRound == Players.PLAYER1)
                {
                    playerRound = Players.PLAYER2;
                    status = Game_Status.PLAYER2_PLAY;
                }
                else if(playerRound == Players.PLAYER2)
                {
                    playerRound = Players.PLAYER1;
                    status = Game_Status.PLAYER1_PLAY;
                }
            }

            break;
            #endregion
            #region GS_P1
            case Game_Status.PLAYER1_PLAY:
                //ATUALIZA O TEXTO DO STATUS ATUAL!
                if(!statusText[1].activeInHierarchy)
                    Change_Text(1);

                //MARCA O JOGADOR ATUAL!
                if(player1.playerText.color != Color.yellow) player1.playerText.color = Color.yellow;

                //PERMITE A MIRA DO JOGADOR ATUAL
                Player_Rotation(player1.Object, 0f);

                //SPAWNA A BOLA!
                if(ball == null)
                {
                    ball = Instantiate(balls[ballInUse], new Vector2(player1.ballLaunchPosition.position.x, 
                                                                     player1.ballLaunchPosition.position.y), 
                                                                     player1.ballLaunchPosition.rotation);

                    ball.transform.parent = player1.ballLaunchPosition;
                    break;
                }
                //SE TEM OUTRA BOLA SELECIONADA DESTROI A ATUAL
                if((ball != null) && (ballInUse != ballSelected))
                {
                    ballInUse = ballSelected;
                    Destroy(ball);
                    break;
                }

                if(Input.GetButtonDown("Jump"))
                {
                    ball.transform.parent = null;
                    ball.transform.rotation = Quaternion.Euler(player1.Object.transform.rotation.eulerAngles.x, 
                                                               player1.Object.transform.rotation.eulerAngles.y, 
                                                               player1.Object.transform.rotation.eulerAngles.z);

                    BallScript._Instance.launched = true;
                    status = Game_Status.BALL_IN_GAME;
                    player1.Object.transform.rotation = Quaternion.Euler(0f,0f,0f);
                }
            break;
            #endregion
            #region GS_P2
            case Game_Status.PLAYER2_PLAY:
                //ATUALIZA O TEXTO DO STATUS ATUAL!
                if(!statusText[2].activeInHierarchy)
                    Change_Text(2);

                //MARCA O JOGADOR ATUAL!
                if(player1.playerText.color != Color.yellow) player1.playerText.color = Color.yellow;

                //PERMITE A MIRA DO JOGADOR ATUAL
                Player_Rotation(player2.Object, 180f);

                //SPAWNA A BOLA!
                if(ball == null)
                {
                    ball = Instantiate(balls[ballInUse], new Vector2(player2.ballLaunchPosition.position.x, 
                                                                     player2.ballLaunchPosition.position.y), 
                                                                     player2.ballLaunchPosition.rotation);
                                                                     
                    ball.transform.parent = player2.ballLaunchPosition;
                    break;
                }
                //SE TEM OUTRA BOLA SELECIONADA DESTROI A ATUAL
                if((ball != null) && (ballInUse != ballSelected))
                {
                    ballInUse = ballSelected;
                    Destroy(ball);
                    break;
                }

                if(Input.GetButtonDown("Jump"))
                {
                    ball.transform.parent = null;
                    ball.transform.rotation = Quaternion.Euler(player2.Object.transform.rotation.eulerAngles.x, 
                                                               player2.Object.transform.rotation.eulerAngles.y, 
                                                               player2.Object.transform.rotation.eulerAngles.z);

                    BallScript._Instance.launched = true;
                    status = Game_Status.BALL_IN_GAME;
                    player2.Object.transform.rotation = Quaternion.Euler(180f,0f,180f);
                }
            break;
            #endregion
            #region GS_BIG
            case Game_Status.BALL_IN_GAME:
                //ATUALIZA O TEXTO DO STATUS ATUAL!
                if(!statusText[3].activeInHierarchy)
                    Change_Text(3);
            break;
            #endregion
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


        if(normalAngle >= 180)
        {
            if((angle < -135) && (angle > -180))
                p.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, angle*-1));

            if((angle > minAngle) && (angle < maxAngle))
                p.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, angle*-1));
        }
        else
        {
            if((angle > minAngle) && (angle < maxAngle))
                p.transform.rotation = Quaternion.Euler(new Vector3(p.transform.rotation.eulerAngles.x, 0f, angle));
        }
    }

    public void Ball_Destroyed(string thePlayer, int damage)
    {
        //Danifica o Jogador!
        switch(thePlayer)
        {
            case "Player1":
                player1.life -= damage;
            break;
            case "Player2":
                player2.life -= damage;
            break;
        }

        //Seta o status como None
        status = Game_Status.NONE;

        if(player1.life <= 0 || player2.life <= 0)
        {
            string winText = "";
            if(player1.life <= 0) winText = "Player2 wins!"; else winText = "Player1 wins!";

            GameObject w = Instantiate(win);
            w.GetComponent<WinScreen>().Win(winText);
        }
    }

    public void Change_Ball(int b)
    {
        ballSelected = b;
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
    public Transform ballLaunchPosition;
    public int life = 100;
    public TMP_Text playerText;
    public TMP_Text lifeText;
}
