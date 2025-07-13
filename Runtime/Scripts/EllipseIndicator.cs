using UnityEngine;

namespace EXPx2.SpellIndicator
{
    public class EllipseIndicator : SpellIndicator
    {
        private static readonly int Angle = Shader.PropertyToID("_Angle");
        private static readonly int HoleRatio = Shader.PropertyToID("_HoleRatio");
        
        public SpellIndicator Show(Vector3 position, Vector3 target, float radius, float angle, float duration)
        {
            return Show(position, target, radius, 0f, angle, duration);
        }
        
        public SpellIndicator Show(Vector3 position, float radius, float angle, float duration)
        {
            return Show(position, radius, 0f, angle, duration);
        }
        
        public SpellIndicator Show(Vector3 position, Vector3 target, float radius, float holeRadius, float angle, float duration)
        {
            if (!IsDecal)
            {
                position += new Vector3(0f, MeshOffsetY, 0f);
            }
            transform.position = position;
            var rot = Quaternion.LookRotation(target - position).eulerAngles;
            transform.Rotate(Vector3.forward, -rot.y);
            var scale = radius * 2;
            transform.localScale = new Vector3(scale, scale, 1f);
            this.SetProperty(Angle, angle);
            this.SetProperty(HoleRatio, holeRadius / radius);
            Fill(duration);
            return this;
        }
        
        public SpellIndicator Show(Vector3 position, float radius, float holeRadius, float angle, float duration)
        {
            if (!IsDecal)
            {
                position += new Vector3(0f, MeshOffsetY, 0f);
            }
            transform.position = position;
            // transform.rotation = Quaternion.LookRotation(Vector3.down);
            var scale = radius * 2;
            transform.localScale = new Vector3(scale, scale, 1f);
            this.SetProperty(Angle, angle);
            this.SetProperty(HoleRatio, holeRadius / radius);
            Fill(duration);
            return this;
        }
    }
}
