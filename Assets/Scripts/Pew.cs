using UnityEngine;

public class Pew : MonoBehaviour {
  [SerializeField] private float speed = 5f;
  [SerializeField] private float lifetime = 5f;

  private void Update() {
    HandleMovement();
    HandleDistance();
    HandleLifetime();
  }

  private void HandleMovement() {
    transform.Translate(Vector3.forward * speed);
  }

  private void HandleDistance() {
    if ((transform.position - Vector3.zero).magnitude > GameManager.Instance.GetOuterLimitDistance()) {
      GameManager.Instance.SpawnMiniCrack(transform.position);
      Destroy(gameObject);
    }
  }

  private void HandleLifetime() {
    lifetime -= Time.deltaTime;
    if (lifetime <= 0f) Destroy(gameObject);
  }
}
