using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navio : MonoBehaviour {
    public int size;
    public bool artilhariaSimples;
    public bool missilBalistico;
    public bool artilhariaPesada;
    public bool bombardeio;
    

    [HideInInspector]
    public int coudown = 0;
    
    private int managerId;
    private SetShipsManager manager;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
    public int getX()
    {
        return Mathf.RoundToInt(transform.position.x / 64);
    }
    public int getY()
    {
        return Mathf.RoundToInt(transform.position.y / 64);
    }
    public bool getVertical()
    {
        if (transform.localEulerAngles.z == 90)
        {
            return true;
        }
        return false;
    }
    public void setId(int id)
    {
        managerId = id;
    }
    public void setManager(SetShipsManager manager_)
    {
        manager = manager_;
    }

    public void setQuadradoRed(int num)
    {

    }
}
