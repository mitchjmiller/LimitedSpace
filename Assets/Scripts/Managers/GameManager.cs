using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  [SerializeField] private ShipController ship;
  [SerializeField] private GameObject batteryPrefab;
  [SerializeField] private GameObject crackPrefab;
  [SerializeField] private GameObject crackMiniPrefab;
  [SerializeField] private GameObject cracksParent;
  [SerializeField] private float outerLimitDistance = 250f;
  [SerializeField] private float atmosphereDistance = 100f;
  [SerializeField] private float killZoneDistance = 50f;
  [SerializeField] private float batterySpawnInterval = 10f;

  private float batterySpawnTimer;

  private void OnDrawGizmos() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(Vector3.zero, outerLimitDistance);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(Vector3.zero, atmosphereDistance);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(Vector3.zero, killZoneDistance);
  }

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void Start() {
    batterySpawnTimer = batterySpawnInterval;
  }

  private void Update() {
    UpdateTimers();
    SpawnBattery();
  }

  private void FixedUpdate() {
    CheckBounds();
  }

  private void UpdateTimers() {
    batterySpawnTimer = Mathf.Clamp(batterySpawnTimer - Time.deltaTime, 0f, 100f);
  }

  private void CheckBounds() {
    float orbitDistance = (ship.transform.position - Vector3.zero).magnitude;
    if (orbitDistance > outerLimitDistance) {
      Vector3 hitPosition = ship.transform.position;
      Vector3 hitNormal = (Vector3.zero - hitPosition).normalized;
      ship.HandleCollision(hitNormal);
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

  public void SpawnBattery() {
    if (batterySpawnTimer > 0f) return;
    batterySpawnTimer = batterySpawnInterval;
    Vector3 spawnLocation = ship.transform.position + (ship.transform.forward * 30f) + (Random.onUnitSphere * 5f);
    Instantiate(batteryPrefab, spawnLocation, Quaternion.identity);
  }

  public float GetOuterLimitDistance() => outerLimitDistance;
}
