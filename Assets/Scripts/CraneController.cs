using UnityEngine;

public class CraneController : MonoBehaviour
{
    [Header("Referencias de la Grúa")]
    public Transform trolley;
    public Transform spreader;

    [Header("Configuración del Carro (Trolley)")]
    public float trolleySpeed = 8f;
    public float trolleyMinZ = -12f;
    public float trolleyMaxZ = 12f;

    [Header("Configuración del Cable/Izaje (Hoist)")]
    public float hoistSpeed = 4f;
    public float minCableLength = 2f;  // Altura máxima del Spreader
    public float maxCableLength = 17f; // Altura mínima (cerca del suelo)

    private ConfigurableJoint joint;
    private float currentCableLength = 8f;

    void Start()
    {
        if (spreader != null)
        {
            joint = spreader.GetComponent<ConfigurableJoint>();
            if (joint != null)
            {
                // Establecemos la longitud inicial del cable
                currentCableLength = Vector3.Distance(trolley.position, spreader.position);
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = new Vector3(0, -currentCableLength, 0);
            }
        }
    }

    void Update()
    {
        HandleTrolleyMovement();
        HandleHoistMovement();
    }

    void HandleTrolleyMovement()
    {
        if (trolley == null) return;

        float inputZ = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputZ = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputZ = -1f;
        
        Vector3 newPosition = trolley.localPosition + Vector3.forward * inputZ * trolleySpeed * Time.deltaTime;
        newPosition.z = Mathf.Clamp(newPosition.z, trolleyMinZ, trolleyMaxZ);
        trolley.localPosition = newPosition;
    }

    void HandleHoistMovement()
    {
        if (joint == null) return;

        float inputY = 0f;
        if (Input.GetKey(KeyCode.Space)) inputY = -1f;          // Subir (acorta el cable)
        else if (Input.GetKey(KeyCode.LeftControl)) inputY = 1f; // Bajar (alarga el cable)

        if (inputY != 0f)
        {
            // Ajustamos la longitud del cable
            currentCableLength += inputY * hoistSpeed * Time.deltaTime;
            currentCableLength = Mathf.Clamp(currentCableLength, minCableLength, maxCableLength);

            // Modificamos el anclaje del Joint directamente
            joint.connectedAnchor = new Vector3(0, -currentCableLength, 0);
        }
    }
}