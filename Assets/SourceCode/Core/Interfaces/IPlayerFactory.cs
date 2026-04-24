using UnityEngine;

namespace Core.Interfaces
{
    public interface IPlayerFactory
    {
        GameObject Create(Vector3 position, Quaternion rotation);
    }
}