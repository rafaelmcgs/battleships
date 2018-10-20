using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canhao : MonoBehaviour
{
    public int tipo;

    public Sprite imageNormal;
    public Sprite imageSelected;

    private bool selected = false;
    private BattleManager manager;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetShipsManager (BattleManager manager_)
    {
        manager = manager_;
    }

    public void clickEvent()
    {
        if (selected)
        {
            selected = false;
            GetComponent<Image>().sprite = imageNormal;
        }
        else
        {
            manager.unselectAllAttacks();
            selected = true;
            GetComponent<Image>().sprite = imageSelected;

        }
    }

    public void unSelect()
    {

        selected = false;
        GetComponent<Image>().sprite = imageNormal;
    }
}
