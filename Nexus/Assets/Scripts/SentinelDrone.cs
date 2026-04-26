using UnityEngine;

public class SentinelDrone : MonoBehaviour
{
    public EnemyData _enemyData; // Referencia a los datos del enemigo
    public Transform _player; // Referencia al jugador
    public float _rotationSpeed = 5f; // Velocidad de rotación del dron 
    public float offsetY = 1.5f; // Desplazamiento vertical para mantener el dron a una altura constante sobre el jugador
    public bool _isActive = false; // Indica si el dron está activo o no
    public GameObject _projectilePrefab; // Prefab del proyectil que el dron disparará
    public Transform _muzzle; // Punto desde donde se dispararán los proyectiles
    
    private float _shootingTimer = 0f; // Temporizador para controlar el intervalo de disparo

    private void OnEnable()
    {
        SecuritySystemActivation.OnSecuritySystemActivated += ActivateDrone; // Suscribirse al evento de activación del sistema de seguridad
        
    }
    private void OnDisable()
    {
        SecuritySystemActivation.OnSecuritySystemActivated -= ActivateDrone; // Cancelar la suscripción al evento al desactivar el dron
      
    }
    private void Update()
    {
        if (_isActive)
        {
            RotateTowardsPlayer(); // Si el dron está activo, rotar hacia el jugador
            Shoot(); // Disparar proyectiles si el dron está activo
        }
    }
    private void ActivateDrone()
    {
        _isActive = true; // Activar el dron cuando se dispare el evento de activación del sistema de seguridad
        
    }
    private void RotateTowardsPlayer()
    {
        Vector3 targetPoint = new Vector3(_player.position.x, transform.position.y + offsetY, _player.position.z); // Mantener la altura actual del dron
        Vector3 direction = targetPoint - transform.position; // Calcular la dirección hacia el jugador

        if (direction != Vector3.zero) // Evitar rotar si el dron ya está en la posición del jugador
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction + Vector3.up * offsetY); // Calcular la rotación objetivo hacia el jugador
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime); // Rotar suavemente hacia el jugador
        }
    }

    private void Shoot()
    {
        if (_isActive && _projectilePrefab != null && _muzzle != null && _enemyData != null)
        {
            
            _shootingTimer += Time.deltaTime; // Incrementar el temporizador de disparo
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position); // Calcular la distancia al jugador
            if (distanceToPlayer <= _enemyData.detectionRange) // Si el jugador está dentro del rango de detección
            {
                if (_shootingTimer >= _enemyData.attackCooldown) // Si ha pasado el intervalo de disparo
                {
                    GameObject bullet = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
                    // Configurar el proyectil con los datos del enemigo y la velocidad de disparo
                    bullet.GetComponent<Projectile>().Setup(_enemyData, _enemyData.projectileSpeed);
                    _shootingTimer = 0f; // Reiniciar el temporizador de disparo
                }
            }
        }
    }
}
