using UnityEngine;

public class Ragdoll : MonoBehaviour {
    private Rigidbody[] _bones;

    private Vector3[] _savedBonesPositions;
    private Quaternion[] _savedBonesRotations;

    private void Awake() {
        _bones = GetComponentsInChildren<Rigidbody>();
        SaveBones();
    }

    public void Fall() {
        SaveBones();
        Enable();
    }

    public void StandUp() {
        LoadBones();
        Disable();
    }

    private void SaveBones() {
        _savedBonesPositions = new Vector3[_bones.Length];
        _savedBonesRotations = new Quaternion[_bones.Length];

        for (int i = 0; i < _bones.Length; i++) {
            _savedBonesPositions[i] = _bones[i].transform.position;
            _savedBonesRotations[i] = _bones[i].transform.rotation;
        }
    }

    private void LoadBones() {
        if (_savedBonesPositions.Length != _bones.Length || _savedBonesRotations.Length != _bones.Length) {
            Debug.LogError($"{gameObject.name}: can't load bones, arrays don't match");
            return;
        }

        for (int i = 0; i < _bones.Length; i++) {
            _savedBonesPositions[i] = _bones[i].transform.position;
            _savedBonesRotations[i] = _bones[i].transform.rotation;
        }
    }


    private void Enable() {
        foreach (var bone in _bones) {
            bone.isKinematic = false;
        }
    }

    private void Disable() {
        foreach (var bone in _bones) {
            bone.isKinematic = true;
        }
    }
}