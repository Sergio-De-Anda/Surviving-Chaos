using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    PlayerPrefs current;
    public int score;
    public string playerName;
    int startTime;
    public bool reset;

    // Use this for initialization
    void Start()
    {
        score = 0;
        if (!PlayerPrefs.HasKey("Users"))
            PlayerPrefs.SetInt("Users", 1);
        else
            PlayerPrefs.SetInt("Users", PlayerPrefs.GetInt("Users") + 1);
        Debug.Log("Number of Users: " + PlayerPrefs.GetInt("Users"));
        //PlayerPrefs.SetString(PlayerPrefs.GetInt("Users").ToString(), name);
        //PlayerPrefs.SetInt(PlayerPrefs.GetInt("Users").ToString(), score);
        
    }

    // Update is called once per frame
    void Update()
    {
        resetBoard();
    }

    public void enemyShot()
    {
        //Debug.Log("Enemy shot");
        score += 50;
    }

    public void enemyKill()
    {
        score += 200;
    }

    public void printScore()
    {
        PlayerPrefs.SetString(PlayerPrefs.GetInt("Users").ToString(), playerName + " " + score.ToString());
        
        //string name = PlayerPrefs.GetString("Name");
        // print all users
        for(int i = 1; i <= PlayerPrefs.GetInt("Users"); i++)
        {
           // Debug.Log("Name: " + PlayerPrefs.GetString(i.ToString()));
            Debug.Log("Score: " + PlayerPrefs.GetString(i.ToString()));
        }
        //Debug.Log("Score: " + score);
    }
   
    void resetBoard()
    {
        if (reset)
            PlayerPrefs.DeleteAll();
    }

}
