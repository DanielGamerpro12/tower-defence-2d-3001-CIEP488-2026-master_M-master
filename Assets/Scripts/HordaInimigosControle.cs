using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordaInimigosControle : MonoBehaviour
{
    public CanvasMapaMng mapaMng;
    public float tempoNovoInimigo;
    public List<InimigoNivel> inimigosDoNivel;
    public GameObject hordaInicio;
    private float tempoProximoInimigo;
    private int contagemInimigosInstanciados = 0;
    private int maximoInimigosMapa = 0;
    private List<GameObject> listaInimigosInstanciados;
    private bool tempoCongelado;

    public int MaximoInimigosMapa
    {
        get { return maximoInimigosMapa; }
    }

    public int ContagemInimigos
    {
        get { return contagemInimigosInstanciados; }
    }

    private void Awake()
    {
        contagemInimigosInstanciados = 0;
        foreach (var inimigo in inimigosDoNivel)
        {
            maximoInimigosMapa += inimigo.quantidade;
        }
        listaInimigosInstanciados = new List<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempoProximoInimigo = Time.timeSinceLevelLoad + tempoNovoInimigo + Constants.TEMPO_ESPERA_INICIAL_GAMEPLAY;
        CanvasGameMng.PannelGamePlay.AtualizarInimigoUI(contagemInimigosInstanciados, maximoInimigosMapa);
    }

    // Update is called once per frame
    void Update()
    {
        if (tempoCongelado == true) return;
        //Lógica do instanciamento dos inimigos
        if (Time.timeSinceLevelLoad > tempoProximoInimigo && inimigosDoNivel.Count>0) {
            tempoProximoInimigo = Time.timeSinceLevelLoad + tempoNovoInimigo;

            var inimigoId = new System.Random().Next(0, inimigosDoNivel.Count);
            inimigosDoNivel[inimigoId].totalInstanciados++;
            var novoInimigo = Instantiate(inimigosDoNivel[inimigoId].inimigo);
            novoInimigo.GetComponent<InimigoIA>().DefinirNovoDestino(mapaMng.primeiroDestino);
            novoInimigo.transform.position = hordaInicio.transform.position;
            contagemInimigosInstanciados++;
            listaInimigosInstanciados.Add(novoInimigo);
            CanvasGameMng.PannelGamePlay.AtualizarInimigoUI(contagemInimigosInstanciados, maximoInimigosMapa);
            if (inimigosDoNivel[inimigoId].quantidade == inimigosDoNivel[inimigoId].totalInstanciados)
            {
                inimigosDoNivel.Remove(inimigosDoNivel[inimigoId]);
            }
        }
    }

    private void RemoverInimigoLista(GameObject inimigo)
    {
        if (listaInimigosInstanciados.Contains(inimigo))
        {
            listaInimigosInstanciados.Remove(inimigo);
        }
    }

    public void DestruirInimigosInstanciados()
    {
        foreach(var inimigo in listaInimigosInstanciados.ToList())
        {
            if (inimigo.activeSelf == false) return;
            RemoverInimigoLista(inimigo);
            inimigo.GetComponent<DanoInimigo>().DestruirInimigo();
        }
    }

    public void CongelarInimigos()
    {
        tempoCongelado = true;
        StartCoroutine(TempoCongelamentoInimigos());
    }

    IEnumerator TempoCongelamentoInimigos()
    {
        foreach(var inimigo in listaInimigosInstanciados)
        {
            inimigo.GetComponent<InimigoIA>().CongelarInimigo();
        }

        yield return new WaitForSeconds(Constants.TEMPO_CONGELAMENTO_INIMIGOS);

        foreach(var inimigo in listaInimigosInstanciados)
        {
            inimigo.GetComponent<InimigoIA>().DescongelarInimigo();
        }

        tempoCongelado = false;
    }
}
