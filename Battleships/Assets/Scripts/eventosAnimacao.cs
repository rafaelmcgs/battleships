using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventosAnimacao : MonoBehaviour {
    public GameObject[] objetos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AtivarGameObject(int num )
    {
        objetos[num].SetActive(true);
    }
}
