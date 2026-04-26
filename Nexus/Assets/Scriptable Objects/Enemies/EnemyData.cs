using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName; // Nombre del enemigo
    public Sprite enemySprite; // Sprite del enemigo
    public int enemyID; // ID único para identificar al enemigo
    public float healthPoints; // Puntos de salud del enemigo
    public float damagePoints; // Daño que el enemigo puede causar
    public float movementSpeed; // Velocidad de movimiento del enemigo
    public float attackRange; // Rango de ataque del enemigo
    public float detectionRange; // Rango de detección del enemigo
    public float attackCooldown; // Tiempo de enfriamiento entre ataques del enemigo
    public float detectionCooldown;
    public float projectileSpeed; // Velocidad de disparo del enemigo (si es un enemigo que dispara)
    public float visualColorChangeDuration; // Duración del cambio de color visual al recibir daño
    public float pointValue; // Valor de puntos que el jugador recibe al derrotar al enemigo
    public float knockbackForce; // Fuerza de retroceso aplicada al jugador al ser golpeado por el enemigo  
    public Color visualColor; // Color visual del enemigo (puede ser utilizado para cambiar el color al recibir daño
    
}
