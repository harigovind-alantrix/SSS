using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class GroundChecker : MonoBehaviour
    {
        // public float groundDistance = 0.6f;
        // public LayerMask groundLayer;

        // public float rayOffset = 0.4f; // half width of cube

        // public bool IsGrounded()
        // {
        //     Vector3[] offsets = new Vector3[]
        //     {
        //         Vector3.zero,
        //         Vector3.left * rayOffset,
        //         Vector3.right * rayOffset
        //     };

        //     foreach (var offset in offsets)
        //     {
        //         if (Physics.Raycast(transform.position + offset, Vector3.down, groundDistance, groundLayer))
        //         {
        //             return true;
        //         }
        //     }

        //     return false;
        // }
        // public bool TryGetGround(out RaycastHit hit)
        // {
        //     Vector3[] offsets = new Vector3[]
        //     {
        //         Vector3.zero,
        //         Vector3.left * rayOffset,
        //         Vector3.right * rayOffset
        //     };

        //     foreach (var offset in offsets)
        //     {
        //         if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, groundDistance, groundLayer))
        //         {
        //             return true; // return FIRST valid hit
        //         }
        //     }

        //     hit = default;
        //     return false;
        // }

        // bool CheckRay(Vector3 offset)
        // {
        //     return Physics.Raycast(
        //         transform.position + offset,
        //         Vector3.down,
        //         groundDistance,
        //         groundLayer
        //     );
        // }

        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;

        //     DrawRay(Vector3.zero);
        //     DrawRay(Vector3.left * rayOffset);
        //     DrawRay(Vector3.right * rayOffset);
        // }

        // void DrawRay(Vector3 offset)
        // {
        //     Vector3 origin = transform.position + offset;
        //     Gizmos.DrawLine(origin, origin + Vector3.down * groundDistance);
        // }
        public float groundedDistance = 0.25f;
        public float landingDistance = 1.2f;
        public LayerMask groundLayer;
        public float rayOffset = 0.4f;

        Vector3[] GetOffsets()
        {
            return new Vector3[]
            {
                Vector3.zero,
                Vector3.left * rayOffset,
                Vector3.right * rayOffset
            };
        }

        public bool IsGrounded()
        {
            foreach (var offset in GetOffsets())
            {
                Vector3 origin = transform.position + Vector3.down * 0.45f + offset;

                if (Physics.Raycast(origin, Vector3.down, groundedDistance, groundLayer))
                    return true;
            }

            return false;
        }

        public bool TryGetGround(out RaycastHit bestHit)
        {
            bool found = false;
            float highestY = float.MinValue;
            bestHit = default;

            foreach (var offset in GetOffsets())
            {
                if (Physics.Raycast(transform.position + offset, Vector3.down, out RaycastHit hit, landingDistance, groundLayer))
                {
                    if (hit.point.y > highestY)
                    {
                        highestY = hit.point.y;
                        bestHit = hit;
                        found = true;
                    }
                }
            }

            return found;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (var offset in GetOffsets())
            {
                Vector3 origin = transform.position + offset;
                Gizmos.DrawLine(origin, origin + Vector3.down * landingDistance);
            }
        }
    }
}

