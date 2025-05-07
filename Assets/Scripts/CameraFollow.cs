using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo; // Personagem a ser seguido
    public Vector3 offset; // Distância entre a câmera e o personagem
    public float suavizacao = 0.125f; // Suavização do movimento

    void LateUpdate()
    {
        if (alvo != null)
        {
            Vector3 posicaoDesejada = alvo.position + offset;
            Vector3 posicaoSuavizada = Vector3.Lerp(transform.position, posicaoDesejada, suavizacao);
            transform.position = new Vector3(posicaoSuavizada.x, posicaoSuavizada.y, transform.position.z);
        }
    }
}
