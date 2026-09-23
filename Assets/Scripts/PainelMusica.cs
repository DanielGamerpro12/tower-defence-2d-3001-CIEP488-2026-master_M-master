using UnityEngine;

public class PainelMusica : MonoBehaviour
{
    public enum TipoPainel
    {
        GameOver,
        NivelCompletado
    }

    public TipoPainel tipo;

    private void OnEnable()
    {
        if (GameManager.Audios == null)
            return;

        if (tipo == TipoPainel.GameOver)
        {
            GameManager.Audios.TocarMusicaGameOver();
        }
        else if (tipo == TipoPainel.NivelCompletado)
        {
            GameManager.Audios.TocarMusicaNivelCompletado();
        }
    }
}