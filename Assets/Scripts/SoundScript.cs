using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour
{
    [SerializeField] private AudioSource fundoMusical;

    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;


    public void VolumeMusical(float value)
    {
        fundoMusical.volume = value;
    }

}
