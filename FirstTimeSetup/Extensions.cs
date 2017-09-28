using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using DiscordBot.API;
using System.Web;
using System.Security.Cryptography;

namespace DiscordBot
{
    public static class Extensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string GetArgumentName(IReadOnlyList<CommandArgument> e)
        {
            foreach(CommandArgument v in e)
            {
                return v.Name;
            }
            return "";
        }

        public static string GetLoginLink(string email, string AuthKey)
        {
            String passwd = Hash(email + "." + GetTimestamp(DateTime.Now) + "." + AuthKey);

            return "https://lyhmehosting.com/billing/dologin.php?email=" + email + "&timestamp=" + GetTimestamp(DateTime.Now) + "&hash=" + passwd;
        }

        public static string GetTimestamp(this DateTime value)
        {
            //var timeUtc = DateTime.UtcNow;
            //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return unixTimestamp.ToString();
        }

        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
