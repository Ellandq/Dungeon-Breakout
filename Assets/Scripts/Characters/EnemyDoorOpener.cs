using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnemyDoorOpener : MonoBehaviour
    {
        private void Reset()
        {
            var boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsInDoorLayer(other.gameObject))
            {
                SetChildSpriteAlpha(other.transform, 0f);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsInDoorLayer(other.gameObject))
            {
                SetChildSpriteAlpha(other.transform, 1f);
            }
        }

        private bool IsInDoorLayer(GameObject obj)
        {
            return obj.layer == LayerMask.NameToLayer("Door");
        }

        private static void SetChildSpriteAlpha(Transform parent, float alpha)
        {
            var spriteRenderer = parent.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null) return;
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}