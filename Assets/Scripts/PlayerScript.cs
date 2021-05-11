using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool ws,updown;
    public float vel;

    void Update()
    {
        if(GameController._Instance.status == GameController.Game_Status.NONE || GameController._Instance.status == GameController.Game_Status.BALL_IN_GAME)
        {
            if(ws)
                WS_CONTROLLER();
            if(updown)
                UPDOWN_CONTROLLER();

            if(transform.position.y > 2.55f)
                transform.position = new Vector2(transform.position.x, 2.55f);
            if(transform.position.y < -3.25f)
                transform.position = new Vector2(transform.position.x, -3.25f);
        }
    }

    void WS_CONTROLLER()
    {
        if(Input.GetKey("w") && (transform.position.y < 2.55f))
        {
            transform.Translate(Vector2.up * vel * Time.deltaTime);
        }
        else if(Input.GetKey("s")&& (transform.position.y > -3.25f))
        {
            transform.Translate(Vector2.down * vel * Time.deltaTime);
        }
    }
    void UPDOWN_CONTROLLER()
    {
        if(Input.GetKey("up"))
        {
            transform.Translate(Vector2.up * vel * Time.deltaTime);
        }
        else if(Input.GetKey("down"))
        {
            transform.Translate(Vector2.down * vel * Time.deltaTime);
        }
    }
}
