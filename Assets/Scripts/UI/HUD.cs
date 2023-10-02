using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
  [SerializeField] private ShipController ship;
  [SerializeField] private Image forceFieldHealthBar;
  [SerializeField] private Image healthBar;
  [SerializeField] private Gradient healthBarGradient;
  [SerializeField] private Image powerBar;
  [SerializeField] private Gradient powerBarGradient;

  private void Start() {
    GameManager.Instance.OnForceFieldHealthChange.AddListener(OnForceFieldHealthChange);
    ship.OnHealthChange.AddListener(OnHealthChange);
    ship.OnPowerChange.AddListener(OnPowerChange);
  }

  private void OnDestroy() {
    GameManager.Instance.OnForceFieldHealthChange.RemoveListener(OnForceFieldHealthChange);
    ship.OnHealthChange.RemoveListener(OnHealthChange);
    ship.OnPowerChange.RemoveListener(OnPowerChange);
  }

  private void OnForceFieldHealthChange() {
    float currentForceFieldHealth = GameManager.Instance.GetCurrentForceFieldHealth();
    float maxForceFieldHealth = GameManager.Instance.GetMaxForceFieldHealth();
    float forceFieldHealthBarPC = currentForceFieldHealth == 0 ? 0 : currentForceFieldHealth / maxForceFieldHealth;
    forceFieldHealthBar.fillAmount = forceFieldHealthBarPC;
  }

  private void OnHealthChange() {
    float currentHealth = ship.GetCurrentHealth();
    float healthBarPC = currentHealth == 0 ? 0 : currentHealth / ship.GetMaxHealth();
    healthBar.color = healthBarGradient.Evaluate(healthBarPC);
    healthBar.fillAmount = healthBarPC;
  }

  private void OnPowerChange() {
    float currentPower = ship.GetCurrentPower();
    float powerBarPC = currentPower == 0 ? 0 : currentPower / ship.GetMaxPower();
    powerBar.color = powerBarGradient.Evaluate(powerBarPC);
    powerBar.fillAmount = powerBarPC;
  }


}
