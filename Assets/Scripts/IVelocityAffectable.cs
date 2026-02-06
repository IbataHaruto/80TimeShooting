using UnityEngine;

// 速度影響を受けることができるインターフェース
public interface IVelocityAffectable
{
    IVelocityAffecter Affecter { set; }
}
