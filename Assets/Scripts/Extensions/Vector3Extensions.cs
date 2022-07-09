using UnityEngine;

public static class Vector3Extensions {
    public static Vector3 ToXZPlane(this Vector3 vector) {
        return new Vector3(vector.x, 0, vector.z);
    }
}
