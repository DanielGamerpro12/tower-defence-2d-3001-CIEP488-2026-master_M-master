using UnityEngine;


public class InimigoIA : MonoBehaviour
{
    public float velocidade;

    private Waypoint destino;
    private bool habilitaMovimentacao;
    private float velocidadeOriginal;

    [Header("Som da Marcha")]
    public AudioClip somPasso;

    private bool contandoComoAndando = false;

    private Animator animator;

    private void Start()
    {
        velocidadeOriginal = velocidade;

        animator = GetComponent<Animator>();
    }

    public void DefinirNovoDestino(Waypoint novoDestino)
    {
        destino = novoDestino;
        habilitaMovimentacao = true;

        // Informa ao AudioMng que existe um inimigo andando
        if (!contandoComoAndando)
        {
            GameManager.Audios.IniciarMarcha(somPasso);
            contandoComoAndando = true;
        }
    }

    void Update()
    {
        if (habilitaMovimentacao == false)
            return;

        // Descobre a direção até o próximo waypoint
        Vector2 direcao = (destino.transform.position - transform.position).normalized;

        // Envia a direção horizontal para o Animator
        animator.SetFloat("DirecaoX", direcao.x);

        transform.position = Vector2.MoveTowards(
            transform.position,
            destino.transform.position,
            velocidade * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, destino.transform.position) < 0.01f)
        {
            if (destino == destino.ObterProximoDestino())
            {
                habilitaMovimentacao = false;

                // Informa que esse inimigo parou
                PararContagemMarcha();
            }
            else
            {
                destino = destino.ObterProximoDestino();
            }
        }
    }

    private void PararContagemMarcha()
    {
        if (contandoComoAndando)
        {
            GameManager.Audios.PararMarcha();
            contandoComoAndando = false;
        }
    }

    public void CongelarInimigo()
    {
        // Se estava andando, deixa de contar como inimigo andando
        if (velocidade > 0 && contandoComoAndando)
        {
            PararContagemMarcha();
        }

        velocidade = 0;
    }

    public void DescongelarInimigo()
    {
        velocidade = velocidadeOriginal;

        // Se ainda deveria estar se movimentando, volta a contar
        if (habilitaMovimentacao && !contandoComoAndando)
        {
            GameManager.Audios.IniciarMarcha(somPasso);
            contandoComoAndando = true;
        }
    }

    private void OnDestroy()
    {
        // Caso o inimigo seja destruído enquanto estiver andando
        PararContagemMarcha();
    }
}