namespace Mi.Domain.Helper
{
    public class CaptchaHelper
    {
        public static byte[] New(string id)
        {
            return new byte[0];
        }

        public static bool Validate(string id, string code)
        {
            return true;
        }
    }
}