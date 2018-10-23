using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{




    private int playerNum;
    private GameObject[,] quadTabuleiroEnemy = new GameObject[15, 15];
    private GameObject[,] quadMeuTabuleiro = new GameObject[15, 15];

    public int artilhariaPesadaDano;
    public int artilhariaPesadaCountdown;
    public int missilDano;
    public int bombardeioDano;
    public int bombardeioCountdown;
    public int bombardeioTamanho;

    [HideInInspector]
    public int artilhariaSimplesDano = 0;
    [HideInInspector]
    public int missilMax = 0;
    [HideInInspector]
    public int artilhariaPesadaMax = 0;
    [HideInInspector]
    public int bombardeioMax = 0;

    private int missilNow = 0;
    private List<float> artilhariaPesadaNow = new List<float>();
    private List<float> bombardeioNow = new List<float>();
    private float artilhariaSimplesBar = 1;

    [HideInInspector]
    public int canhaoSelected = 0;
    private bool firstTargetSelected = false;
    private int[] firstTargetCoord = new int[2] {99,99 };
    private List<int[]> savedTargetsCoord = new List<int[]>();


    public GameObject tabuleiroMe;
    public GameObject tabuleiroEnemy;
    public GameObject helperText;

    public GameObject attack1;
    public GameObject attack2;
    public GameObject attack3;
    public GameObject attack4;

    public GameObject navio1Prefab;
    public GameObject navio2Prefab;
    public GameObject navio3Prefab;
    public GameObject navio4Prefab;
    public GameObject navio5Prefab;
    public GameObject quadrado;
    public GameObject Explosao;



    private GameObject tempShip; //utilizo para manusear prefabs, pois parece que não autoriza o uso de maneira local


    // Use this for initialization
    void Start ()
    {

        playerNum = PlayerPrefs.GetInt("turno");

        //diminuir countdown dos ataques
        naviosCountdownRefresh();

        //inserir quadrados
        inserirQuadrados();
        
        //constroe tabuleiros
        popularMeuTabuleiro();
        popularTabuleiroEnemy();
        colocarAtaques("1");
        colocarAtaques("2");

        //inicializa script do tabuleiro enemy
        tabuleiroEnemy.GetComponent<tabuleiroEnemy>().setManager(this) ;


        //inicializa ataques btn
        AttacksInit();
        //configura ataques btn
        AttacksConfigs();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //função para setar texto de ajuda
    public void setText(string texto)
    {
        Text textTemp = helperText.GetComponent<Text>();
        textTemp.text = texto;
        
    }

    //funções de construção dos tabuleiros
    private void inserirQuadrados()
    {
        //os quadrados são gameobjects que formam o fundo e sua função é colorir o mapa
        Image image;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                quadMeuTabuleiro[i, j] = Instantiate(quadrado) as GameObject;
                quadMeuTabuleiro[i, j].transform.parent = tabuleiroMe.transform;
                quadMeuTabuleiro[i, j].transform.localScale = new Vector3(1f, 1f, 1f);
                quadMeuTabuleiro[i, j].transform.localPosition = new Vector3(i * 64, j * -64, 0f);
                image = quadMeuTabuleiro[i, j].GetComponent<Image>();
                var colorTemp = image.color;
                colorTemp.a = 0;
                colorTemp.r = 0;
                colorTemp.g = 0;
                colorTemp.b = 0;
                image.color = colorTemp;

                quadTabuleiroEnemy[i, j] = Instantiate(quadrado) as GameObject;
                quadTabuleiroEnemy[i, j].transform.parent = tabuleiroEnemy.transform;
                quadTabuleiroEnemy[i, j].transform.localScale = new Vector3(1f, 1f, 1f);
                quadTabuleiroEnemy[i, j].transform.localPosition = new Vector3(i * 64, j * -64, 0f);
                image = quadTabuleiroEnemy[i, j].GetComponent<Image>();
                colorTemp = image.color;
                colorTemp.a = 0;
                colorTemp.r = 0;
                colorTemp.g = 0;
                colorTemp.b = 0;
                image.color = colorTemp;
            }
        }
    }
    private void popularMeuTabuleiro()
    {
        
        string[] myShipsInfos = PlayerPrefs.GetString("player" + playerNum.ToString()).Split('$');
        string[] shipInfo;
        
        float tempX;
        float tempY;
        float tempAngle;
        navio tempNavioComp;
        GameObject[] oldNavios = GameObject.FindGameObjectsWithTag("Navios");

        for (var i = 0; i < oldNavios.Length; i++)
        {
            DestroyImmediate(oldNavios[i]);
        }

        //inserir navios
        for (int i=0;i<myShipsInfos.Length;i++)
        {
            /* separado por #
            [
                0 = size (o que define o tipo de navio)
                1 = posicao X
                2 = posicao Y
                3 = posicao Vertical ( 0=horizontal | 1=vertical)
                4 = coutdown
                5 = exposto ( 0=n | 1=s)
                6 = destruido ( 0=n | 1=s)
                7 = array da vida dosquadrados // separado por :
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
            tempNavioComp.hideQuadrados();
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
        //inserir navios 100% destruidos ou expostos

        //pego a playerprefs dos navios do jogador inimigo, uma array separada pelo caracter $
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
            /* separado por #
            [
                0 = size (o que define o tipo de navio)
                1 = posicao X
                2 = posicao Y
                3 = posicao Vertical ( 0=horizontal | 1=vertical)
                4 = coutdown
                5 = exposto ( 0=n | 1=s)
                6 = destruido ( 0=n | 1=s)
                7 = array da vida dosquadrados // separado por :
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
    private void colocarAtaques(string playerNum_)
    {
        /* separado por $
         0 = x
         1 = y
         2 = tipo (0 = miss | 1 = acertou | 2 = destruido)
         */
        if (!PlayerPrefs.HasKey("player" + playerNum_ + "Ataques"))
        {
            PlayerPrefs.SetString("player" + playerNum_ + "Ataques", "");
        }
        if (PlayerPrefs.GetString("player" + playerNum_ + "Ataques") != "")
        {
            string[] ataques = PlayerPrefs.GetString("player" + playerNum_ + "Ataques").Split('$');
            string[] ataqueTemp;
            Image image;
            for (int i = 0; i < ataques.Length; i++)
            {
                ataqueTemp = ataques[i].Split('#');

                if (playerNum_ == "1" && playerNum == 1 || playerNum_ == "2" && playerNum == 2)
                {
                    image = quadTabuleiroEnemy[int.Parse(ataqueTemp[0]), int.Parse(ataqueTemp[1])].GetComponent<Image>();
                }
                else
                {
                    image = quadMeuTabuleiro[int.Parse(ataqueTemp[0]), int.Parse(ataqueTemp[1])].GetComponent<Image>();

                }
                var colorTemp = image.color;
                if (ataqueTemp[2] == "1")
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
    public void resetarAtaquesTabuleiro()
    {

        Image image;
        Color cores;
        for (int i = 0; i < savedTargetsCoord.Count; i++)
        {

            image = quadTabuleiroEnemy[savedTargetsCoord[i][0], savedTargetsCoord[i][1]].GetComponent<Image>();
            cores = image.color;
            cores.r = 0f;
            cores.g = 0f;
            cores.b = 0f;
            cores.a = 0f;
            image.color = cores;
        }
        savedTargetsCoord.Clear();
        colocarAtaques("1");
        colocarAtaques("2");
    }
    public void ativarQuadradosMira(int x, int y)
    {

        Image image;
        Color cores;
        int[] temp = new int[2];

        //reseto os quadrados

        List<int[]> quadradosParaAtivar = new List<int[]>();
        // são 3 situação de mira:
        if (canhaoSelected == 2) //missil - formato cruz
        {
            quadradosParaAtivar.Add(new int[2] { x, y });
            quadradosParaAtivar.Add(new int[2] { x + 1, y });
            quadradosParaAtivar.Add(new int[2] { x - 1, y });
            quadradosParaAtivar.Add(new int[2] { x, y + 1 });
            quadradosParaAtivar.Add(new int[2] { x, y - 1 });



        }
        else if (canhaoSelected == 4 && firstTargetSelected) //linha reta - bombardeio
        {
            int tempValue;
            if (
                firstTargetCoord[1] == y ||
                Mathf.Abs(firstTargetCoord[0] - x) >= Mathf.Abs(firstTargetCoord[1] - y)
            )
            {
                //linha horizontal

                for (int i = 0; i <= Mathf.Abs(firstTargetCoord[0] - x); i++)
                {
                    if (firstTargetCoord[0] < x)
                    {//movimento da esquerda para direita
                        tempValue = firstTargetCoord[0] + i;
                    }
                    else
                    {//movimento da direita para esquerda
                        tempValue = firstTargetCoord[0] - i;

                    }
                    quadradosParaAtivar.Add(new int[2] { tempValue, firstTargetCoord[1] });
                }
            }
            else
            {
                //linha vertical
                for (int i = 0; i <= Mathf.Abs(firstTargetCoord[1] - y); i++)
                {
                    if (firstTargetCoord[1] < y)
                    {//movimento de cima para baixo
                        tempValue = firstTargetCoord[1] + i;
                    }
                    else
                    {//movimento de baixo para cima
                        tempValue = firstTargetCoord[1] - i;

                    }
                    quadradosParaAtivar.Add(new int[2] { firstTargetCoord[0], tempValue });
                }


            }

        }
        else if (canhaoSelected != 0) //outra situação de canhão selecionado
        {
            quadradosParaAtivar.Add(new int[2] { x, y });
        }

        resetarAtaquesTabuleiro();
        //adiciono cor nos quadrados
        for (int i = 0; i < quadradosParaAtivar.Count; i++)
        {
            if (
                (
                    (canhaoSelected == 4 && i < bombardeioTamanho) ||
                    canhaoSelected != 4
                ) &&
                (quadradosParaAtivar[i][0] >= 0 && quadradosParaAtivar[i][0] < 15 &&
                quadradosParaAtivar[i][1] >= 0 && quadradosParaAtivar[i][1] < 15)
            )
            {
                savedTargetsCoord.Add(new int[2] { quadradosParaAtivar[i][0], quadradosParaAtivar[i][1] });
                image = quadTabuleiroEnemy[quadradosParaAtivar[i][0], quadradosParaAtivar[i][1]].GetComponent<Image>();
                cores = image.color;
                cores.g = 255;
                cores.a = 0.5f;
                image.color = cores;

            }
        }


    }




    //funções relacionadas aos ataques
    private void naviosCountdownRefresh()
    {

    }
    private void AttacksInit()
    {

        attack1.GetComponent<canhao>().SetShipsManager(this);
        attack2.GetComponent<canhao>().SetShipsManager(this);
        attack3.GetComponent<canhao>().SetShipsManager(this);
        attack4.GetComponent<canhao>().SetShipsManager(this);
    }
    private void AttacksConfigs()
    {

        string[] myShipsInfos = PlayerPrefs.GetString("player" + playerNum.ToString()).Split('$');
        string[] shipInfo;
        float pct;

        artilhariaSimplesDano = 0;
        missilMax = 0;
        missilNow = 0;
        artilhariaPesadaMax = 0;
        artilhariaPesadaNow = new List<float>();
        bombardeioMax = 0;
        bombardeioNow = new List<float>();

        //coleta informações dos navios relevantes a configuração
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
            //shipInfo[4] = "1";

            switch (int.Parse(shipInfo[0]))
            {
                case 2:
                    artilhariaSimplesDano++;
                break;
                case 3:
                    artilhariaSimplesDano++;
                    missilMax++;
                    if (int.Parse(shipInfo[4]) == 0)
                    {
                        missilNow++;
                    }
                    break;
                case 4:
                    artilhariaSimplesDano++;
                    artilhariaPesadaMax++;

                    pct = (float)(artilhariaPesadaCountdown - int.Parse(shipInfo[4])) / artilhariaPesadaCountdown;
                    artilhariaPesadaNow.Add(pct);
                    break;
                case 5:
                    artilhariaSimplesDano++;
                    bombardeioMax++;

                    pct = (float)(bombardeioCountdown - int.Parse(shipInfo[4])) / bombardeioCountdown;
                    bombardeioNow.Add(pct);
                    break;
            }
        }

        //seta quantidade maxima de cartuchos ou barras
        attack1.GetComponent<canhao>().setBarsQtd(1);
        attack2.GetComponent<canhao>().setCartuchosQtd(missilMax);
        attack3.GetComponent<canhao>().setBarsQtd(artilhariaPesadaMax);
        attack4.GetComponent<canhao>().setBarsQtd(bombardeioMax);

        //seta a quantidade
        Debug.Log("max" + missilMax.ToString());
        Debug.Log("now" + missilNow.ToString());
        attack1.GetComponent<canhao>().setBarsValues(new List<float>() { artilhariaSimplesBar });
        attack2.GetComponent<canhao>().removeCartuchos(missilMax,missilNow);
        
        attack3.GetComponent<canhao>().setBarsValues(artilhariaPesadaNow);
        attack4.GetComponent<canhao>().setBarsValues(bombardeioNow);


    }

    public void mouseClickAttack()
    {
        if (canhaoSelected == 4 && !firstTargetSelected)
        {
            //registrar primeiro botão e aguardar o segundo
            firstTargetSelected = true;
            firstTargetCoord[0] = savedTargetsCoord[0][0];
            firstTargetCoord[1] = savedTargetsCoord[0][1];
            //mudar texto

        }
        else
        {
            firstTargetSelected = false;
            lockAllMouseEvents(); //travo todos os eventos de mouse, para depois só liberar ao fim da animação
            //lancarataques
            int dano = 0;
            bool navioExpostoMy = false;
            bool navioExpostoEnemy = false;
            int countDown = 0;
            int navioTipo = 0;

            //definir as informações do ataque
            switch (canhaoSelected)
            {
                case 1: // artilharia simples
                    dano = artilhariaSimplesDano;
                    artilhariaSimplesBar = 0;
                    break;
                case 2: // missil
                    dano = missilDano;
                    //expor navio
                    navioExpostoMy = true;
                    navioTipo = 3;
                    countDown = 99;
                    break;
                case 3: // artilharia pesada
                    dano = artilhariaPesadaDano;
                    //expoe navio inimigo se acertar
                    navioExpostoEnemy = true;
                    navioTipo = 4;
                    countDown = artilhariaPesadaCountdown;
                    break;
                case 4: // bombardeio
                    dano = bombardeioDano;
                    Debug.Log(savedTargetsCoord.Count);
                    if (savedTargetsCoord.Count == 1) //cancelo o ataque pois clicou no mesmo quadrado
                    {
                        unLockAllMouseEvents(); 
                        return;
                    }
                    if (savedTargetsCoord.Count != bombardeioTamanho) //o jogador fez uma reta, porem não marcou 7 casa, então iremos completar
                    {
                        int faltando = bombardeioTamanho - savedTargetsCoord.Count;
                        for (int i=0;i < faltando;i++)
                        {
                            if (savedTargetsCoord[0][0] == savedTargetsCoord[1][0])//vertical
                            {
                                if (
                                     (savedTargetsCoord[0][1] < savedTargetsCoord[1][1] &&//movimento de cima para baixo
                                     savedTargetsCoord[savedTargetsCoord.Count-1][1]+1 <15)//com espaço embaixo
                                     || (savedTargetsCoord[0][1] > savedTargetsCoord[1][1] && //movimento de baixo para cima
                                     savedTargetsCoord[savedTargetsCoord.Count - 1][1] - 1 < 0) //porem sem espaço acima
                                ) {
                                    //adiciono quadrado abaixo
                                    savedTargetsCoord.Add(new int[2] { savedTargetsCoord[0][0], savedTargetsCoord[0][1]+1 });
                                }
                                else
                                {
                                    savedTargetsCoord.Add(new int[2] { savedTargetsCoord[0][0], savedTargetsCoord[0][1] - 1 });
                                    //adiciono quadrado cima

                                }
                            }
                            else//horizontal
                            {
                                if (
                                     (savedTargetsCoord[0][0] < savedTargetsCoord[1][0] &&//movimento de esquerda para direita
                                     savedTargetsCoord[savedTargetsCoord.Count - 1][0] + 1 < 15)//com espaço direita
                                     || (savedTargetsCoord[0][0] > savedTargetsCoord[1][0] && //movimento de direita para esquerda
                                     savedTargetsCoord[savedTargetsCoord.Count - 1][0] - 1 < 0) //porem sem espaço a esquerda
                                )
                                {
                                    //movimento da esquerda para direita
                                    savedTargetsCoord.Add(new int[2] { savedTargetsCoord[0][0]+1, savedTargetsCoord[0][1] });
                                }
                                else
                                {
                                    //movimento da direita para esquerda
                                    savedTargetsCoord.Add(new int[2] { savedTargetsCoord[0][0] - 1, savedTargetsCoord[0][1] });

                                }
                            }
                        }
                    }
                    navioTipo = 5;
                    countDown = bombardeioCountdown;
                    break;
            }
            

            registrarAtaque(dano, navioTipo, navioExpostoMy, navioExpostoEnemy, countDown);
            //configura ataques btn
            AttacksConfigs();
            //repopula tabuleiros
            popularMeuTabuleiro();
            popularTabuleiroEnemy();
            //reseta os quadrados dos tabuleiros
            resetarAtaquesTabuleiro();
            unselectAllAttacks();

        }

    }
    private void registrarAtaque(int dano, int navioTipo, bool navioExpostoMy, bool navioExpostoEnemy, int countDown)
    {
        if (navioTipo != 0)
        {
            AtualizarMeusNaviosInfos(navioTipo, navioExpostoMy, countDown);
        }

        string[,] newAtaquesToInsert = EfetuarDanoNoInimigo(dano, navioExpostoEnemy);

        SalvarAtaquesNoHistorico(newAtaquesToInsert);
        CriarExplosoes();
    }
    private void AtualizarMeusNaviosInfos(int navioTipo, bool navioExposto, int countDown)
    {
        string[] navio;
        string[] navioQuadrados;
        int melhorNavioIndex = -1;
        int maxQuadradosExpostos = -1;
        int tempCount = 0;

        string[] myShipsInfos = PlayerPrefs.GetString("player" + playerNum.ToString()).Split('$');


        //inserir info do ataque no navio do jogador
        //procurar a melhor opção de navio para alteração
        for (int i = 0; i < myShipsInfos.Length; i++)
        {
            navio = myShipsInfos[i].Split('#');
            if (
                navioTipo.ToString() == navio[0] && // é do msmo tipo
                navio[6] == "0" // não está destruido
            )
            {
                if (navioExposto && navio[5] == "0")
                {
                    //verifico se é melhor que opção anterior
                    tempCount = 0;
                    navioQuadrados = navio[7].Split(':');
                    for (int j = 0; j < navioQuadrados.Length; j++)
                    {
                        if (navioQuadrados[j] != "15")
                        {
                            tempCount++;
                        }
                    }
                    if (tempCount >= maxQuadradosExpostos)
                    {
                        maxQuadradosExpostos = tempCount;
                        melhorNavioIndex = i;
                    }
                }
                else if (!navioExposto && navio[4] == "0")
                {
                    melhorNavioIndex = i;
                }
            }
        }
        //alterar navio do jogador
        navio = myShipsInfos[melhorNavioIndex].Split('#');
        navio[4] = countDown.ToString();
        if (navioExposto)
        {
            navio[5] = "1";
        }
        myShipsInfos[melhorNavioIndex] = string.Join("#", navio);
        PlayerPrefs.SetString("player" + playerNum.ToString(), string.Join("$", myShipsInfos));
    }
    private string[,]  EfetuarDanoNoInimigo(int dano, bool navioExposto)
    {
        string[,] newAtaquesToInsert = new string[savedTargetsCoord.Count, 3];
        string[] enemyShipsInfos;
        int[] navioQuadradosCoord;
        string[] navio;
        string[] navioQuadrados;
        string EnemyId = "1";

        //inserir info do ataque no navio do inimigo e salvar o historico de ataque]

        if (playerNum == 1)
        {
            EnemyId = "2";
        }

        enemyShipsInfos = PlayerPrefs.GetString("player"+ EnemyId).Split('$');

        //verifico se acertou algum quadrado do navio e insiro dano
        for (int j = 0; j < savedTargetsCoord.Count; j++)
        {
            newAtaquesToInsert[j, 0] = savedTargetsCoord[j][0].ToString();
            newAtaquesToInsert[j, 1] = savedTargetsCoord[j][1].ToString();
            newAtaquesToInsert[j, 2] = "0"; //miss
            int destruidos = 0;
            for (int i = 0; i < enemyShipsInfos.Length; i++)
            {
                navio = enemyShipsInfos[i].Split('#');
                navioQuadrados = navio[7].Split(':');
                //verifico quais são os quadrados do navio e vejo se bate com o alvo
                navioQuadradosCoord = new int[2];
                destruidos = 0;
                for (int k = 0; k < int.Parse(navio[0]); k++)
                {

                    if (navio[3] == "0")//horizontal
                    {
                        navioQuadradosCoord[0] = int.Parse(navio[1]) + k;
                        navioQuadradosCoord[1] = int.Parse(navio[2]);
                    }
                    else //vertical
                    {
                        navioQuadradosCoord[0] = int.Parse(navio[1]);
                        navioQuadradosCoord[1] = int.Parse(navio[2]) - k;
                    }
                    if (navioQuadradosCoord[0] == savedTargetsCoord[j][0] && navioQuadradosCoord[1] == savedTargetsCoord[j][1])
                    {
                        if (navioExposto ) { //tiro de artilharia pesa expoe navio inimigo qd acertar
                            navio[5] = "1";
                        }
                        int newLife = int.Parse(navioQuadrados[k]) - dano;
                        newAtaquesToInsert[j, 2] = "1"; //acertou
                        if (newLife <= 0)
                        {
                            newLife = 0;
                            newAtaquesToInsert[j, 2] = "2"; //destruiu
                            destruidos++;
                        }
                        navioQuadrados[k] = newLife.ToString();
                    }
                    else if (navioQuadrados[k] == "0")
                    {
                        destruidos++;
                    }

                }
                //verifica se navio está destruido
                if (destruidos >= int.Parse(navio[0]))
                {
                    navio[6] = "1";
                }
                navio[7] = string.Join(":", navioQuadrados);
                enemyShipsInfos[i] = string.Join("#", navio);
            }
        }

        //salvar alterações nos navios do inimigo
        PlayerPrefs.SetString("player" + EnemyId, string.Join("$", enemyShipsInfos));

        return newAtaquesToInsert;
    }
    private void SalvarAtaquesNoHistorico(string[,] newAtaquesToInsert)
    {
        //salvar ataquehistorico
        string[] ataques;
        string[] ataqueTemp;
        string ataquesSave = "";

        if (PlayerPrefs.GetString("player" + playerNum.ToString() + "Ataques") != "")
        {
            ataques = PlayerPrefs.GetString("player" + playerNum.ToString() + "Ataques").Split('$');
        }
        else
        {
            ataques = new string[0];
        }
        for (int i = 0; i < savedTargetsCoord.Count; i++)
        {
            bool novaMarcacao = true;
            for (int j = 0; j < ataques.Length; j++)
            {
                ataqueTemp = ataques[j].Split('#');
                if (newAtaquesToInsert[i, 0] == ataqueTemp[0] && newAtaquesToInsert[i, 1] == ataqueTemp[1])
                {
                    ataques[j] = newAtaquesToInsert[i, 0] + "#" + newAtaquesToInsert[i, 1] + "#" + newAtaquesToInsert[i, 2];
                    novaMarcacao = false;
                }
            }
            if (novaMarcacao)
            {
                ataquesSave = ataquesSave + newAtaquesToInsert[i, 0] + "#" + newAtaquesToInsert[i, 1] + "#" + newAtaquesToInsert[i, 2] + "$";
            }
        }
        if (ataques.Length > 0)
        {
            ataquesSave = ataquesSave + string.Join("$", ataques);
        }
        else
        {
            ataquesSave = ataquesSave.Substring(0, ataquesSave.Length - 1);
        }
        PlayerPrefs.SetString("player" + playerNum.ToString() + "Ataques", ataquesSave);
    }
    private void CriarExplosoes()
    {

        GameObject temp;
        for (int i = 0; i < savedTargetsCoord.Count; i++)
        {

            temp = Instantiate(Explosao) as GameObject;
            temp.transform.parent = tabuleiroEnemy.transform;
            temp.transform.localScale = new Vector3(1f, 1f, 1f);
            temp.transform.localPosition = new Vector3((savedTargetsCoord[i][0] * 64)+32, (savedTargetsCoord[i][1] * -64) - 32, 0f);
        }
        
    }



    public void unselectAllAttacks()
    {
        setText("Escolha um ataque!");
        attack1.GetComponent<canhao>().unSelect();
        attack2.GetComponent<canhao>().unSelect();
        attack3.GetComponent<canhao>().unSelect();
        attack4.GetComponent<canhao>().unSelect();
        canhaoSelected = 0;
        firstTargetSelected = false;
        resetarAtaquesTabuleiro();
    }
    private void lockAllMouseEvents()
    {

    }
    private void unLockAllMouseEvents()
    {

    }
}
