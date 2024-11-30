using BlazorCharts.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCharts;

public partial class BlazorChart {
    [Parameter]
    public int Width { get; set; } = 200;
    
    [Parameter]
    public int Height { get; set; } = 200;
    
    [Parameter]
    public Color BackgroundColor { get; set; } = Colors.White;
    
    [Parameter]
    public Color PrimaryColor { get; set; } = Colors.Black;
    
    [Parameter]
    public Color PopUpBackground { get; set; } = Colors.Black;
    
    [Parameter]
    public Color PopUpPrimary { get; set; } = Colors.White;
    
    [Parameter]
    public EventCallback<ChartValue> OnClick { get; set; }

    [Parameter]
    public ValueFormatter ValueFormatter { get; set; } = ValueFormatters.Default;

}