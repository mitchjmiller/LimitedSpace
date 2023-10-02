using UnityEngine;

public class ShipVisual : MonoBehaviour {
  [SerializeField] private ParticleSystem mainJet;
  [SerializeField] private ParticleSystem[] boostJets;
  [SerializeField] private GameObject burnUpEffects;
  [SerializeField] private ParticlesController dust;
  [SerializeField] private float dustSpeed;

  private ShipController ship;

  private void Awake() {
    ship = GetComponent<ShipController>();
  }

  private void Start() {
    ship.OnBoostStart.AddListener(OnBoostStart);
    ship.OnBoostEnd.AddListener(OnBoostEnd);
  }

  private void OnDestroy() {
    ship.OnBoostStart.RemoveListener(OnBoostStart);
    ship.OnBoostEnd.RemoveListener(OnBoostEnd);
  }

  private void OnBoostStart() {
    dust.SetStartSpeed(dustSpeed * 3);
    foreach (ParticleSystem boostJet in boostJets) {
      boostJet.Play();
    }
  }

  private void OnBoostEnd() {
    dust.SetStartSpeed(dustSpeed);
    foreach (ParticleSystem boostJet in boostJets) {
      boostJet.Stop();
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Atmosphere")) {
      burnUpEffects.SetActive(true);
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.CompareTag("Atmosphere")) {
      burnUpEffects.SetActive(false);
    }
  }
}
