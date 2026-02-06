using UnityEngine;

// 速度影響を与えることができるインターフェース
public interface IVelocityAffecter
{
    Vector3 AffectedVelocity { get; }
}
