using Cinemachine;
using System.Collections;
using UnityEngine;

public class ShipCamera : MonoBehaviour {
  private CinemachineVirtualCamera virtualCam;
  private CinemachineTransposer transposer;

  [SerializeField] private float offSetMultiplier = 1.5f;

  private void Awake() {
    virtualCam = GetComponent<CinemachineVirtualCamera>();
    transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
  }
  private void Start() {
    InputManager.Instance.OnBoostStart.AddListener(OnBoostStart);
    InputManager.Instance.OnBoostEnd.AddListener(OnBoostEnd);
  }

  private void OnDestroy() {
    InputManager.Instance.OnBoostStart.RemoveListener(OnBoostStart);
    InputManager.Instance.OnBoostEnd.RemoveListener(OnBoostEnd);
  }

  private void OnBoostStart() {
    StartCoroutine(SetFollowOffset(transposer.m_FollowOffset, transposer.m_FollowOffset * offSetMultiplier));
  }

  private void OnBoostEnd() {
    StartCoroutine(SetFollowOffset(transposer.m_FollowOffset, transposer.m_FollowOffset / offSetMultiplier));
  }

  private IEnumerator SetFollowOffset(Vector3 current, Vector3 target) {
    float elapsedTime = 0f;
    float transitionTime = 3f;
    while (elapsedTime < transitionTime) {
      transposer.m_FollowOffset = Vector3.Lerp(current, target, (elapsedTime / transitionTime));
      elapsedTime += Time.deltaTime;
      yield return null;
    }
  }
}
