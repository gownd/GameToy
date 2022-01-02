using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSideDetector : MonoBehaviour
{
    public CollisionSide GetCollisionSide(Collision2D other, BoxCollider2D collider)
    {
        float halfColldierSizeX = collider.size.x / 2f;
        float halfColliderSizeY = collider.size.y / 2f;

        Vector2 contactPos = other.GetContact(0).point;
        Vector2 offset = ((Vector2)transform.position + collider.offset) - contactPos;

        if(Mathf.Abs(offset.x) >= halfColldierSizeX)
        {
            if (offset.x < 0f) return CollisionSide.right;
            else if(offset.x > 0f) return CollisionSide.left;
        }
        else if(Mathf.Abs(offset.y) >= halfColliderSizeY)
        {
            if(offset.y < 0f) return CollisionSide.up;
            else if (offset.y > 0f) return CollisionSide.down;
        }

        return CollisionSide.unknown;
    }
}

public enum CollisionSide
{
    up, down, left, right, unknown
}
