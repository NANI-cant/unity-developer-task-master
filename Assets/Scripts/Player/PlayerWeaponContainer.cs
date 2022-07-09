using UnityEngine;

public class PlayerWeaponContainer : MonoBehaviour {
    [Header("Dev")]
    [SerializeField] private Transform _attachRight;
    [SerializeField] private Transform _riffle;
    [SerializeField] private Transform _sword;
    [SerializeField] private Transform _scabbard;

    public void SetSword() {
        _riffle.gameObject.SetActive(false);
        _sword.parent = _attachRight;
        _sword.localPosition = Vector3.zero;
        _sword.localRotation = Quaternion.identity;
    }

    public void SetRiffle() {
        _sword.parent = _scabbard;
        _sword.localPosition = Vector3.zero;
        _sword.localRotation = Quaternion.identity;
        _riffle.gameObject.SetActive(true);
    }
}
