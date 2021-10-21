using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Text scoreTxt;
    public Text highTxt;

    // Start is called before the first frame update
    void Start()
    {
        //score.text = "";
        highTxt.text = HandleHightFile.ReadString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHomeScene()
    {
        SceneManager.LoadScene("Home");
    }   
    
    public void ChangeSPScene()
    {
        SceneManager.LoadScene("SP_LV1");
    }  
    
    public void Exit()
    {
        Application.Quit();
    }    

}
