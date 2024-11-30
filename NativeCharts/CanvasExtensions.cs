using System.Text;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using NativeCharts.Models;

namespace NativeCharts;

public static class CanvasExtensions {
    
    public static async Task DrawCircle(this IContext2DWithoutGetters context, double x, double y, double radius, Color color) {
        await context.Fill(color, async c => await c.ArcAsync(x, y, radius, 0, Math.PI * 2));

    }
    
    public static async Task FillPartCircle(this IContext2DWithoutGetters context, double x, double y, double radius, double startAngle, double endAngle , Color color) {
        // y += radius;
        
        startAngle *= 2 * Math.PI;
        endAngle *= 2 * Math.PI;

        startAngle %= 2 * Math.PI;
        endAngle %= 2 * Math.PI;
        
        double middleX = x + radius;
        double middleY = y + radius;

        
        await context.Fill(color, async c => {
            await c.EllipseAsync(middleX, middleY, radius, radius, -Math.PI / 2, startAngle, endAngle);
            await c.LineToAsync(middleX, middleY);
        });
    }

    public static async Task StrokePartCircle(this IContext2DWithoutGetters context,
        double x,
        double y,
        double radius,
        double startAngle,
        double endAngle,
        Color color,
        int width) {
        // y += radius;
        
        startAngle *= 2 * Math.PI;
        endAngle *= 2 * Math.PI;

        startAngle %= 2 * Math.PI;
        endAngle %= 2 * Math.PI;
        
        double middleX = x + radius;
        double middleY = y + radius;


        double startX = middleX + Math.Sin(startAngle) * radius;
        double startY = middleY - Math.Cos(startAngle) * radius;
        
        await context.Stroke(color, width, async c => {
            await c.EllipseAsync(middleX, middleY, radius, radius, -Math.PI / 2, startAngle, endAngle);
            await c.LineToAsync(middleX, middleY);
            await c.LineToAsync(startX, startY);
        });
    }
    public static async Task Fill(this IContext2DWithoutGetters context, Color color, AsyncAction<IContext2DWithoutGetters> action) {
        await context.BeginPathAsync();
        await context.FillStyleAsync(color.ToString());
        await action(context);
        await context.FillAsync(FillRule.NonZero);
    }

    public static async Task Stroke(this IContext2DWithoutGetters context, Color color, int width, AsyncAction<IContext2DWithoutGetters> action) {
        await context.BeginPathAsync();
        await context.LineWidthAsync(width);
        await context.StrokeStyleAsync(color.ToString());
        await action(context);
        await context.StrokeAsync();
    }

    public static async Task Text(this IContext2DWithoutGetters context, Color color, int size, AsyncAction<IContext2DWithoutGetters> action, bool bold = false, bool italic = false) {
        var textStyle = new StringBuilder();
        if (bold) {
            textStyle.Append("bold ");
        }
        if (italic) {
            textStyle.Append("italic ");
        }
        textStyle.Append(size);
        textStyle.Append("px");
        textStyle.Append(" sans-serif");
        await context.TextStyles.FontAsync(textStyle.ToString());
        await context.FillStyleAsync(color.ToString());
        await action(context);
        // await context.FillAsync(FillRule.NonZero);
    }
}