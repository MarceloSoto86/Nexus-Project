using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    //public float damage;
    public float lifetime = 5f; // Tiempo de vida del proyectil antes de ser destruido
    public GameObject impactEffect; // Efecto visual al impactar

    private EnemyData _enemyData;
    private float projectileSpeed;
    //private PlayerController _playerController;



    private void Start()
    {
        // Destruir el proyectil después de su tiempo de vida
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el proyectil colisiona con un enemigo
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); // Destruir el proyectil al impactar
        }
            else if (other.CompareTag("Player"))
            {
            PlayerController _player = other.GetComponent<PlayerController>(); // Obtener el componente PlayerController del objeto con el que colisionó
            if (_player != null)
                {
                    _player.TakeDamage(_enemyData.damagePoints); // Aplicar daño al jugador establecido en EnemyData (ScriptableObject) mediante la función TakeDamage del PlayerController
                    Vector3 knockbackDirection = transform.forward; // Dirección del knockback (puede ser ajustada según tus necesidades)
                    float knockbackForce = _enemyData.knockbackForce;
                    _player.ApplyKnockback(knockbackDirection, knockbackForce);
                }
                if (impactEffect != null)
                {
                    Instantiate(impactEffect, transform.position, Quaternion.identity);
                }
                Destroy(gameObject); // Destruir el proyectil al impactar
        }
    }

    public void Setup(EnemyData enemyData, float speed)
    {
        _enemyData = enemyData;
        projectileSpeed = speed;
    }
}
