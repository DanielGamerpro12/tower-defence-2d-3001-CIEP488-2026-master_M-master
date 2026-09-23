using UnityEngine;
using UnityEngine.SceneManagement;

public class PannelPauseGame : MonoBehaviour
{
    public void ContinuarJogo()
    {
        Time.timeScale = 1;
        CanvasGameMng.Instance.AtivarPainel(EnumPaineisGame.Gameplay);
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Sair()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void ExibirConfiguracoes()
    {

    }

    

}
