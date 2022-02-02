using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reiniciarjuego : MonoBehaviour
{
    public void ResetGame()
    {
        SceneManager.LoadScene("Game");     
    }
}
