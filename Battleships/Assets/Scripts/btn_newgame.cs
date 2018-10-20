using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btn_newgame : MonoBehaviour {

	public void startGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("turno", 1);
        SceneManager.LoadScene("SetShips");
    }
}
