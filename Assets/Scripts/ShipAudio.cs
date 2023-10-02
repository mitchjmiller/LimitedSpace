using System.Collections;
using UnityEngine;

public class ShipAudio : MonoBehaviour {
  [SerializeField] private AudioSource engineSource;
  [SerializeField] private AudioSource mainAudioSource;
  [SerializeField] private AudioClip chargeUpSound;
  [SerializeField] private AudioClip shootSound;
  [SerializeField] private AudioClip forceFieldCollisionSound;

  private ShipController ship;

  private void Awake() {
    ship = GetComponent<ShipController>();
  }

  private void Start() {
    ship.OnBoostStart.AddListener(OnBoostStart);
    ship.OnBoostEnd.AddListener(OnBoostEnd);
    ship.OnShoot.AddListener(OnShoot);
    ship.OnChargeUp.AddListener(OnChargeUp);
    ship.OnForceFieldCollision.AddListener(OnForceFieldCollision);
  }

  private void OnDestroy() {
    ship.OnBoostStart.RemoveListener(OnBoostStart);
    ship.OnBoostEnd.RemoveListener(OnBoostEnd);
    ship.OnShoot.RemoveListener(OnShoot);
    ship.OnChargeUp.RemoveListener(OnChargeUp);
  }

  private void OnBoostStart() {
    StartCoroutine(AdjustPitch(engineSource, 1, 3));
  }

  private void OnBoostEnd() {
    StartCoroutine(AdjustPitch(engineSource, 1, 1));
  }

  private void OnShoot() {
    mainAudioSource.PlayOneShot(shootSound);
  }

  private void OnChargeUp() {
    mainAudioSource.PlayOneShot(chargeUpSound);
  }

  private void OnForceFieldCollision() {
    mainAudioSource.PlayOneShot(forceFieldCollisionSound);
  }

  public IEnumerator AdjustPitch(AudioSource audioSource, float duration, float target) {
    float currentTime = 0;
    float start = audioSource.pitch;
    while (currentTime < duration) {
      currentTime += Time.deltaTime;
      audioSource.pitch = Mathf.Lerp(start, target, currentTime / duration);
      yield return null;
    }
    yield break;
  }
}
