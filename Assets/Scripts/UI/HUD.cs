using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
  [SerializeField] private ShipController ship;
  [SerializeField] private Image powerBar;
  [SerializeField] private Gradient powerBarGradient;

  private void Start() {
    ship.OnPowerChange.AddListener(OnPowerChange);
  }

  private void OnDestroy() {
    ship.OnPowerChange.RemoveListener(OnPowerChange);
  }

  private void OnPowerChange() {
    float powerBarPC = ship.GetCurrentPower() / ship.GetMaxPower();
    powerBar.color = powerBarGradient.Evaluate(powerBarPC);
    powerBar.fillAmount = powerBarPC;
  }
}
