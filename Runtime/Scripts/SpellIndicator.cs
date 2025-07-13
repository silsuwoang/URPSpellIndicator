using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EXPx2.SpellIndicator
{
    public delegate void IndicatorCallback();
    
    public class SpellIndicator : MonoBehaviour
    {
        protected const float MeshOffsetY = 0.001f;
        private static readonly int Progress = Shader.PropertyToID("_Progress");

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private DecalProjector _decalProjector;
        
        public IndicatorCallback Completed;
        private Coroutine _fillRoutine;
        
        public Material Material { get; private set; }
        protected bool IsDecal { get; private set; }
        
        private void Awake()
        {
            IsDecal = _meshRenderer == null;
            if (IsDecal)
            {
                Material = new Material(_decalProjector.material);
                _decalProjector.material = Material;
            }
            else
            {
                Material = _meshRenderer.material;
            }
        }
        
        protected void Fill(float duration)
        {
            if (_fillRoutine != null)
            {
                StopCoroutine(_fillRoutine);
            }
            _fillRoutine = StartCoroutine(FillRoutine(duration));
        }
        
        private IEnumerator FillRoutine(float time)
        {
            var current = 0f;
            while (current < time)
            {
                current += Time.deltaTime;
                this.SetProperty(Progress, current / time);
                yield return null;
            }

            this.SetProperty(Progress, 1f);
            Completed?.Invoke();
            _fillRoutine = null;
        }
    }

    public static class SpellIndicatorExtensions
    {
        public static T OnComplete<T>(this T t, IndicatorCallback completed) where T : SpellIndicator
        {
            t.Completed = completed;
            return t;
        }
        
        public static T SetProperty<T>(this T t, int property, float value) where T : SpellIndicator
        {
            t.Material.SetFloat(property, value);
            return t;
        }
        
        public static T SetProperty<T>(this T t, int property, Color value) where T : SpellIndicator
        {
            t.Material.SetColor(property, value);
            return t;
        }
    }
}
