﻿using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using NativeCharts.Models;

namespace NativeCharts;

public class DonutChart: PieChart {
    [Parameter]
    public double InnerRadius { get; set; } = 20;
    
    
    protected override async Task Render(IContext2DWithoutGetters context) {
        (double labelX, double labelY, ChartValue? selectedValue) = await RenderPie(context);

        await context.Fill(this.BackgroundColor, async c => {
            await c.EllipseAsync(this.Middle, this.Middle, InnerRadius, InnerRadius, 0, 0, 2 * Math.PI);
        });
        
        
        if (selectedValue is null) {
            return;
        }

        await RenderPopUp(context, selectedValue, labelX, labelY);
    }

    protected override async Task Hover(IContext2DWithoutGetters context, double x, double y) {
        double lx = x - (this.Radius + this._margin);
        double ly = y - (this.Radius + this._margin);

        double r = Math.Sqrt(lx * lx + ly * ly);

        if (r < InnerRadius) {
            await HoverEnd(context);
            return;
        }
        
        await base.Hover(context, x, y);
    }
}