using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private GunManager gunManager;

    private void Update()
    {
        if (InputController.Instance.GetButtonDown(InputController.InputAction.Interact))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            AmmoBox ammoBox = hit.collider.GetComponent<AmmoBox>();
            if (ammoBox != null)
            {
                BaseGun currentGun = gunManager.GetCurrentGun();

                ammoBox.RefillAmmo(currentGun);
            }
        }
    }
}
