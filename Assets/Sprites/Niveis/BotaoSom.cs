using UnityEngine;

public class BotaoSom : MonoBehaviour
{
    public AudioClip somClique;

    public void TocarSom()
    {
        GameManager.Audios.TocarSFX(somClique);
    }

    public void Sair()
    {
        Application.Quit();
    }
}