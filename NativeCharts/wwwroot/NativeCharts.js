window.measure_text_length = (canvas, text, style) => {
    const context = canvas.getContext("2d");
    context.font = style;
    const metrics = context.measureText(text);
    return metrics.width;
}