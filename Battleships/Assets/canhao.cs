using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canhao : MonoBehaviour
{
    public int tipo;

    public Sprite imageNormal;
    public Sprite imageSelected;
    public GameObject bar;

    private bool selected = false;
    private BattleManager manager;
    private List<GameObject> cartuchos = new List<GameObject>();


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
            manager.setText("Escolha um ataque!");
            selected = false;
            GetComponent<Image>().sprite = imageNormal;
        }
        else
        {
            manager.unselectAllAttacks();
            selected = true;
            GetComponent<Image>().sprite = imageSelected;
            switch (tipo)
            {
                case 1: manager.setText("Artilharia Simples (1x1)\nDano: "+manager.artilhariaSimplesDano.ToString()+" x Navios\n1 tiro por rodada"); break;
                case 2: manager.setText("Míssil balístico (3x3)\nDano: "+ manager.missilDano.ToString() + "\nDisparo Limitado\nExpõe uma fragata"); break;
                case 3: manager.setText("Artilharia Pesada (1x1)\nDano: " + manager.artilhariaPesadaDano.ToString() + "\nCountdown: " + manager.artilhariaPesadaCountdown.ToString() + ""); break;
                case 4: manager.setText("Bombardeio (1x" + manager.bombardeioTamanho.ToString() + ")\nDano: " + manager.bombardeioDano.ToString() + "\nCountdown: " + manager.bombardeioCountdown.ToString() + "\nSelecione duas pontas de uma reta"); break;
            }

        }
    }

    public void unSelect()
    {

        selected = false;
        GetComponent<Image>().sprite = imageNormal;
    }

    public void setCartuchosQtd(int num)
    {
        for (int i=0;i<num;i++)
        {
            cartuchos.Add(Instantiate(bar) as GameObject);
            cartuchos[i].transform.parent = transform;
            cartuchos[i].transform.localScale = new Vector3(1f, 1f, 1f);
            RectTransform rt = cartuchos[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(60, 42);
            cartuchos[i].transform.localPosition = new Vector3(i*70f, -208f, 0f);
            cartuchos[i].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }

    }
    public void removeCartuchos(int num)
    {
        for (int i = 0; i< num; i++)
        {
            Destroy(cartuchos[cartuchos.Count-1]);
            cartuchos.RemoveAt(cartuchos.Count - 1);

        }
    }
    public void setBarsQtd(int num)
    {
        

        float barHeight = Mathf.Max(20,40/ num);


        for (int i = 0; i < num; i++)
        {
            cartuchos.Add(Instantiate(bar) as GameObject);
            cartuchos[i].transform.parent = transform;
            cartuchos[i].transform.localScale = new Vector3(1f, 1f, 1f);
            RectTransform rt = cartuchos[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(350, barHeight);
            cartuchos[i].transform.localPosition = new Vector3(0, -230 + (i*25), 0f);
            cartuchos[i].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    public void setBarsValues(List<float>barsValues)
    {
        for(int i = 0; i < barsValues.Count; i++)
        {
            GameObject theBar = cartuchos[i];
            var theBarRectTransform = theBar.transform as RectTransform;
            theBarRectTransform.sizeDelta = new Vector2(350 * barsValues[i], theBarRectTransform.sizeDelta.y);

        }
    }
}
