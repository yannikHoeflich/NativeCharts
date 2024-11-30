namespace BlazorCharts.Models;

public record ColorA(byte Red, byte Green, byte Blue, byte Alpha) : Color(Red, Green, Blue) {
    public override string ToString() {
        Span<char> chars = stackalloc char[9];
        chars[0] = '#';

        chars[1] = ToHex(Red / 16);
        chars[2] = ToHex(Red % 16);

        chars[3] = ToHex(Green / 16);
        chars[4] = ToHex(Green % 16);

        chars[5] = ToHex(Blue / 16);
        chars[6] = ToHex(Blue % 16);

        chars[7] = ToHex(Alpha / 16);
        chars[8] = ToHex(Alpha % 16);

        return new string(chars);
    }
}