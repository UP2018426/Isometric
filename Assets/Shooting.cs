using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootingPoint;
    private ThirdPersonController tpsController;

    [SerializeField] float shootForce;

    [Header("Input Actions")]
    public InputActionReference shootAction;

    void Start()
    {
        tpsController = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame())
        {
            GameObject shootingProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.identity);

            Rigidbody projectileRigidbody = shootingProjectile.GetComponent<Rigidbody>();

            if (tpsController.targetObject != null)
            {
                projectileRigidbody.AddForce((tpsController.targetObject.position - shootingPoint.position).normalized * shootForce, ForceMode.Impulse);
            }   
            else
            {
                projectileRigidbody.AddForce(shootingPoint.forward * shootForce, ForceMode.Impulse);
            }
        }
    }

    private void OnEnable()
    {
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
    }
}
