using UnityEngine;

namespace EXPx2.SpellIndicator
{
    public class RectangleIndicator : SpellIndicator
    {
        public SpellIndicator Show(Vector3 position, Vector3 target, float width, float height, float duration)
        {
            var dir = target - position;
            dir.y = 0f;
            position += dir.normalized * (height * 0.5f);
            if (!IsDecal)
            {
                position += new Vector3(0f, MeshOffsetY, 0f);
            }
            transform.position = position;
            var rot = Quaternion.LookRotation(dir).eulerAngles;
            transform.Rotate(Vector3.forward, -rot.y);
            transform.localScale = new Vector3(width, height, 1f);
            Fill(duration);
            return this;
        }
    }
}
