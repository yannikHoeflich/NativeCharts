namespace NativeCharts.Models;

public record ColorA(byte Red, byte Green, byte Blue, byte Alpha) : Color(Red, Green, Blue) {
    public override string ToString() {
        Span<char> chars = stackalloc char[9];
        chars[0] = '#';

        chars[1] = ToHex(this.Red / 16);
        chars[2] = ToHex(this.Red % 16);

        chars[3] = ToHex(this.Green / 16);
        chars[4] = ToHex(this.Green % 16);

        chars[5] = ToHex(this.Blue / 16);
        chars[6] = ToHex(this.Blue % 16);

        chars[7] = ToHex(Alpha / 16);
        chars[8] = ToHex(Alpha % 16);

        return new string(chars);
    }
}