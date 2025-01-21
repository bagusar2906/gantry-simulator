namespace Extensions
{
    public static class Conversion
    {
        public static float ToNative(this float orig, float scale, float offset)
        {
            return orig * scale + offset;
        }
        
        public static float ToEng(this float orig, float scale, float offset)
        {
            return (orig - offset)/scale;
        }
    }
}