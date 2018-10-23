using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabuleiroEnemy : MonoBehaviour
{
    public GameObject mira;

    [HideInInspector]
    public bool isDraggin = false;
    [HideInInspector]
    private Vector3 dragMousePosiInicial;
    private BattleManager manager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setManager(BattleManager manager_)
    {
        manager = manager_;
    }

    public void mouseBeginDrag()
    {
        if (manager.canhaoSelected == 0) { return; }
        isDraggin = true;
        dragMousePosiInicial = Input.mousePosition;
        mira.transform.position = Input.mousePosition;
        mira.transform.SetAsLastSibling();
    }
    public void mouseEndDrag()
    {
        if (manager.canhaoSelected == 0) { return; }
        isDraggin = false;
        mira.SetActive(false);

    }
    public void mouseDrag()
    {
        if (manager.canhaoSelected == 0) { return; }
        // adiciono distancia pecorrida pelo mouse
        mira.transform.position = Input.mousePosition;

        Vector3 tempPosi = mira.transform.localPosition;
        // verifico e corrijo posição de acordo com os limites do tabuleiro 15x15
        if (checkAndReturnPosition(tempPosi))
        {
            mira.SetActive(true);
            mira.transform.localPosition = tempPosi;
            snapMira();
        }
        else
        {
            isDraggin = false;
            mira.SetActive(false);
            manager.resetarAtaquesTabuleiro();

        }

    }
    public void mouseDown()
    {
        if (manager.canhaoSelected == 0) { return; }
        mira.SetActive(true);
        mira.transform.position = Input.mousePosition;
        snapMira();
    }
    public void mouseClick()
    {
        if (manager.canhaoSelected == 0) { return; }
        mira.transform.position = Input.mousePosition;
        snapMira();
        manager.mouseClickAttack();
        mira.SetActive(false);
    }
    private void snapMira()
    {
        Vector3 temp = mira.transform.localPosition;
        temp.x = Mathf.Floor(temp.x / 64) * 64;
        temp.y = Mathf.Ceil(temp.y / 64) * 64;
        mira.transform.localPosition = temp;
        manager.ativarQuadradosMira(Mathf.FloorToInt(temp.x / 64), Mathf.FloorToInt(temp.y / 64)*-1);
    }

    private bool checkAndReturnPosition(Vector3 tempPosi_)
    {
        Vector3 tempPosi = tempPosi_;
        // verificar limites do tabuleiro
        if (
            tempPosi.x < 0 ||        
            tempPosi.x > (15 * 64))
        {
            return false;
        }
        if (
            tempPosi.y > 0 || 
            tempPosi.y < 15 * 64 * -1)
        {
            return false;
        } 
        return true;
    }
}
