﻿using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using NativeCharts.Models;

namespace NativeCharts;

public partial class PieChart : BlazorChart {
    
    [Parameter]
    public List<ChartValue>? Values { get; set; }

    private double _hoverAngle = -1;

    protected int _margin = 20;

    protected double Radius => (Math.Min(this.Width, this.Height) - _margin * 2) / 2.0;
    protected double Middle => _margin + Radius;

    protected override async Task Render(IContext2DWithoutGetters context) {
        (double labelX, double labelY, ChartValue? selectedValue) = await RenderPie(context);

        if (selectedValue is null) {
            return;
        }

        await RenderPopUp(context, selectedValue, labelX, labelY);
    }

    protected async Task<(double labelX, double labelY, ChartValue? selectedValue)> RenderPie(IContext2DWithoutGetters context) {
        if (Values is null || Values.Count == 0) {
            return (0, 0, null);
        }

        await context.Fill(this.BackgroundColor, async c => {
            await c.RectAsync(0, 0, this.Width, this.Height);
        });
        ;
        
        double sum = Values.Sum(x => x.Value);

        double labelX = 0;
        double labelY = 0;
        ChartValue? selectedValue = null;
        
        double lastAngle = 0;
        foreach (ChartValue value in Values) {
            double angle = value.Value / sum;

            if (Math.Abs(angle - 1) < 0.0001) {
                angle = 0.9999;
            }
            
            if (lastAngle < _hoverAngle && lastAngle + angle > _hoverAngle) {
                await context.FillPartCircle(_margin, _margin, Radius, lastAngle, lastAngle + angle, value.Color.Highlight(1.4));
                await context.StrokePartCircle(_margin, _margin, Radius, lastAngle, lastAngle + angle, this.BackgroundColor, 2);

                labelX = Middle + Math.Sin((lastAngle + angle / 2) * Math.PI * 2) * (Radius * 2 / 3);
                labelY = Middle - Math.Cos((lastAngle + angle / 2) * Math.PI * 2) * (Radius * 2 / 3);

                selectedValue = value;
            } else {
                await context.FillPartCircle(_margin, _margin, Radius, lastAngle, lastAngle + angle, value.Color);
                await context.StrokePartCircle(_margin, _margin, Radius, lastAngle, lastAngle + angle, this.BackgroundColor, 2);
            }

            lastAngle += angle;
        }

        if (HasLegend) {
            await RenderLegend(context);
        }

        return (labelX, labelY, selectedValue);
    }

    protected async Task RenderLegend(IContext2DWithoutGetters context) {
        double x = _margin;
        double y = Radius * 2 + _margin * 4;

        if (Values is null) {
            return;
        }

        int retries = 0;
        while (retries < 10) {
            try {
                IEnumerable<ColorName> nameColors = Values.Select(x => new ColorName(x.Color, x.Label));

                await base.RenderLegend(context, nameColors, x, y, Width - _margin * 2);
                return;
            } catch(InvalidOperationException) {
                retries++;
            }
        }
    }

    protected override async Task Hover(IContext2DWithoutGetters context, double x, double y) {
        x -= (Radius + _margin);
        y -= (Radius + _margin);
        
        double a = GetAngle(x, y);

        double r = Math.Sqrt(x * x + y * y);

        if (r < Radius) {
            _hoverAngle = a;
            await Render(context);
        } else {
            await HoverEnd(context);
        }
    }

    protected override async Task Click(IContext2DWithoutGetters context, double x, double y) {
        double sum = Values.Sum(x => x.Value);
        double lastAngle = 0;

        ChartValue? clickedValue = null;
        foreach (ChartValue value in Values) {
            if (lastAngle > _hoverAngle) {
                return;
            }
            
            double angle = value.Value / sum;
            
            if (lastAngle + angle > _hoverAngle) {
                clickedValue = value;
                break;
            }

            lastAngle += angle;
        }

        if (clickedValue is null) {
            return;
        }

        await OnClick.InvokeAsync(clickedValue);

    }

    private double GetAngle(double x, double y) {
        double a = double.Atan2(x, y);

        a = -a + Math.PI;
        a /= Math.PI * 2;
        return a;
    }

    protected override async Task HoverEnd(IContext2DWithoutGetters context) {
        _hoverAngle = -1;
        await Render(context);
    }
}