using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMng : MonoBehaviour
{
    public AudioSource audioMusica;
    public AudioSource audioSFX;
    public AudioSource audioMarcha;

    [Header("Músicas")]
    public AudioClip musicaMenu;
    public AudioClip musicaFase;
    public AudioClip musicaGameOver;
    public AudioClip musicaNivelCompletado;

    private int inimigosAndando = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TrocarMusicaPorCena(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        TrocarMusicaPorCena(cena.name);
    }

    private void TrocarMusicaPorCena(string nomeCena)
    {
        AudioClip musicaNova;

        if (nomeCena == "Menu")
        {
            musicaNova = musicaMenu;
        }
        else
        {
            musicaNova = musicaFase;
        }

        TocarMusica(musicaNova);
    }

    public void TocarMusicaGameOver()
    {
        TocarMusica(musicaGameOver);
    }

    public void TocarMusicaNivelCompletado()
    {
        TocarMusica(musicaNivelCompletado);
    }

    public void TocarMusica(AudioClip musica)
    {
        if (musica == null)
            return;

        if (audioMusica.clip == musica && audioMusica.isPlaying)
            return;

        audioMusica.Stop();

        audioMusica.clip = musica;

        // Músicas de Game Over e Nível Completado tocam apenas uma vez
        audioMusica.loop = false;

        audioMusica.Play();
    }

    public void AtualizarVolumes(float volumeMusica, float volumeSFX)
    {
        audioMusica.volume = volumeMusica;
        audioSFX.volume = volumeSFX;
        audioMarcha.volume = volumeSFX;

        DBMng.SalvarVolumes(volumeMusica, volumeSFX);
    }

    public void TocarSFX(AudioClip som)
    {
        if (som != null)
        {
            audioSFX.PlayOneShot(som);
        }
    }

    public void IniciarMarcha(AudioClip som)
    {
        inimigosAndando++;

        if (!audioMarcha.isPlaying)
        {
            audioMarcha.clip = som;
            audioMarcha.loop = true;
            audioMarcha.Play();
        }
    }

    public void PararMarcha()
    {
        inimigosAndando--;

        if (inimigosAndando < 0)
            inimigosAndando = 0;

        if (inimigosAndando == 0)
        {
            audioMarcha.Stop();
        }
    }
}