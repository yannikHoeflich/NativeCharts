namespace NativeCharts;

public delegate string ValueFormatter(double value);

public static class ValueFormatters {
    public static readonly ValueFormatter Default = value => value.ToString("0.00");
    public static readonly ValueFormatter Currency = value => value.ToString("C");
    public static readonly ValueFormatter Integer = value => value.ToString("0");
}