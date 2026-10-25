using UnityEngine;
using TMPro;

public class UIHUDManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text textStatus;
    public TMP_Text textHeight;
    public TMP_Text textWeight;

    [Header("Referencias Escena")]
    public Transform spreader;
    public Transform ground;

    private SpreaderPickup pickupScript;

    void Start()
    {
        if (spreader != null)
        {
            pickupScript = spreader.GetComponent<SpreaderPickup>();
        }
    }

    void Update()
    {
        UpdateTelemetry();
    }

    void UpdateTelemetry()
    {
        if (spreader == null) return;

        // Calcular altura respecto al suelo
        float groundY = (ground != null) ? ground.position.y : 0f;
        float currentHeight = spreader.position.y - groundY;
        textHeight.text = $"Altura: {currentHeight:F1} m";

        // Actualizar estado de enganche
        if (pickupScript != null)
        {
            if (pickupScript.IsAttached) // Crearemos esta propiedad en SpreaderPickup
            {
                textStatus.text = "Estado: CARGA ENGANCHADA";
                textStatus.color = Color.green;
                textWeight.text = "Peso Carga: 24,000 kg (20ft)";
            }
            else
            {
                textStatus.text = "Estado: SIN CARGA";
                textStatus.color = Color.yellow;
                textWeight.text = "Peso Carga: 0 kg";
            }
        }
    }
}