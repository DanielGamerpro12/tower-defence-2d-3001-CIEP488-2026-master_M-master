using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DanoInimigo : MonoBehaviour
{
    public float vidaInimigo;
    public int valorInimigo;
    public Slider sldVidaInimigo;

    [Header("Som da Morte")]
    public AudioClip somMorte;

    private Animator animator;
    private bool estaMorto = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        sldVidaInimigo.maxValue = vidaInimigo;
        sldVidaInimigo.value = vidaInimigo;
    }

    public void EfetuarDanoAoInimigo(float valorDano)
    {
        if (CanvasGameMng.PannelGamePlay.FimDeJogo == true) return;

        // Impede que o inimigo receba dano após morrer
        if (estaMorto) return;

        vidaInimigo -= valorDano;

        if (vidaInimigo <= 0)
        {
            DestruirInimigo();
        }

        sldVidaInimigo.value = vidaInimigo;
    }

    public void DestruirInimigo()
    {
        if (estaMorto) return;

        estaMorto = true;
        vidaInimigo = 0;

        // SOM DA MORTE
        if (somMorte != null)
        {
            GameManager.Audios.TocarSFX(somMorte);
        }

        // Gerar Moedas para o Player
        CanvasGameMng.PannelGamePlay.AdicionarMoedas(valorInimigo);
        CanvasGameMng.PannelGamePlay.TotalInimigosMortosPeloJogador += 1;
        CanvasGameMng.PannelGamePlay.ContarInimigoMorto();

        // Congelar o movimento do inimigo
        InimigoIA ia = GetComponent<InimigoIA>();

        if (ia != null)
        {
            ia.CongelarInimigo();
        }

        // Escolher a animação de morte
        float direcaoX = animator.GetFloat("DirecaoX");

        if (direcaoX < -0.1f)
        {
            animator.SetInteger("DirecaoMorte", 1);
        }
        else if (direcaoX > 0.1f)
        {
            animator.SetInteger("DirecaoMorte", 2);
        }
        else
        {
            animator.SetInteger("DirecaoMorte", 0);
        }

        // Ativar a animação de morte
        animator.SetTrigger("Morte");

        // Esperar a animação terminar
        StartCoroutine(EsperarMorte());
    }

    private IEnumerator EsperarMorte()
    {
        // Aguarda a transição para o estado de morte
        yield return null;

        // Aguarda a animação terminar
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projetil"))
        {
            float valorDano = collision.GetComponent<ProjetilControle>().Dano;

            EfetuarDanoAoInimigo(valorDano);

            Destroy(collision.gameObject);
        }
    }

    public float CalcularDanoAoJogador()
    {
        return valorInimigo * Constants.PORCENTAGEM_DANO_INIMIGO;
    }
}