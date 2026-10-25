using UnityEngine;

public class SpreaderPickup : MonoBehaviour
{
    [Header("Configuración de Enganche")]
    public KeyCode attachKey = KeyCode.E;
    private GameObject currentContainer = null;
    private FixedJoint joint = null;
    private bool isAttached = false;

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