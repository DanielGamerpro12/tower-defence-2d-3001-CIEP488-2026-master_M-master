using UnityEngine;

public class ProjetilControle : MonoBehaviour
{
    private float dano;

    [Header("Som do Tiro")]
    public AudioClip somTiro;

    public float Dano
    {
        get { return dano; }
    }

    public void Init(float porcentagemDano)
    {
        dano = Constants.VALOR_PADRAO_DANO_PROJETIL * porcentagemDano;

        // Tocar som do tiro
        if (somTiro != null)
        {
            GameManager.Audios.TocarSFX(somTiro);
        }
    }
}