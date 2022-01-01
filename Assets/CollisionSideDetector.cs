using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSideDetector : MonoBehaviour
{
    public CollisionSide GetCollisionSide(Collision2D other, BoxCollider2D collider)
    {
        Vector2 contactPos = other.GetContact(0).point;
        Vector2 offset = (Vector2)transform.position - contactPos;

        float halfColldierSizeX = collider.size.x / 2f;
        float halfColliderSizeY = collider.size.y / 2f;

        if (Mathf.Abs(offset.x) - Mathf.Abs(offset.y) > 0)
        {
            if (offset.x <= -halfColldierSizeX) return CollisionSide.right;
            else if(offset.x >= halfColldierSizeX) return CollisionSide.left;
        }
        else
        {
            if(offset.y <= -halfColliderSizeY) return CollisionSide.up;
            else if (offset.y >= halfColliderSizeY) return CollisionSide.down;
        }

        return CollisionSide.unknown;
    }
}

public enum CollisionSide
{
    up, down, left, right, unknown
}
