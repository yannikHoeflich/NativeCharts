using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace NativeCharts;

public abstract partial class BlazorChart : ComponentBase {
    [Inject]
    public IJSRuntime Js { get; set; }
    
    [Inject]
    public CanvasService CanvasService { get; set; }
    
    protected Context2D? _context;

    private ElementReference? _canvas;

    private bool _mouseOver = false;

    protected abstract Task Render(IContext2DWithoutGetters context);
    protected abstract Task Hover(IContext2DWithoutGetters context, double x, double y);
    protected abstract Task Click(IContext2DWithoutGetters context, double x, double y);
    protected abstract Task HoverEnd(IContext2DWithoutGetters context);
    
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender) {
            await RenderWrapper();
            return;
        }

        if (_canvas is null) {
            return;
        }
        
        _context = await Js.GetContext2DAsync(_canvas.Value);

        await using Batch2D? batch = _context.CreateBatch();
        await Render(batch);
    }

    private async Task RenderWrapper() {
        if (_context is null) {
            return;
        }
        await using Batch2D? batch = _context.CreateBatch();
        await Render(batch);
    }

    private async Task HoverEnd() {
        if (_context is null) {
            return;
        }

        _mouseOver = false;
        
        await using Batch2D? batch = _context.CreateBatch();
        await HoverEnd(batch);
    }
    
    private async Task MouseMove(MouseEventArgs args) {
        if (_context is null) {
            return;
        }
        
        await using Batch2D? batch = _context.CreateBatch();
        await Hover(batch, args.OffsetX, args.OffsetY);
    }
    
    private async Task Click(MouseEventArgs args) {
        if (_context is null) {
            return;
        }
        
        await using Batch2D? batch = _context.CreateBatch();
        await Click(batch, args.OffsetX, args.OffsetY);
    }
}