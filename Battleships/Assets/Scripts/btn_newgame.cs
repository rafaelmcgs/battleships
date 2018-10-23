using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btn_newgame : MonoBehaviour {

	public void startGame()
    {
        GetComponent<AudioSource>().Play();

        StartCoroutine(mudarCena());
    }

    IEnumerator mudarCena()
    {
        yield return new WaitForSeconds(0.22f);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("turno", 1);
        SceneManager.LoadScene("SetShips");
    }
}
