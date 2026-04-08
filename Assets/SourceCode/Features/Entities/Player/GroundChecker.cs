using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class GroundChecker : MonoBehaviour
    {
        public float groundDistance = 0.6f;
        public LayerMask groundLayer;

        public float rayOffset = 0.4f; // half width of cube

        public bool IsGrounded()
        {
            Vector3[] offsets = new Vector3[]
            {
                Vector3.zero,
                Vector3.left * rayOffset,
                Vector3.right * rayOffset
            };

            foreach (var offset in offsets)
            {
                if (Physics.Raycast(transform.position + offset, Vector3.down, groundDistance, groundLayer))
                {
                    return true;
                }
            }

            return false;
        }
        public bool TryGetGround(out RaycastHit hit)
        {
            Vector3[] offsets = new Vector3[]
            {
                Vector3.zero,
                Vector3.left * rayOffset,
                Vector3.right * rayOffset
            };

            foreach (var offset in offsets)
            {
                if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, groundDistance, groundLayer))
                {
                    return true; // return FIRST valid hit
                }
            }

            hit = default;
            return false;
        }

        bool CheckRay(Vector3 offset)
        {
            return Physics.Raycast(
                transform.position + offset,
                Vector3.down,
                groundDistance,
                groundLayer
            );
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            DrawRay(Vector3.zero);
            DrawRay(Vector3.left * rayOffset);
            DrawRay(Vector3.right * rayOffset);
        }

        void DrawRay(Vector3 offset)
        {
            Vector3 origin = transform.position + offset;
            Gizmos.DrawLine(origin, origin + Vector3.down * groundDistance);
        }

    }
}

