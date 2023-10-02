using UnityEngine;

public class Pew : MonoBehaviour {
  [SerializeField] private float speed = 5f;
  [SerializeField] private float lifetime = 5f;

  private void Update() {
    HandleLifetime();
  }

  private void FixedUpdate() {
    HandleMovement();
    HandleDistance();
  }

  private void HandleMovement() {
    transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
  }

  private void HandleDistance() {
    if (GameManager.Instance.GetCurrentForceFieldHealth() > 0f && (transform.position - Vector3.zero).magnitude > GameManager.Instance.GetOuterLimitDistance()) {
      GameManager.Instance.AdjustForceFieldHealth(-0.2f);
      GameManager.Instance.SpawnMiniCrack(transform.position);
      Destroy(gameObject);
    }
  }

  private void HandleLifetime() {
    lifetime -= Time.deltaTime;
    if (lifetime <= 0f) Destroy(gameObject);
  }

  private void OnCollisionEnter(Collision collision) {
    Destroy(gameObject);
  }
}
