using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Text scoreTxt;
    public Text highTxt;
    public Animator fadeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //score.text = "";
        //highTxt.text = HandleHightFile.ReadString();
    }


    public void ChangeHomeScene()
    {
        fadeAnimator.SetTrigger("In");
        Invoke(nameof(ChangeToHome), 1f);
    }
    private void ChangeToHome()
    {
        SceneManager.LoadScene(0);
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
