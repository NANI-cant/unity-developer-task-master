using System;
using UnityEngine;

public interface IFinishable {
    event Action<IFinishable> Finished;

    void Finish(MonoBehaviour killer);
}
