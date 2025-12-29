using UnityEngine;

public class ColourChanging : MonoBehaviour
{
    public float windupSpeed;
    float windingTimer;

    public bool preparingToAttack;

    [SerializeField] Color telegraphColour;
    MeshRenderer meshRenderer;
    Color baseColour;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        baseColour = meshRenderer.material.color;
    }

    void Update()
    {
        if (preparingToAttack)
        {
            windingTimer += windupSpeed * Time.deltaTime;
        }
        else
        {
            windingTimer -= windupSpeed * Time.deltaTime;
        }

        windingTimer = Mathf.Clamp01(windingTimer);

        meshRenderer.material.color = Color.Lerp(baseColour, telegraphColour, windingTimer);

        /*Color matColour = meshRenderer.materials[1].color;
        matColour.a = windingTimer;
        meshRenderer.materials[1].color = matColour;*/
    }
}
