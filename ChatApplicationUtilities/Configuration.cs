using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class Configuration
    {
        public static string FIFO_FOLDER = Path.Combine(Path.GetTempPath(), "fifo");
        public static string BROADCAST_CHANNELNAME = "#all";
        public static string BROADCAST_CHANNEL = Path.Combine(FIFO_FOLDER, BROADCAST_CHANNELNAME);

        private static readonly string validFilenameRegex = @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$";

        public static bool IsValidFilename(string filename)
        {
            // Max length for filename for windows is 260 characters: http://stackoverflow.com/a/3406521/2428827
            return !string.IsNullOrEmpty(filename) && filename.Length < 260 && (Regex.IsMatch(filename, validFilenameRegex, RegexOptions.CultureInvariant));
        }

        public static bool UsernameIsTaken(string username)
        {
            string path = Path.Combine(Configuration.FIFO_FOLDER, username);
            return !File.Exists(path);
        }

        public static void SendMessageToUser(string recipient, string message)
        {
            string fullPathname = Path.Combine(FIFO_FOLDER, recipient);
            File.AppendAllLines(
                fullPathname,
                new string[] { message },
                Encoding.ASCII);
        }

    }
}
