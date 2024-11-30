namespace NativeCharts.Models;

public record Color(byte Red, byte Green, byte Blue) {
    public override string ToString() {
        Span<char> chars = stackalloc char[7];
        chars[0] = '#';

        chars[1] = ToHex(Red / 16);
        chars[2] = ToHex(Red % 16);

        chars[3] = ToHex(Green / 16);
        chars[4] = ToHex(Green % 16);

        chars[5] = ToHex(Blue / 16);
        chars[6] = ToHex(Blue % 16);

        return new string(chars);
    }

    protected static char ToHex(int value) {
        return value switch {
            0 => '0',
            1 => '1',
            2 => '2',
            3 => '3',
            4 => '4',
            5 => '5',
            6 => '6',
            7 => '7',
            8 => '8',
            9 => '9',
            10 => 'A',
            11 => 'B',
            12 => 'C',
            13 => 'D',
            14 => 'E',
            15 => 'F',
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public Color Highlight(double d) {
        GetHsv(out double hue, out double saturation, out double value);

        saturation *= d;
        if (saturation >= 1) {
            saturation = 1;
        }
        
        value *= d;
        if (value >= 1) {
            value = 1;
        }

        return FromHsv(hue, saturation, value);
    }

    public void GetHsv(out double hue, out double saturation, out double value) {
        double r = Red / 255.0;
        double g = Green / 255.0;
        double b = Blue / 255.0;

        double cMax = Math.Max(r, Math.Max(g, b));
        double cMin = Math.Min(r, Math.Min(g, b));

        double diff = cMax - cMin;
        double h = -1, s = -1;

        if (Math.Abs(cMax - cMin) < 0.001)
            h = 0;

        else if (Math.Abs(cMax - r) < 0.001)
            h = 60 * ((g - b) / diff) + 360 % 360;

        else if (Math.Abs(cMax - g) < 0.001)
            h = 60 * ((b - r) / diff) + 120 % 360;

        else if (Math.Abs(cMax - b) < 0.001) h = 60 * ((r - g) / diff) + 240 % 360;

        if (cMax == 0)
            s = 0;
        else
            s = (diff / cMax);

        if (h < 0) {
            h += 360;
        }
        
        hue = h % 360;
        saturation = s;
        value = cMax;
    }

    public static Color FromHsv(double hue, double saturation, double value) {
        double r = 0, g = 0, b = 0;

        if (saturation == 0)
        {
            r = value;
            g = value;
            b = value;
        }
        else
        {
            int i;
            double f, p, q, t;

            if (Math.Abs(hue - 360) < 0.01)
                hue = 0;
            else
                hue /= 60;

            i = (int)Math.Truncate(hue);
            f = hue - i;
            p = value * (1.0 - saturation);
            q = value * (1.0 - (saturation * f));
            t = value * (1.0 - (saturation * (1.0 - f)));

            switch (i)
            {
                case 0:
                    r = value;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = value;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = value;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = value;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = value;
                    break;
                default:
                    r = value;
                    g = p;
                    b = q;
                    break;
            }
        }

        return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

    }

    public ColorA WithOpacity(byte alpha) => new(Red, Green, Blue, alpha);
}