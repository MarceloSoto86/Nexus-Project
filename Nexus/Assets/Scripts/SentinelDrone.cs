using UnityEngine;

public class SentinelDrone : MonoBehaviour
{
    public EnemyData _enemyData; // Referencia a los datos del enemigo
    public Transform _player; // Referencia al jugador
    public float _rotationSpeed = 5f; // Velocidad de rotación del dron 
    public float offsetY = 1.5f; // Desplazamiento vertical para mantener el dron a una altura constante sobre el jugador
    public float distanceToPlayer = 12f; // Distancia a la que el dron detectará al jugador y comenzará a disparar
    public bool _isActive = false; // Indica si el dron está activo o no
    public GameObject _projectilePrefab; // Prefab del proyectil que el dron disparará
    public Transform _muzzle; // Punto desde donde se dispararán los proyectiles
    public Transform[] waypointList; // Lista de puntos de patrulla para el dron (si se desea implementar patrullaje)
    [Range(0f, 1f)] public float predictionFactor = 0.3f; // Factor de predicción para ajustar la posición futura del jugador

    private float _shootingTimer = 0f; // Temporizador para controlar el intervalo de disparo
    private int _currentWaypointIndex = 0; // Índice del punto de patrulla actual (si se desea implementar patrullaje)

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
            if(Vector3.Distance(transform.position, _player.position) <= distanceToPlayer)
            {
                RotateTowardsPlayer(); // Si el dron está activo, rotar hacia el jugador
                Shoot(); // Disparar proyectiles si el dron está activo
            }
            else
            {
                if (Vector3.Distance(transform.position, _player.position) > distanceToPlayer)
                {
                    if (waypointList == null || waypointList.Length == 0)
                    {
                        //Debug.logWarning($"[SentinelDrone] {gameObject.name} no tiene Waypoints asignados en el Inspector.");
                        return; // Corta el frame aquí para evitar que se ejecute la línea 41 dañada
                    }
                    Vector3 targetPoint = waypointList[_currentWaypointIndex].position; // Obtener el punto de patrulla actual
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint, _rotationSpeed * Time.deltaTime); // Mover el dron hacia el punto de patrulla
                    if (AudioManager.Instance != null && AudioManager.Instance.flyingDroneSFX != null)
                    {
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.flyingDroneSFX, 0.5f); // Reproduce el efecto de sonido del dron volando
                    }
                    else
                    {
                        //Debug.logWarning("AudioManager o flyingDroneSFX no está asignado.");
                    }
                    if (Vector3.Distance(transform.position, targetPoint) < 0.2f) // Si el dron ha llegado al punto de patrulla
                    {
                        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypointList.Length; // Avanzar al siguiente punto de patrulla
                    }
                }
            }
        }
    }
    private void ActivateDrone()
    {
        _isActive = true; // Activar el dron cuando se dispare el evento de activación del sistema de seguridad
        
    }
    private void RotateTowardsPlayer()
    {
        if (PlayerController.player == null || _enemyData == null) return;

        Vector3 playerPos = _player.position; // Obtener la posición del jugador
        Vector3 playerVelocity = PlayerController.player.rb.linearVelocity; // Obtener la velocidad del jugador

        float distance = Vector3.Distance(transform.position, playerPos); // Calcular la distancia al jugador real para ajustar la rotación del dron

        float bulletSpeed = _enemyData.projectileSpeed > 0 ? _enemyData.projectileSpeed : 10f; // Velocidad del proyectil, con un valor predeterminado si no se ha configurado en EnemyData
        float estimatedTimeToHit = distance / bulletSpeed; // Estimar el tiempo que tardará el proyectil en alcanzar al jugador
        estimatedTimeToHit = Mathf.Clamp(estimatedTimeToHit, 0f, 0.4f); // Limitar el tiempo estimado para evitar predicciones extremas

        //Calcular la posición futura del jugador basándose en su velocidad actual y el tiempo estimado para que el proyectil lo alcance
        Vector3 futurePlayerPos = playerPos + (Vector3.up * offsetY) + (playerVelocity * estimatedTimeToHit * predictionFactor);

       // Vector3 targetPoint = new Vector3(_player.position.x, transform.position.y + offsetY, _player.position.z); // Mantener la altura actual del dron
        Vector3 direction = futurePlayerPos - transform.position; // Calcular la dirección hacia el jugador

        if (direction != Vector3.zero) // Evitar rotar si el dron ya está en la posición del jugador
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Calcular la rotación objetivo hacia el jugador
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
                    if (AudioManager.Instance != null && AudioManager.Instance.centinelShotsSFX != null)
                    {
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.centinelShotsSFX, 0.1f); // Reproduce el efecto de sonido de disparo
                    }
                    // Configurar el proyectil con los datos del enemigo y la velocidad de disparo
                    bullet.GetComponent<Projectile>().Setup(_enemyData, _enemyData.projectileSpeed);
                    _shootingTimer = 0f; // Reiniciar el temporizador de disparo
                }
            }
        }
    }
}
