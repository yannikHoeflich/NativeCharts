using BlazorCharts.Models;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;

namespace BlazorCharts;

public partial class BlazorChart {
    
    protected async Task<double> TextWidth(string text, int size, bool bold = false, bool italic = false) {
        if (_canvas is null) {
            return 0;
        }

        return await CanvasService.MeasureTextWidth(_canvas.Value, text, size, bold, italic);
    }
    
    
    protected async Task RenderPopUp(IContext2DWithoutGetters context, ChartValue value, double x, double y) {
        const int margin = 3;
        const int fontSize = 16;
        const int lineHeight = fontSize + 4;
        const int labelHeight = lineHeight * 2 + margin * 3;
        
        double labelWidth = await TextWidth(value.Label, fontSize, bold: true);

        string valueString = ValueFormatter(value.Value);
        double valueWidth = await TextWidth(valueString, fontSize, italic: true);
        double totalWidth = Math.Max(valueWidth, labelWidth) + margin * 3;

        y += labelHeight / 2.0;
        
        if (x > Width / 2) {
            x -= totalWidth;
        }

        y -= labelHeight;
        await context.Fill(PopUpBackground.WithOpacity(150), async c => { await PopUpPath(x, y, c, margin, totalWidth, labelHeight); });
        await context.Stroke(PopUpBackground, 1, async c => { await PopUpPath(x, y, c, margin, totalWidth, labelHeight); });
        y += labelHeight;
                
        await context.Text(PopUpPrimary, fontSize, async c => {
            await c.FillTextAsync(value.Label, x + margin, y - margin * 4 - lineHeight);
        }, bold: true);
                
        await context.Text(PopUpPrimary, fontSize, async c => {
            await c.FillTextAsync(valueString, x + margin, y - margin * 2);
        }, italic: true);
    }

    private async Task PopUpPath(double x,
        double y,
        IContext2DWithoutGetters context,
        int margin,
        double totalWidth,
        int labelHeight) {
        await context.MoveToAsync(x + margin, y);

        await context.LineToAsync(x + totalWidth - margin, y);
        await context.QuadraticCurveToAsync(x + totalWidth, y, x + totalWidth, y + margin);
        if (x > Width / 2) {
            await context.LineToAsync(x + totalWidth, y + (labelHeight - margin) / 2 - 8);
            await context.LineToAsync(x + totalWidth + 8, y + (labelHeight - margin) / 2);
            await context.LineToAsync(x + totalWidth, y + (labelHeight - margin) / 2 + 8);
        }
        await context.LineToAsync(x + totalWidth, y + labelHeight - margin);
        await context.QuadraticCurveToAsync(x + totalWidth, y + labelHeight, x + totalWidth - margin, y + labelHeight);
        await context.LineToAsync(x + margin, y + labelHeight);
        await context.QuadraticCurveToAsync(x, y + labelHeight, x, y + labelHeight - margin);
        if (x < Width / 2) {
            await context.LineToAsync(x, y + (labelHeight - margin) / 2 + 8);
            await context.LineToAsync(x - 8, y + (labelHeight - margin) / 2);
            await context.LineToAsync(x, y + (labelHeight - margin) / 2 - 8);
        }
        await context.LineToAsync(x, y + margin);
        await context.QuadraticCurveToAsync(x, y, x + margin, y);
    }
}