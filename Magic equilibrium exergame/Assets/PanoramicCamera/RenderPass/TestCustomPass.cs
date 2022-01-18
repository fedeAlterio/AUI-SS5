using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class TestCustomPass : CustomPass
{
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private Material _material;
    private ScriptableRenderContext _context;

    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in an performance manner.
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _context = renderContext;
    }

    protected override void Execute(CustomPassContext ctx)
    {
        // Executed every frame for all the camera inside the pass volume.
        // The context contains the command buffer to use to enqueue graphics commands.
        CoreUtils.SetRenderTarget(ctx.cmd, _renderTexture);
        CoreUtils.DrawFullScreen(ctx.cmd, _material, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        // Cleanup code
    }
}