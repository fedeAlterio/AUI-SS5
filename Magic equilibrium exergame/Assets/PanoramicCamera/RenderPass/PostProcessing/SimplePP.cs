using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;
using Assets.Scripts.Cameras;

[Serializable, VolumeComponentMenu("Post-processing/Custom/SimplePP")]
public sealed class SimplePP : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    // Private fields
    private PerspectiveHandler _perspectiveHandler;

    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    public RenderTextureParameter renderTexture = new RenderTextureParameter(null);
    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/SimplePP";

    public override void Setup()
    {
        _perspectiveHandler = FindObjectOfType<PerspectiveHandler>();   
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume SimplePP is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;
        m_Material.SetTexture("_CameraATex", renderTexture.value);
        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetTexture("_InputTexture", source);
        m_Material.SetMatrix("_FloorNormalizedToCameraClip", _perspectiveHandler.ComputeNormalizedFloorToCameraClip());
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
