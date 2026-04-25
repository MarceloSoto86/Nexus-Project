using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MemoryFlashes", menuName = "Scriptable Objects/MemoryFlashes")]
public class MemoryFlashes : ScriptableObject
{
    public float _numberID;
    public Sprite _imageMemory;
    public TextMeshPro _textMeshProMemory;
    public Sprite _nitidFaceImg; // Imagen de la cara nitida desbloqueada
    public TextMeshPro _nitidFaceText; // Texto de la cara nitida desbloqueada
    public AudioClip _audioClipMemory; // Audio del recuerdo
}
