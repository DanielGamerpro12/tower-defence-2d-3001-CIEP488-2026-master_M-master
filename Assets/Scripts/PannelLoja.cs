using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PannelLoja : MonoBehaviour
{
    
    public GameObject itemPoder;
    public GameObject itemTorre;
    public Transform contentTorre;
    public Transform contentPoder;
    public TextMeshProUGUI txtMoedas;
    public GameObject pnlConfirmacaoCompra;
    public List<GameObject> listaTorres = new List<GameObject>(); //Referencia dos itens no content
    public List<GameObject> listaPoderes = new List<GameObject>();
    private int moedasJogador;
    private void OnEnable()
    {
        AtualizarLoja();
    }

    public void AtualizarLoja()
    {
        AtualizarMoedas();
        //Verificar se a lista de torres e poderes já foi preenchida
        if (listaTorres.Count > 0)
        {
            // Se a lista tiver preenchida, eu devo apagar todos os elementos e inserir novamente
            foreach (GameObject item in listaTorres)
            {
                Destroy(item);
            }            
        }
        listaTorres.Clear();

        if (listaPoderes.Count > 0)
        {
            foreach (GameObject item in listaPoderes)
            {
                Destroy(item);
            }            
        }
        listaPoderes.Clear();

        //preencher a lista com os dados atualizados
        foreach (TorreSO torreSO in GameManager.GameData.torres)
        {
            GameObject item = Instantiate(itemTorre, contentTorre);
            item.GetComponent<ItemTorre>().Init(torreSO.torre, torreSO.icone);
            listaTorres.Add(item);
        }

        foreach (PoderSO poderSO in GameManager.GameData.poderes)
        {
            //Atribuir o poder ao player
            DBMng.InserirPoderesPlayer(poderSO.poder);

            //Atualizar a quantidade do poder na loja
            Poder poderAtualizado = DBMng.BuscarPoderPlayer(poderSO.poder.id);

            GameObject item = Instantiate(itemPoder, contentPoder);
            item.GetComponent<ItemPoder>().Init(poderAtualizado, poderSO.icone);
            listaPoderes.Add(item);
        }
    }

    public void ComprarTorre(Torre novaTorre)
    {
        pnlConfirmacaoCompra.SetActive(true);
        pnlConfirmacaoCompra.GetComponent<PannelConfirmacaoCompra>().Init(novaTorre);
    }

    public void ComprarPoder(Poder novoPoder)
    {
        pnlConfirmacaoCompra.SetActive(true);
        pnlConfirmacaoCompra.GetComponent<PannelConfirmacaoCompra>().Init(novoPoder);
    }

    public void AtualizarMoedas()
    {
        moedasJogador = DBMng.ObterMoedasPlayer();
        txtMoedas.text = $"${moedasJogador}";
    }
}
