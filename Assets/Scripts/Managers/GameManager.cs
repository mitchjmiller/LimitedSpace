using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  [SerializeField] private ShipController ship;
  [SerializeField] private GameObject crackPrefab;
  [SerializeField] private GameObject crackMiniPrefab;
  [SerializeField] private GameObject cracksParent;
  [SerializeField] private float outerLimitDistance = 250f;
  [SerializeField] private float maxForceFieldHealth = 100f;

  public UnityEvent OnForceFieldHealthChange = new UnityEvent();

  private float _currentForceFieldHealth;
  private float CurrentForceFieldHealth {
    get { return _currentForceFieldHealth; }
    set { _currentForceFieldHealth = Mathf.Clamp(value, 0f, maxForceFieldHealth); OnForceFieldHealthChange.Invoke(); }
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(Vector3.zero, outerLimitDistance);
  }

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void Start() {
    _currentForceFieldHealth = maxForceFieldHealth;
  }

  private void FixedUpdate() {
    CheckBounds();
  }

  private void CheckBounds() {
    if (CurrentForceFieldHealth <= 0f) return;

    float orbitDistance = (ship.transform.position - Vector3.zero).magnitude;
    if (orbitDistance > outerLimitDistance) {
      Vector3 hitPosition = ship.transform.position;
      Vector3 hitNormal = (Vector3.zero - hitPosition).normalized;
      ship.HandleCollision(hitNormal);
      AdjustForceFieldHealth(-5f);
      SpawnCrack(hitPosition);
    }
  }

  public void SpawnCrack(Vector3 hitPosition) {
    Vector3 hitNormal = (Vector3.zero - hitPosition).normalized;
    Instantiate(crackPrefab, hitPosition, Quaternion.LookRotation(hitNormal, Vector3.up), cracksParent.transform);
  }

  public void SpawnMiniCrack(Vector3 hitPosition) {
    Vector3 hitNormal = (Vector3.zero - hitPosition).normalized;
    Instantiate(crackMiniPrefab, hitPosition, Quaternion.LookRotation(hitNormal, Random.onUnitSphere), cracksParent.transform);
  }

  public float GetOuterLimitDistance() => outerLimitDistance;

  public void AdjustForceFieldHealth(float health) {
    CurrentForceFieldHealth += health;
    if (CurrentForceFieldHealth <= 0f) {
      Destroy(cracksParent.gameObject);
    }
  }

  public float GetCurrentForceFieldHealth() => CurrentForceFieldHealth;
  public float GetMaxForceFieldHealth() => maxForceFieldHealth;

  public void GameOver() {
    StartCoroutine(ReloadScene());
  }

  private IEnumerator ReloadScene() {
    yield return new WaitForSeconds(8);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
