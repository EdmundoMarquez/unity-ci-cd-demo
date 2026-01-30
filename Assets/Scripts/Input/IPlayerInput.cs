using UnityEngine;

namespace CIDemo.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool JumpPressed { get; }
        bool CrouchPressed { get; }
        bool SprintPressed { get; }
    }
}
