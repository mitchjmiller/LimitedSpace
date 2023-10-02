using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
  public static SpawnManager Instance { get; private set; }

  [SerializeField] private ShipController ship;
  [SerializeField] private float spawnDistance = 30f;
  [SerializeField] private float spawnDistanceVariability = 5f;
  [SerializeField] private float spawnInterval = 5f;
  [SerializeField] private Spawnable[] spawnables;

  private GameObject parent;
  private float timer = 0f;

  [System.Serializable]
  public class Spawnable {
    [SerializeField] public GameObject prefab;
    [SerializeField] public int weight;
  }

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    parent = new GameObject("Spawnables");
  }

  private void Start() {
    timer = spawnInterval;
  }

  private void Update() {
    HandleSpawn();
  }

  private void HandleSpawn() {
    timer -= Time.deltaTime;
    if (timer <= 0) {
      SpawnObject();
      timer = spawnInterval;
    }
  }

  private void SpawnObject() {
    int totalWeight = spawnables.Sum((Spawnable spawnable) => spawnable.weight);
    int random = Random.Range(0, totalWeight);
    int cumulative = 0;
    Vector3 spawnLocation = ship.transform.position + (ship.transform.forward * spawnDistance) + (Random.onUnitSphere * spawnDistanceVariability);




    // Weighted random spawn chance
    foreach (Spawnable spawnable in spawnables) {
      cumulative += spawnable.weight;
      if (random < cumulative) {
        // Debug.Log($"{random}/{totalWeight} - {spawnable.prefab.name}");
        Instantiate(spawnable.prefab, spawnLocation, Quaternion.identity, parent.transform);
        break;
      }
    }
  }
}
