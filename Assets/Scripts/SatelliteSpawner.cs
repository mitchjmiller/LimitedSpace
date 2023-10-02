using UnityEngine;

public class SatelliteSpawner : MonoBehaviour {
  [SerializeField] private GameObject prefab;
  [SerializeField] private float distance;

  private void Start() {
    float[] xs = { -1, 0, 1 };
    float[] ys = { -1, 0, 1 };
    float[] zs = { -1, 0, 1 };

    foreach (float x in xs) {
      foreach (float y in ys) {
        foreach (float z in zs) {
          Vector3 vector = new Vector3(x, y, z);
          if (vector == Vector3.zero) break;
          Instantiate(prefab, vector.normalized * distance, Quaternion.identity, transform);
        }
      }
    }
  }
}
