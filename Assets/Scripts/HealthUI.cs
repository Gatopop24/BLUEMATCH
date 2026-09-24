using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Health playerHealth;

    private void Update()
    {
        float healthPercent = (float)playerHealth.currentHealth / playerHealth.MaxHealth;
        fillImage.fillAmount = healthPercent;
    }
}
