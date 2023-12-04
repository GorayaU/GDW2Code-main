using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public static bool GameIsOver = false;

    public GameObject GameOverUI;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Side Block")
        {
            GameOver();
        }

        
    }
    public void GameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsOver = true;
    }

     public void LoadMenu()
     {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
     }

}
