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
    [HideInInspector]
    public bool enabled = true;


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
        if (!enabled) { return; }
        if (selected)
        {
            manager.setText("Escolha um ataque!");
            selected = false;
            GetComponent<Image>().sprite = imageNormal;
            manager.canhaoSelected = 0;

        }
        else
        {
            manager.unselectAllAttacks();
            selected = true;
            GetComponent<Image>().sprite = imageSelected;
            switch (tipo)
            {
                case 1: manager.setText("Artilharia Simples (1x1)\nDano: "+manager.artilhariaSimplesDano.ToString()+"\n1 tiro por rodada"); break;
                case 2: manager.setText("Míssil balístico (3x3)\nDano: "+ manager.missilDano.ToString() + "\nDisparo Limitado\nExpõe uma fragata"); break;
                case 3: manager.setText("Artilharia Pesada (1x1)\nDano: " + manager.artilhariaPesadaDano.ToString() + "\nCountdown: " + manager.artilhariaPesadaCountdown.ToString() + ""); break;
                case 4: manager.setText("Bombardeio (1x" + manager.bombardeioTamanho.ToString() + ")\nDano: " + manager.bombardeioDano.ToString() + "\nCountdown: " + manager.bombardeioCountdown.ToString() + "\nSelecione duas pontas de uma reta"); break;
            }
            manager.canhaoSelected = tipo;

        }
    }

    public void unSelect()
    {

        selected = false;
        GetComponent<Image>().sprite = imageNormal;
    }

    public void setCartuchosQtd(int num)
    {
        clearCartuchos();
        for (int i=0;i<num;i++)
        {
            cartuchos.Add(Instantiate(bar) as GameObject);
            cartuchos[i].transform.parent = transform;
            cartuchos[i].transform.localScale = new Vector3(1f, 1f, 1f);
            RectTransform rt = cartuchos[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(60, 42);
            cartuchos[i].transform.localPosition = new Vector3(i*70f, -208f, 0f);
            cartuchos[i].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            Image image = cartuchos[i].GetComponent<Image>();
            var tempColor = image.color;
            tempColor.g = 1f;
            image.color = tempColor;
        }

    }
    public void removeCartuchos(int missilMax, int missilNow)
    {
        for (int i = missilMax-1; i>= missilNow; i--)
        {
            DestroyImmediate(cartuchos[i]);
            cartuchos.RemoveAt(i);

        }
        if (cartuchos.Count == 0)
        {
            enabled = false;
        }
        else
        {
            enabled = true;

        }
    }
    private void clearCartuchos()
    {
        for (int i = 0; i < cartuchos.Count; i++)
        {
            DestroyImmediate(cartuchos[cartuchos.Count - 1]);
            cartuchos.RemoveAt(cartuchos.Count - 1);

        }

    }
    public void setBarsQtd(int num)
    {
        clearCartuchos();

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
            Image image = cartuchos[i].GetComponent<Image>();
            var tempColor = image.color;
            tempColor.g = 1f;
            image.color = tempColor;
        }
    }
    public void setBarsValues(List<float>barsValues)
    {
        enabled = false;
        for(int i = 0; i < barsValues.Count; i++)
        {
            GameObject theBar = cartuchos[i];
            var theBarRectTransform = theBar.transform as RectTransform;
            theBarRectTransform.sizeDelta = new Vector2(350 * barsValues[i], theBarRectTransform.sizeDelta.y);
            Image image = cartuchos[i].GetComponent<Image>();
            var tempColor = image.color;
            if (barsValues[i] == 1f)
            {
                enabled = true;
            }
            if (barsValues[i] == 1f)
            {
                tempColor.g = 1f;
                tempColor.r = 0f;
            }
            else
            {
                tempColor.r = 1f;
                tempColor.g = 0f;

            }
            image.color = tempColor;

        }
    }
}
