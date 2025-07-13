using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EXPx2.SpellIndicator
{
    public class SpellIndicatorSample : MonoBehaviour
    {
        private GUIStyle _buttonStyle;

        [SerializeField] private RectangleIndicator _rectangle;
        [SerializeField] private RectangleIndicator _rectangleSpread;
        [SerializeField] private EllipseIndicator _ellipse;
        [SerializeField] private RectangleIndicator _rectangleDecal;
        [SerializeField] private RectangleIndicator _rectangleSpreadDecal;
        [SerializeField] private EllipseIndicator _ellipseDecal;

        [SerializeField] private Transform _player;
        [SerializeField] private Transform _target;

        private bool _hasDecalFeature = false;
        
        private void Start()
        {
            var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (urpAsset == null)
            {
                Debug.LogError("This project is not URP");
                return;
            }
            var rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(urpAsset);
            var defaultRendererIndex = (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(urpAsset);
            var rendererData = rendererDataList[defaultRendererIndex];
            if (rendererData == null)
            {
                Debug.LogError("Failed to find RendererData");
                return;
            }

            // Already added
            _hasDecalFeature = rendererData.rendererFeatures.Any(f => f.GetType().Name.Contains("Decal") && f.isActive);
        }

        private void ShowRectangle(bool isDecal, float width, float height, float duration)
        {
            var indicator = Instantiate(isDecal ? _rectangleDecal : _rectangle);
            indicator.Show(_player.position, _target.position, width, height, duration).OnComplete(() => Destroy(indicator.gameObject));
        }
        
        private void ShowRectangleSpread(bool isDecal, float width, float height, float duration)
        {
            var indicator = Instantiate(isDecal ? _rectangleSpreadDecal : _rectangleSpread);
            indicator.Show(_player.position, _target.position, width, height, duration).OnComplete(() => Destroy(indicator.gameObject));
        }
        
        private void ShowEllipse(bool isDecal, float radius, float holeRadius, float angle, float duration)
        {
            var indicator = Instantiate(isDecal ? _ellipseDecal : _ellipse);
            indicator.Show(_player.position, _target.position, radius, holeRadius, angle, duration).OnComplete(() => Destroy(indicator.gameObject));
        }
        
        private void ShowEllipse(bool isDecal, float radius, float holeRadius, float duration)
        {
            var indicator = Instantiate(isDecal ? _ellipseDecal : _ellipse);
            indicator.Show(_target.position, radius, holeRadius, 360f, duration).OnComplete(() => Destroy(indicator.gameObject));
        }
        private void OnGUI()
        {
            _buttonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 30,
                stretchWidth = true,
                stretchHeight = true,
            };

            GUILayout.BeginArea(new Rect(0, 0, Screen.width / 3, Screen.height * (2 / 3f)));
            if (GUILayout.Button("Rectangle (5, 5)", _buttonStyle))
            {
                ShowRectangle(false, 5, 5, 2f);
            }
            if (GUILayout.Button("Rectangle Spread (3, 8)", _buttonStyle))
            {
                ShowRectangleSpread(false, 3, 8, 2f);
            }
            if (GUILayout.Button("Ellipse (5, 120)", _buttonStyle))
            {
                ShowEllipse(false, 5, 0f, 120f, 2f);
            }
            if (GUILayout.Button("Ellipse (3, 360)", _buttonStyle))
            {
                ShowEllipse(false, 3f, 0f, 2f);
            }
            if (GUILayout.Button("Donut (3, 1, 240)", _buttonStyle))
            {
                ShowEllipse(false, 3f, 1f, 240f, 2f);
            }

            if (_hasDecalFeature)
            {
                if (GUILayout.Button("Rectangle Decal (7, 4)", _buttonStyle))
                {
                    ShowRectangle(true, 7, 4, 2f);
                }

                if (GUILayout.Button("Rectangle Spread Decal (5, 5)", _buttonStyle))
                {
                    ShowRectangleSpread(true, 5, 5, 2f);
                }

                if (GUILayout.Button("Ellipse Decal (4, 240)", _buttonStyle))
                {
                    ShowEllipse(true, 4, 0f, 240f, 2f);
                }

                if (GUILayout.Button("Ellipse Decal (4, 360)", _buttonStyle))
                {
                    ShowEllipse(true, 4f, 0f, 2f);
                }

                if (GUILayout.Button("Donut Decal (5, 3, 60)", _buttonStyle))
                {
                    ShowEllipse(true, 5f, 3f, 60f, 2f);
                }
            }
            else
            {
                GUILayout.Label("Universal Renderer Data does not have Decal Feature", new GUIStyle(GUI.skin.label)
                {
                    fontSize = 35,
                    fontStyle = FontStyle.Bold,
                    margin = new RectOffset(10, 10, 0, 0),
                });
            }

            GUILayout.EndArea();
        }
    }
}
