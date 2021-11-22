using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Text score;
    public Animator fadeAnimator;
    public Result result;

    // Start is called before the first frame update
    void Start()
    {
        result = Result.instance;
        score.text = "SCORE:"+ result.score;
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
