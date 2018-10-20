using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject tabuleiroMe;
    public GameObject tabuleiroEnemy;

    public GameObject attack1;
    public GameObject attack2;
    public GameObject attack3;
    public GameObject attack4;

    public GameObject navio1Prefab;
    public GameObject navio2Prefab;
    public GameObject navio3Prefab;
    public GameObject navio4Prefab;
    public GameObject navio5Prefab;
    public GameObject quadEnemy;


    private int playerNum;
    private GameObject[,] quadTabuleiroEnemy = new GameObject[15,15];


    private GameObject tempShip; //utilizo para manusear prefabs, pois parece que não autoriza o uso de maneira local


    // Use this for initialization
    void Start ()
    {

        playerNum = PlayerPrefs.GetInt("turno", 1);


        colocarMeusNavios();

        popularTabuleiroEnemy();
        colocarMeusAtaques();

        attack1.GetComponent<canhao>().SetShipsManager(this);
        attack2.GetComponent<canhao>().SetShipsManager(this);
        attack3.GetComponent<canhao>().SetShipsManager(this);
        attack4.GetComponent<canhao>().SetShipsManager(this);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void colocarMeusNavios()
    {

        string[] myShipsInfos = PlayerPrefs.GetString("player" + playerNum.ToString()).Split('$');
        string[] shipInfo;

        float tempX;
        float tempY;
        float tempAngle;
        navio tempNavioComp;

        for (int i=0;i<myShipsInfos.Length;i++)
        {
            /*
            [
                0 = size (o que define o tipo de navio)
                1 = posicao X
                2 = posicao Y
                3 = posicao Vertical ( 0=horizontal | 1=vertical)
                4 = coutdown
                5 = exposto ( 0=n | 1=s)
                6 = destruido ( 0=n | 1=s)
                7 = array da vida dosquadrados
            ]
            */
            shipInfo = myShipsInfos[i].Split('#');

            switch (int.Parse(shipInfo[0]))
            {
                case 1: tempShip = Instantiate(navio1Prefab) as GameObject; break;
                case 2: tempShip = Instantiate(navio2Prefab) as GameObject; break;
                case 3: tempShip = Instantiate(navio3Prefab) as GameObject; break;
                case 4: tempShip = Instantiate(navio4Prefab) as GameObject; break;
                case 5: tempShip = Instantiate(navio5Prefab) as GameObject; break;
            }

            tempNavioComp = tempShip.GetComponent<navio>();
            tempNavioComp.canDrag = false;

            //forçar criar quadrados
            tempNavioComp.createQuadrados();
            //setar exposto
            if(int.Parse(shipInfo[5]) == 1)
            {
                tempNavioComp.setExposto(true);
            }
            else
            {
                tempNavioComp.setExposto(false);
            }
            //setar destruido
            if (int.Parse(shipInfo[6]) == 1)
            {
                tempNavioComp.setDestroyed(true);
            }
            else
            {
                tempNavioComp.setDestroyed(false);
            }
            //setar quadrados
            tempNavioComp.setQuadByInfos(shipInfo[7]);

            //setar posição
            tempX = 64 * int.Parse(shipInfo[1]);
            tempY = 64 * int.Parse(shipInfo[2]) * -1;
            tempAngle = 0f;
            //correção da posição y se estiver vertical, pois o archorpoint esta no canto inferior esquerdo
            if (int.Parse(shipInfo[3]) == 1)
            {
                tempY = 64 * (int.Parse(shipInfo[2]) - 1) * -1;
                tempAngle = 90f;
            }
            tempShip.transform.parent = tabuleiroMe.transform;
            tempShip.transform.localScale = new Vector3(1f, 1f, 1f);
            tempShip.transform.localPosition = new Vector3(tempX, tempY, 0f);
            tempShip.transform.localEulerAngles = new Vector3(0f, 0f, tempAngle);



        }
    }
    private void popularTabuleiroEnemy()
    {
        //inserir quadrados
        for(int i =0; i < 15; i++)
        {
            for (int j=0;j<15;j++)
            {
                quadTabuleiroEnemy[i,j] = Instantiate(quadEnemy) as GameObject;
                quadTabuleiroEnemy[i, j].transform.parent = tabuleiroEnemy.transform;
                quadTabuleiroEnemy[i, j].transform.localScale = new Vector3(1f, 1f, 1f);
                quadTabuleiroEnemy[i, j].transform.localPosition = new Vector3(i*64, j*-64, 0f);
            }
        }

        //inserir navios 100% destruidos ou expostos
        string[] myShipsInfos = PlayerPrefs.GetString("player1").Split('$');
        if (playerNum == 1)
        {
            myShipsInfos = PlayerPrefs.GetString("player2").Split('$');
        }
        string[] shipInfo;

        float tempX;
        float tempY;
        float tempAngle;
        navio tempNavioComp;

        for (int i = 0; i < myShipsInfos.Length; i++)
        {
            /*
            [
                0 = size (o que define o tipo de navio)
                1 = posicao X
                2 = posicao Y
                3 = posicao Vertical ( 0=horizontal | 1=vertical)
                4 = coutdown
                5 = exposto ( 0=n | 1=s)
                6 = destruido ( 0=n | 1=s)
                7 = array da vida dosquadrados
            ]
            */
            shipInfo = myShipsInfos[i].Split('#');
            //verifico se navio está exposto ou destruido
            if (int.Parse(shipInfo[5]) == 1 || int.Parse(shipInfo[6])==1)
            {
                switch (int.Parse(shipInfo[0]))
                {
                    case 1: tempShip = Instantiate(navio1Prefab) as GameObject; break;
                    case 2: tempShip = Instantiate(navio2Prefab) as GameObject; break;
                    case 3: tempShip = Instantiate(navio3Prefab) as GameObject; break;
                    case 4: tempShip = Instantiate(navio4Prefab) as GameObject; break;
                    case 5: tempShip = Instantiate(navio5Prefab) as GameObject; break;
                }

                tempNavioComp = tempShip.GetComponent<navio>();
                tempNavioComp.canDrag = false;

                //forçar criar quadrados
                tempNavioComp.createQuadrados();
                tempNavioComp.hideQuadrados();
                //setar destruido
                if (int.Parse(shipInfo[6]) == 1)
                {
                    tempNavioComp.setDestroyed(true);
                }
                else
                {
                    tempNavioComp.setDestroyed(false);
                }

                //setar posição
                tempX = 64 * int.Parse(shipInfo[1]);
                tempY = 64 * int.Parse(shipInfo[2]) * -1;
                tempAngle = 0f;
                //correção da posição y se estiver vertical, pois o archorpoint esta no canto inferior esquerdo
                if (int.Parse(shipInfo[3]) == 1)
                {
                    tempY = 64 * (int.Parse(shipInfo[2]) - 1) * -1;
                    tempAngle = 90f;
                }
                tempShip.transform.parent = tabuleiroEnemy.transform;
                tempShip.transform.localScale = new Vector3(1f, 1f, 1f);
                tempShip.transform.localPosition = new Vector3(tempX, tempY, 0f);
                tempShip.transform.localEulerAngles = new Vector3(0f, 0f, tempAngle);
            }


        }

    }
    private void colocarMeusAtaques()
    {
        /*
         0 = x
         1 = y
         2 = tipo (0 = miss | 1 = acertou | 2 = destruido)
         */
        
        if (!PlayerPrefs.HasKey("player" + playerNum.ToString()+"Ataques"))
        {
            PlayerPrefs.SetString("player" + playerNum.ToString() + "Ataques", "");
        }
        if (PlayerPrefs.GetString("player" + playerNum.ToString() + "Ataques") != "")
        {
            string[] ataques = PlayerPrefs.GetString("player" + playerNum.ToString() + "Ataques").Split('$');
            string[] ataqueTemp;
            Image image;
            for (int i = 0; i< ataques.Length;i++)
            {
                ataqueTemp = ataques[i].Split('#');


                image = quadTabuleiroEnemy[int.Parse(ataqueTemp[0]), int.Parse(ataqueTemp[1])].GetComponent<Image>();
                var colorTemp = image.color;
                if(ataqueTemp[2] == "1")
                {
                    colorTemp.r = 1f;
                    colorTemp.g = 1f;
                }
                else if (ataqueTemp[2] == "2")
                {
                    colorTemp.r = 1f;
                }

                colorTemp.a = 0.5f;
                image.color = colorTemp;

            }
        }
    }

    public void unselectAllAttacks()
    {
        attack1.GetComponent<canhao>().unSelect();
        attack2.GetComponent<canhao>().unSelect();
        attack3.GetComponent<canhao>().unSelect();
        attack4.GetComponent<canhao>().unSelect();
    }
}
