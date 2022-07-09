using System;
using UnityEngine;

[Serializable]
public struct FinishingOptions {
    [SerializeField][Min(0f)] public float DetectionRadius;
    [SerializeField][Min(0f)] public float TimeForApproaching;
    [SerializeField][Min(0f)] public float StopDistance;
    [SerializeField][Min(0f)] public float Duration;
}
