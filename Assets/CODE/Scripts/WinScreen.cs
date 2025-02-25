using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public static WinScreen _Instance;
    string theText;
    bool textSet;


    private void Awake() 
    {
        //Set the instance!
        if(_Instance == null)
        {
            _Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
        else Destroy(this);
    }

    private void Update()
    {
        if(!textSet && SceneManager.GetActiveScene().name == "EndScreen")
        {
            print("pass1");
            if(GameObject.Find("WinText"))
            {
                print("pass2");
                TMP_Text txt = GameObject.Find("WinText").GetComponent<TMP_Text>();
                txt.text = theText;
                textSet = true;
            }
        }
    }

    public void Win(string text)
    {
        theText = text;
        SceneManager.LoadScene("EndScreen");
    }
    
}
