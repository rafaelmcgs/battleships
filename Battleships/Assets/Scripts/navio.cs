using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class navio : MonoBehaviour
{
    public GameObject img;
    public GameObject quadGroup;
    public GameObject quadrado;
    public bool canDrag = true;
    public int size;
    public bool artilhariaSimples;
    public bool missilBalistico;
    public bool artilhariaPesada;
    public bool bombardeio;
    

    [HideInInspector]
    public int coudown = 0;
    public bool destroyed = false;
    
    private int managerId;
    private SetShipsManager manager;
    private GameObject[] quadrados;

    //variaveis do drag
    [HideInInspector]
    public bool isDraggin = false;
    private Vector3 dragMousePosiInicial;
    private Vector3 dragObjectPosiInicial;

    //variavel para identificar doubleClick
    private float doubleClickTimeLimit = 0.25f;
    public float doubleClickTimeCount = 0f;




    // Use this for initialization
    void Start () {
        createQuadrados(); //criar quadrados de fundo (aqueles verdes)
        

    }



    // Update is called once per frame
    void Update ()
    {
        //caso exista um click recente, aqui eu diminuo o tempo do contador
        if (doubleClickTimeCount != 0)
        {
            doubleClickTimeCount = doubleClickTimeCount - 0.01f;
            if (doubleClickTimeCount < 0)
            {
                doubleClickTimeCount = 0;
            }
        }
    }

    //funções para inicializacao
    public void setId(int id)
    {
        managerId = id;
    }
    public void setManager(SetShipsManager manager_)
    {
        manager = manager_;
    }

    //funções relacionadas a posição

    public int getX()
    {
        return Mathf.RoundToInt(transform.localPosition.x / 64);
    }
    public int getY()
    {
        int temp = Mathf.RoundToInt(transform.localPosition.y / 64) * -1;
        if (getVertical())
        {
            temp = temp -1;//1 a menos pois o archor point vai para o canto inferior esquerdo
        }
        return temp;
    }
    public bool getVertical()
    {
        if (transform.localEulerAngles.z == 90f)
        {
            return true;
        }
        return false;
    }
    private void snapPosition()
    {
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x / 64f) * 64f, Mathf.Round(transform.localPosition.y / 64f) * 64f, 0);
    }
    private void girarNavio()
    {
        transform.SetAsLastSibling();

        isDraggin = true; //ativo isDraggin para remover as sobreposições a seguir e logo apos eu desativo o draggin
        manager.RemoveShipPosition(managerId);
        isDraggin = false;

        Vector3 tempAngle;
        Vector3 tempPosi = transform.localPosition;
        if (getVertical())
        {
            tempAngle = new Vector3(0f, 0f, 0f);
            //centralizo o giro (vertical para horizontal)
            tempPosi.x = tempPosi.x - (64 * (Mathf.Ceil(size / 2)));
            tempPosi.y = tempPosi.y + (64 * (Mathf.Ceil(size / 2)+1));
        }
        else
        {
            tempAngle = new Vector3(0f, 0f, 90f);
            //centralizo o giro (horizontal para vertical)
            tempPosi.x = tempPosi.x + (64 * (Mathf.Ceil(size / 2)));
            tempPosi.y = tempPosi.y - (64 * (Mathf.Ceil(size / 2)+1));
        }
        transform.localEulerAngles = tempAngle;



        // verifico e corrijo posição de acordo com os limites do tabuleiro 15x15
        tempPosi = checkAndReturnPosition(tempPosi);
        transform.localPosition = tempPosi;


        //reseto quadrados deste navio
        hideQuadrados();
        showQuadrados();

        manager.SetShipPosition(managerId);
    }
    private Vector3 checkAndReturnPosition(Vector3 tempPosi_)
    {
        Vector3 tempPosi =  tempPosi_;
        // verificar limites do tabuleiro
        if (tempPosi.x < 0) { tempPosi.x = 0; }
        if (tempPosi.x + (64 * size) > (15 * 64) && !getVertical()) { tempPosi.x = (15 * 64) - (64 * size); }
        if (tempPosi.x > 14 * 64) { tempPosi.x = 14 * 64; } // 1 a menos, pois o archorpoint esta nocanto superior esquerdo


        if (tempPosi.y > 0) { tempPosi.y = 0; }
        if (tempPosi.y + (64 * size) > 0 && getVertical()) { tempPosi.y = 64 * size * -1; }
        if (tempPosi.y < 15 * 64 * -1 && getVertical()) { tempPosi.y = 15 * 64 * -1; }
        if (tempPosi.y < 14 * 64 * -1 && !getVertical()) { tempPosi.y = 14 * 64 * -1; } //1 a menos pois o archor point vai para o canto inferior esquerdo
        return tempPosi;
    }


    //funções relacionadas aos quadrados de fundo
    public void createQuadrados()
    {
        if (quadrados != null) { return; }
        quadrados = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            quadrados[i] = Instantiate(quadrado) as GameObject;
            quadrados[i].transform.parent = quadGroup.transform;
            quadrados[i].transform.localScale = new Vector3(1f, 1f, 1f);
            quadrados[i].transform.localPosition = new Vector3(64*i, 0f, 0f);
        }
    }
    public void setQuadradoRed(int num)
    {
        Image image = quadrados[num].GetComponent<Image>();
        var tempColor = image.color;
        tempColor.r = 1f;
        tempColor.g = 0f;
        tempColor.b = 0f;
        tempColor.a = 0.5f;
        image.color = tempColor;

    }
    public void setQuadradoYellow(int num)
    {
        Image image = quadrados[num].GetComponent<Image>();
        var tempColor = image.color;
        tempColor.r = 1f;
        tempColor.g = 1f;
        tempColor.b = 0f;
        tempColor.a = 0.5f;
        image.color = tempColor;

    }
    public void setQuadradoGreen(int num, bool recheck)
    {
        //como essa função pode ser chamada para remover o quadrado vermelho por sobreposição
        //coloquei um parametro para verificar se existe outra sobreposição com outro navio
        if (recheck)
        {
            manager.SetShipPosition(managerId);
        }
        Image image = quadrados[num].GetComponent<Image>();
        var tempColor = image.color;
        tempColor.r = 0f;
        tempColor.g = 1f;
        tempColor.b = 0f;
        tempColor.a = 0.5f;
        image.color = tempColor;

    }
   
    public void hideQuadrados()
    {
        Image image;
        for (int i = 0; i < size; i++)
        {
            image = quadrados[i].GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0f;

            //reset cor para verde
            tempColor.r = 0f;
            tempColor.g = 1f;
            image.color = tempColor;
        }
    }
    private void showQuadrados()
    {
        Image image;
        for (int i = 0; i < size; i++)
        {
            image = quadrados[i].GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0.5f;
            image.color = tempColor;
        }

    }
    public bool hasQuadRed()
    {
        Image image;
        for (int i = 0; i < size; i++)
        {
            image = quadrados[i].GetComponent<Image>();
            if (image.color.r == 1)
            {
                return true;
            }
        }
        return false;
    }
    public void setExposto(bool exposto_)
    {
            Image image;
            for (int i = 0; i < size; i++)
            {
                image = quadrados[i].GetComponent<Image>();
                var tempColor = image.color;
            if (exposto_)
            {
                tempColor.a = 0.5f;
                tempColor.r = 1f;
                tempColor.g = 1f;
                tempColor.b = 1f;
            }
            else
            {
                tempColor.a = 0f;

            }
                image.color = tempColor;
            }
        
    }


    //funções de interação
    public void beginDrag()
    {
        if (canDrag)
        {
            transform.SetAsLastSibling();
            isDraggin = true;
            manager.RemoveShipPosition(managerId);
            hideQuadrados();
            dragMousePosiInicial = Input.mousePosition;
            dragObjectPosiInicial = transform.localPosition;
        }
    }
    public void drag()
    {
        if (canDrag)
        {
            // estou trabalhando com local position, portanto eu corrigi o movimento em relação a escala do tabuleiro
            Vector3 temp = (Input.mousePosition - dragMousePosiInicial);
            temp.x = temp.x;
            temp.y = temp.y;

            // adiciono distancia pecorrida pelo mouse
            Vector3 tempPosi = dragObjectPosiInicial + temp;

            // verifico e corrijo posição de acordo com os limites do tabuleiro 15x15
            tempPosi = checkAndReturnPosition(tempPosi);



            transform.localPosition = tempPosi;
        }
    }
    public void drop()
    {
        if (canDrag)
        {
            isDraggin = false;
            snapPosition();
            manager.SetShipPosition(managerId);
            showQuadrados();
        }
    }
    public void MouseDown()
    {
        if (doubleClickTimeCount == 0)
        {
            doubleClickTimeCount = doubleClickTimeLimit;
        }
        else
        {
            girarNavio();
        }
    }


    //funções para setar e pegar infos
    public string getStringInfos()
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
        string verticalTemp = "0";
        if (getVertical())
        {
            verticalTemp = "1";
        }

        string quadradosLifeString = "";
        for (int i = 0; i < size; i++)
        {
            quadradosLifeString = quadradosLifeString + "15:";
        }
        quadradosLifeString = quadradosLifeString.Substring(0, quadradosLifeString.Length - 1);


        string retorno = size.ToString() + "#" + getX().ToString() + "#" + getY().ToString() + "#" + verticalTemp + "#0#0#0#" + quadradosLifeString;



        return retorno;
    }

    
    public void setQuadByInfos(string infos)
    {

        string[] quadArray = infos.Split(':');
        for (int i = 0; i < quadArray.Length; i++)
        {
            if (quadArray[i] == "0")
            {
                setQuadradoRed(i);
            }
            else if (quadArray[i] != "15")
            {
                setQuadradoYellow(i);
            }
        }
    }
    public void setDestroyed(bool destroyed_)
    {
        destroyed = destroyed_;
        if (destroyed_)
        {
            Image image = img.GetComponent<Image>();
            var tempColor = image.color;
            tempColor.r = 0.2f;
            tempColor.g = 0.2f;
            tempColor.b = 0.2f;
            image.color = tempColor;
            
        }

    }
}
