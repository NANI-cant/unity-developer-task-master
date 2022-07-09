using System.Collections;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {
    [Header("Dev")]
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _spineBone;
    [SerializeField] private Animator _animator;

    private const string BACKBOOL = "Back";
    private const string FORWARDBOOL = "Forward";
    private const string FINISHTRIGGER = "Finish";

    private const float TIMEFORROTATION = 0.1f;
    private const int ANGLETOROTATEBODY = 90;
    private const int ROTATIONACCURACITY = 5;

    private bool _isEnable = true;
    private Coroutine _smoothRotation;

    public void PlayFinishing(MonoBehaviour target) {
        _body.forward = (target.transform.position - transform.position).normalized;
        _animator.SetTrigger(FINISHTRIGGER);
    }

    public void VisualiseMoving(Vector3 moveDirection, Vector3 lookDirection) {
        if (!_isEnable) return;

        if (moveDirection != Vector3.zero) {
            Vector3 targetBodyDirection = moveDirection;
            float lookMoveAngle = Vector3.Angle(moveDirection, lookDirection);
            if (lookMoveAngle > 90) {
                targetBodyDirection = -moveDirection;
                GoBack();
            }
            else {
                targetBodyDirection = moveDirection;
                GoForward();
            }

            SmoothRotateBody(targetBodyDirection);
        }
        else {
            if (lookDirection == Vector3.zero) return;
            float bodyLookAngle = Vector3.Angle(_body.forward, lookDirection);
            if (bodyLookAngle > ANGLETOROTATEBODY) {
                SmoothRotateBody(lookDirection);
            }
            StayIdle();
        }

        Quaternion fromToRotation = Quaternion.FromToRotation(_body.forward, lookDirection);
        _spineBone.Rotate(fromToRotation.eulerAngles, Space.World);
    }

    public void Enable() => _isEnable = true;
    public void Disable() {
        StopAllCoroutines();
        _isEnable = false;
    }

    private void SmoothRotateBody(Vector3 direction) {
        StopAllCoroutines();
        StartCoroutine(RotateBody(direction));
    }

    private IEnumerator RotateBody(Vector3 direction) {
        float deltaAngle = Vector3.SignedAngle(_body.forward, direction, Vector3.up);
        while (Mathf.Abs(deltaAngle) >= ROTATIONACCURACITY) {
            _body.Rotate(Vector3.up, deltaAngle / TIMEFORROTATION * Time.deltaTime, Space.World);
            yield return new WaitForEndOfFrame();
            deltaAngle = Vector3.SignedAngle(_body.forward, direction, Vector3.up);
        }
        _body.forward = direction;
    }

    private void StayIdle() {
        _animator.SetBool(FORWARDBOOL, false);
        _animator.SetBool(BACKBOOL, false);
    }

    private void GoForward() {
        _animator.SetBool(FORWARDBOOL, true);
        _animator.SetBool(BACKBOOL, false);
    }

    private void GoBack() {
        _animator.SetBool(FORWARDBOOL, false);
        _animator.SetBool(BACKBOOL, true);
    }
}
