using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class GroundChecker
    {
        private readonly HashSet<Collider> _touchingColliders = new();

        private readonly HashSet<string> _jumpableTags = new()
        {
            "Ground",
            "Platform",
            "Clone"
        };

        public bool IsGrounded() => _touchingColliders.Count > 0;

        public void OnCollisionEnter(Collision collision)
        {
            if (_jumpableTags.Contains(collision.gameObject.tag))
                _touchingColliders.Add(collision.collider);
        }

        public void OnCollisionExit(Collision collision)
        {
            _touchingColliders.Remove(collision.collider);
        }
    }
}