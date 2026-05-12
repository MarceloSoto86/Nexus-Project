using UnityEngine;

public class AnchorToParent : MonoBehaviour
{
    // Usamos LateUpdate porque se ejecuta después de cualquier 
    // cálculo de física o animación que pueda estar moviendo al hijo.
    void LateUpdate()
    {
        // Forzamos a que la posición local sea siempre el centro del padre.
        transform.localPosition = Vector3.zero;

        // Si notas que el modelo también rota solo, podés forzar la rotación:
        // transform.localRotation = Quaternion.identity;
    }
}
