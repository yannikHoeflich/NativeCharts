using Excubo.Blazor.Canvas.Contexts;
using NativeCharts.Models;

namespace NativeCharts;

public partial class BlazorChart {
    protected const int Margin = 3;
    protected const int FontSize = 16;
    protected const int LineHeight = FontSize + 4;
    
    protected async Task<double> TextWidth(string text, int size, bool bold = false, bool italic = false) {
        if (_canvas is null) {
            return 0;
        }

        return await CanvasService.MeasureTextWidth(_canvas.Value, text, size, bold, italic);
    }
    
    
    protected async Task RenderPopUp(IContext2DWithoutGetters context, ChartValue value, double x, double y) {
        const int labelHeight = LineHeight * 2 + Margin * 3;
        
        double labelWidth = await TextWidth(value.Label, FontSize, bold: true);

        string valueString = ValueFormatter(value.Value);
        double valueWidth = await TextWidth(valueString, FontSize, italic: true);
        double totalWidth = Math.Max(valueWidth, labelWidth) + Margin * 3;

        y += labelHeight / 2.0;
        
        if (x > Width / 2) {
            x -= totalWidth;
        }

        y -= labelHeight;
        await context.Fill(PopUpBackground.WithOpacity(150), async c => { await PopUpPath(x, y, c, Margin, totalWidth, labelHeight); });
        await context.Stroke(PopUpBackground, 1, async c => { await PopUpPath(x, y, c, Margin, totalWidth, labelHeight); });
        y += labelHeight;
                
        await context.Text(PopUpPrimary, FontSize, async c => {
            await c.FillTextAsync(value.Label, x + Margin, y - Margin * 4 - LineHeight);
        }, bold: true);
                
        await context.Text(PopUpPrimary, FontSize, async c => {
            await c.FillTextAsync(valueString, x + Margin, y - Margin * 2);
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

    internal async Task RenderLegend(IContext2DWithoutGetters context, IEnumerable<ColorName> colorNames, double x, double y, double width) {
        double startX = x;
        double startY = y;

        double blockSize = FontSize / 4.0 * 3;
        foreach (ColorName value in colorNames) {
            double nameWidth = await TextWidth(value.Name, FontSize);

            if (x + nameWidth + Margin + blockSize > startX + width) {
                y += LineHeight * 2;
                x = startX;
            }
            
            await context.Fill(value.Color, async c => {
                await c.RectAsync(x + 1, y + 1, blockSize - 2, blockSize - 2);
            });

            await context.Stroke(PrimaryColor, 2, async c => {
                await c.RectAsync(x, y, blockSize, blockSize);
            });

            x += blockSize + Margin;

            await context.Text(PrimaryColor, FontSize, async c => {
                await c.FillTextAsync(value.Name, x, y + blockSize);
            });

            x += nameWidth + Margin * 15;
        }
    }
}