using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShipsManager : MonoBehaviour {

    public GameObject btnPronto;
    public GameObject avatar;
    public GameObject avatarBorda;
    public GameObject conteudoBorda;
    public GameObject water;
    public GameObject grid;
    public GameObject titulo;
    public GameObject texto;
    
    private GameObject[] naviosObjs;

    private void Start()
    {
        //registro os navios
        naviosObjs = GameObject.FindGameObjectsWithTag("Navios");
        navio navio;
        for(int i = 0; i < naviosObjs.Length; i++)
        {
            naviosObjs[i].SetActive(false);
            navio = naviosObjs[i].GetComponent<navio>();
            navio.setManager(this);
        }

        //desativo todos os objetos e deixo só o avatarBorder e o tabuleioBorda, que iniciam as  sequencias de animação
        texto.SetActive(false);
        titulo.SetActive(false);
        btnPronto.SetActive(false);
        avatar.SetActive(false);
        conteudoBorda.SetActive(false);
        water.SetActive(false);
        grid.SetActive(false);
    }
    public int[,] GetShipQuadrados(int x, int y, bool vertical, int size)
    {
        int temp = 0;
        int[,] quadrados = new int[size, 2];
        int iMax = 1;
        int jMax = 1;
        if (vertical)
        {
            jMax = size;
        }
        else
        {
            iMax = size;
        }
        temp = 0;
        for (int i = 0; i < iMax; i++)
        {
            for (int j = 0; j < jMax; j++)
            {
                quadrados[temp, 0] = x + i;
                quadrados[temp, 1] = y - j; // pois se vertical o archor pointer fica na base
                temp = temp + 1;
            }
        }


        return quadrados;
    }
    public void SetShipPosition(int id)
    {
        navio navio;

        //defino os quadrados a serem ocupados
        navio = naviosObjs[id].GetComponent<navio>();
        int[,] quadrados = GetShipQuadrados(navio.getX(), navio.getY(), navio.getVertical(), navio.size);

        //marco os quadrados que estiverem sobrepondo 
        int[,] quadradosTemp;
        navio navioTemp;
        for (int i = 0; i < naviosObjs.Length; i++)
        {
            if (i != id)
            {
                navioTemp = naviosObjs[i].GetComponent<navio>();
                quadradosTemp = GetShipQuadrados(navioTemp.getX(), navioTemp.getY(), navioTemp.getVertical(), navioTemp.size);

                for (int j =0; j<quadrados.Length;j++)
                {
                    for(int jT=0; jT < quadradosTemp.Length; j++)
                    {
                        if (quadrados[j,0] == quadradosTemp[jT,0] && quadrados[j, 1] == quadradosTemp[jT, 1])
                        {
                            navio.setQuadradoRed(j);
                            navioTemp.setQuadradoRed(jT);
                        }
                    }
                }

            }
        }
    }

    public void showShips()
    {
        GameObject[] navios = GameObject.FindGameObjectsWithTag("Navios");
    }
    
}
