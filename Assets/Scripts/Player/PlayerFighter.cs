using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerFighter : MonoBehaviour {
    [SerializeField] private LayerMask _enemyMask;

    public event Action<IFinishable> FinishTargetFound;
    public event Action<IFinishable> FinishTargetLost;

    private FinishingOptions _finishing;
    private IFinishable _targetFinishable;
    private Coroutine _finishCoroutine;

    public void Initialize(FinishingOptions finishing) {
        _finishing = finishing;
        var detectiveSphere = GetComponent<SphereCollider>();
        detectiveSphere.radius = finishing.DetectionRadius;
        detectiveSphere.center = Vector3.zero;

    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<IFinishable>(out IFinishable finishable)) {
            if (_targetFinishable == null) {
                _targetFinishable = finishable;
                _targetFinishable.Finished += OnTargetFinished;
                FinishTargetFound?.Invoke(_targetFinishable);
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.TryGetComponent<IFinishable>(out IFinishable finishable)) {
            if (_targetFinishable == finishable) {
                _targetFinishable.Finished -= OnTargetFinished;
                _targetFinishable = null;
                FinishTargetLost?.Invoke(_targetFinishable);
            }
        }
    }

    public bool HandleFinishing(out MonoBehaviour finishableBehaviour) {
        finishableBehaviour = (MonoBehaviour)_targetFinishable;
        if (_targetFinishable == null) return false;

        if (_finishCoroutine != null) {
            StopCoroutine(_finishCoroutine);
        }
        _finishCoroutine = StartCoroutine(ExecuteApproaching(finishableBehaviour));

        return true;
    }

    private IEnumerator ExecuteApproaching(MonoBehaviour finishableBehaviour) {
        float distance = Mathf.Sqrt((finishableBehaviour.transform.position - transform.position).sqrMagnitude);
        float speed = distance / _finishing.TimeForApproaching;

        while (distance > _finishing.StopDistance) {
            Vector3 direction = (finishableBehaviour.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            yield return new WaitForEndOfFrame();
            distance = Mathf.Sqrt((finishableBehaviour.transform.position - transform.position).sqrMagnitude);
        }

        _targetFinishable.Finish(this);
    }

    private void OnTargetFinished(IFinishable finishable) {
        finishable.Finished -= OnTargetFinished;
        _targetFinishable = null;
        FinishTargetLost?.Invoke(finishable);
    }
}
