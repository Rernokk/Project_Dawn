using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace jp.gulti.ColorBlind
{

	public class ColorBlindnessSimulator : jp.gulti.PostEffectsBase
	{
		public enum ColorBlindMode
		{
      None,
			Protanope,
			Deuteranope,
      Tritanope,
      Achromatope
		}
		[SerializeField]
		public ColorBlindMode BlindMode = ColorBlindMode.Protanope;

		[SerializeField]
		public float BlindIntensity = 1.0f;

		[SerializeField]
		public Shader ColorBlindShader;
		private Material ColorBlindMat;

		public static readonly string ColorBlindShaderName = "Hidden/GULTI/ColorBlindSimulator";

		#region Overrides
		
		protected override bool CheckResources ()
		{
			CheckSupport(false);
			ColorBlindShader = Shader.Find(ColorBlindShaderName);
			ColorBlindMat = CreateMaterial(ColorBlindShader, ColorBlindMat);
			return ColorBlindMat != null;
		}
		
		#endregion

		#region Monobehavior

		void OnDisable()
		{
			if(ColorBlindMat != null)
			{
#if UNITY_EDITOR
				if(!UnityEditor.EditorApplication.isPlaying)
					DestroyImmediate(ColorBlindMat, true);
				else
#endif
				Destroy(ColorBlindMat);
			}
		}

		void OnRenderImage(RenderTexture _src, RenderTexture _dst)
		{
			if(ColorBlindMat == null)
			{
				if(!CheckResources())
				{
					NotSupported();
					return;
				}
			}

			switch (BlindMode)
			{
				case ColorBlindMode.Protanope:
					ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_ONE" };
					break;
				case ColorBlindMode.Deuteranope:
					ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_TWO" };
					break;
        case ColorBlindMode.Tritanope:
          ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_THREE" };
          break;
        case ColorBlindMode.Achromatope:
          ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_FOUR" };
          break;
        default:
          ColorBlindMat.shaderKeywords = new string[] { "NONE" };
          break;
			}

			//Intensity Set
			ColorBlindMat.SetFloat("_BlindIntensity", BlindIntensity);

			Graphics.Blit(_src, _dst, ColorBlindMat);
		}

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.F1))
      {
        BlindMode = ColorBlindMode.None;
      }
      if (Input.GetKeyDown(KeyCode.F2))
      {
        BlindMode = ColorBlindMode.Protanope;
      }
      if (Input.GetKeyDown(KeyCode.F3))
      {
        BlindMode = ColorBlindMode.Deuteranope;
      }
      if (Input.GetKeyDown(KeyCode.F4))
      {
        BlindMode = ColorBlindMode.Tritanope;
      }
      if (Input.GetKeyDown(KeyCode.F5))
      {
        BlindMode = ColorBlindMode.Achromatope;
      }
    }
    #endregion
  }
}