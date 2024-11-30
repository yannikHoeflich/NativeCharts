using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCharts;

public class CanvasService {
    private readonly IJSRuntime _js;

    public CanvasService(IJSRuntime jsRuntime) {
        _js = jsRuntime;
    }

    public async Task<double> MeasureTextWidth(ElementReference canvas,
        string text,
        int size,
        bool bold = false,
        bool italic = false) {
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

        try {
            return await _js.InvokeAsync<double>("window.measure_text_length", canvas, text, textStyle.ToString());
        } catch (JSDisconnectedException) {
            return 0;
        }
    }
}