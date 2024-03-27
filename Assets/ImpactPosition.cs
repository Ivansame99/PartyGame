using UnityEngine;

public class ImpactPosition : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 direction;

    private void Start()
    {
        // Guarda la posición inicial del meteorito
        initialPosition = transform.position;

        // Define la dirección del meteorito (puede ser una dirección aleatoria o predefinida)
        direction = new Vector3(0, -25, 12).normalized; // Por ejemplo, diagonal hacia abajo y a la derecha
    }

    private void Update()
    {
        // Mueve el meteorito en la dirección definida
        transform.position += direction * Time.deltaTime * 10;

        // Lanza un raycast desde la posición inicial del meteorito en la dirección de movimiento
        RaycastHit hit;
        if (Physics.Raycast(initialPosition, direction, out hit))
        {
            // Verifica si el raycast ha golpeado el suelo
            if (hit.collider.CompareTag("Ground"))
            {
                // Acción a realizar cuando el meteorito impacta en el suelo
                Debug.Log("El meteorito ha impactado en el suelo en la posición: " + hit.point);

                // Puedes realizar cualquier acción adicional aquí, como generar un efecto visual en el punto de impacto.
            }
        }
    }
}