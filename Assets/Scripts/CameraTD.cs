using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTD : MonoBehaviour
{
    [Header("Velocidade")]
    public float velocidadeMouse = 0.02f;
    public float velocidadeToque = 0.02f;

    [Header("Limites")]
    public float limiteCima = 10f;
    public float limiteBaixo = -10f;

    private float posicaoInicialY;

    private void Start()
    {
        posicaoInicialY = transform.position.y;
    }

    private void Update()
    {
        MoverCamera();
        LimitarCamera();
    }

    void MoverCamera()
    {
        // =========================
        // MOUSE
        // =========================

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                float movimentoY = Mouse.current.delta.ReadValue().y;

                transform.position -= new Vector3(
                    0,
                    movimentoY * velocidadeMouse,
                    0
                );
            }
        }

        // =========================
        // CELULAR
        // =========================

        if (Touchscreen.current != null)
        {
            var toque = Touchscreen.current.primaryTouch;

            if (toque.press.isPressed)
            {
                Vector2 movimento = toque.delta.ReadValue();

                transform.position -= new Vector3(
                    0,
                    movimento.y * velocidadeToque,
                    0
                );
            }
        }
    }

    void LimitarCamera()
    {
        Vector3 posicao = transform.position;

        posicao.y = Mathf.Clamp(
            posicao.y,
            posicaoInicialY + limiteBaixo,
            posicaoInicialY + limiteCima
        );

        transform.position = posicao;
    }
}