using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
  public enum DifficultyLevel // Enum para representar los niveles de dificultad
    {
    Easy,
    Medium,
    Hard
    }
    public DifficultyLevel currentDifficultyLevel;

    public static DifficultyManager Instance; // Instancia singleton para acceder al DifficultyManager desde otros scripts

    private void Awake() // Método Awake para inicializar la instancia singleton
    {
        Instance = this;
    }

    public float GetStepHeight() // Método para obtener la altura del paso según el nivel de dificultad actual
    {
        return currentDifficultyLevel
            switch
        {
            DifficultyLevel.Easy => 15.0f,
            DifficultyLevel.Medium => 30f,
            _ => 10000f // Valor predeterminado en caso de que no se haya establecido un nivel de dificultad válido
        };
    }
}
