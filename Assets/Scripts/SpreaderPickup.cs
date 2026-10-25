using UnityEngine;

public class SpreaderPickup : MonoBehaviour
{
    [Header("Configuración de Enganche")]
    public KeyCode attachKey = KeyCode.E;
    private GameObject currentContainer = null;
    private FixedJoint joint = null;
    private bool isAttached = false;

    [Header("Simulación de Viento")]
    [Tooltip("Activa o desactiva las ráfagas de viento físicas sobre el Spreader/Contenedor.")]
    public bool enableWind = true;
    [Tooltip("Intensidad de la fuerza del viento.")]
    public float windIntensity = 25f;
    [Tooltip("Frecuencia con la que varían las ráfagas de viento.")]
    public float windFrequency = 1.2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attachKey))
        {
            if (isAttached)
            {
                DetachContainer();
            }
            else if (currentContainer != null)
            {
                AttachContainer();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicar la fuerza del viento en el ciclo de físicas (FixedUpdate)
        ApplyWindForce();
    }

    private void ApplyWindForce()
    {
        if (!enableWind || rb == null) return;

        // Genera ráfagas de viento oscilantes usando una combinación de ondas seno
        float windVariation = Mathf.Sin(Time.time * windFrequency) * Mathf.Cos(Time.time * 0.7f);
        
        // La fuerza actúa principalmente en el eje lateral (Z o X según la orientación de tu escena)
        Vector3 windDirection = Vector3.right * windVariation; 

        // Si el contenedor está enganchado, el peso aumenta, por lo que aplicamos un pequeño multiplicador
        float weightMultiplier = isAttached ? 1.3f : 1.0f;

        rb.AddForce(windDirection * windIntensity * weightMultiplier, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Container") && !isAttached)
        {
            currentContainer = other.gameObject;
            Debug.Log("Contenedor detectado. Presiona 'E' para enganchar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Container") && !isAttached)
        {
            if (currentContainer == other.gameObject)
            {
                currentContainer = null;
            }
        }
    }

    void AttachContainer()
    {
        if (currentContainer == null) return;

        // Crear un FixedJoint dinámicamente para sujetar el contenedor al Spreader
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = currentContainer.GetComponent<Rigidbody>();
        
        isAttached = true;
        Debug.Log("Contenedor ENGANCHADO.");
    }

    void DetachContainer()
    {
        if (joint != null)
        {
            Destroy(joint);
        }

        isAttached = false;
        Debug.Log("Contenedor DESENGANCHADO.");
    }

    public bool IsAttached => isAttached;
}