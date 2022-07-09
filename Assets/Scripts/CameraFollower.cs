using UnityEngine;

public class CameraFollower : MonoBehaviour {
    [Header("Dev")]
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;

    private void Update() {
        transform.position = _followTarget.position + _positionOffset;
        transform.rotation = Quaternion.Euler(_rotationOffset);
    }

    [ContextMenu("Get Rotation From Transform")]
    private void GetRotationFromTransform() {
        _rotationOffset = transform.rotation.eulerAngles;
    }

    [ContextMenu("Get Position From Scene")]
    private void GetPositionFromScene() {
        if (_followTarget == null) {
            Debug.LogError($"{gameObject.name}: {nameof(_followTarget)} is null, can't initialize position offset");
            return;
        }
        _positionOffset = transform.position - _followTarget.transform.position;
    }

    [ContextMenu("Initialize Offsets From Scene")]
    private void InitializeOffsetsFromScene() {
        GetRotationFromTransform();
        GetPositionFromScene();
    }
}
