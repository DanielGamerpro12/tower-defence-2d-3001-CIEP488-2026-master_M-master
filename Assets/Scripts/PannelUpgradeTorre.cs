using TMPro;
using UnityEngine;

public class PannelUpgradeTorre : MonoBehaviour
{
    public TextMeshProUGUI txtMensagem;
    public TextMeshProUGUI txtCustoUpgrade;
    public PannelTorres pnlTorres;
    private Torre torreSelecionada;
    private int custoUpgrade;
    public void Init(Torre torre)
    {
        torreSelecionada = torre;
        txtMensagem.text = $"Deseja evoluir a torre para o próximo nível?" +
            $"\r\nNv.{torreSelecionada.nivel} > Nv.{torreSelecionada.nivel+1}\r\n+20% Velocidade" +
            $"\r\n+10% de ataque";
        custoUpgrade = torreSelecionada.preco * (torreSelecionada.nivel + 1);
        txtCustoUpgrade.text = $"${custoUpgrade}";
    }

    public void ComprarUpgrade()
    {
        int moedas = DBMng.ObterMoedasPlayer();
        if (moedas>=custoUpgrade)
        {
            //Comprar o upgrade da torre
            Torre torreAtualizada = torreSelecionada;
            torreAtualizada.nivel += 1;
            torreAtualizada.velocidadeAtaque *= 1.2f; //Aumenta a velocidade de ataque em 20%
            torreAtualizada.poderAtaque *= 1.1f; //Aumenta o poder de ataque em 10%
            //Salvar na memoria
            DBMng.AtualizarNivelTorre(torreAtualizada, custoUpgrade);

            //Ocultar o painel e atualizar as torres no painel torres
            pnlTorres.Init();
            gameObject.SetActive(false);
        }
    }
}
