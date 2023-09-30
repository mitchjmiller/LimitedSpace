using UnityEngine;

public class ShipVisual : MonoBehaviour {
  [SerializeField] private ParticleSystem mainJet;
  [SerializeField] private ParticleSystem[] boostJets;
  [SerializeField] private ParticlesController stars;

  private void Start() {
    InputManager.Instance.OnBoostStart.AddListener(OnBoostStart);
    InputManager.Instance.OnBoostEnd.AddListener(OnBoostEnd);
  }

  private void OnDestroy() {
    InputManager.Instance.OnBoostStart.RemoveListener(OnBoostStart);
    InputManager.Instance.OnBoostEnd.RemoveListener(OnBoostEnd);
  }

  private void OnBoostStart() {
    stars.SetStartSpeed(15);
    foreach (ParticleSystem boostJet in boostJets) {
      boostJet.Play();
    }
  }

  private void OnBoostEnd() {
    stars.SetStartSpeed(5);
    foreach (ParticleSystem boostJet in boostJets) {
      boostJet.Stop();
    }
  }
}
