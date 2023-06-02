using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace WMPFluent.Extensions
{
    public static class StringExtensions
    {
        #region -- Data Members --
        static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
        #endregion
        public static string ColorToHexString(this Windows.UI.Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
        public static string GetLocalizedString(this string s)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
            return resourceLoader.GetString(s);
        }
        public static string MakeSafeForFileName(this string text)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(c, '-');
            }
            if (text.EndsWith('.'))
            {
                text = text.Remove(text.Length - 1);
            }
            return text;
        }

        public static bool ContainsAll(this string s, string[] strings, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return strings.ToList().All(x => s.Contains(x, stringComparison));
        }

        public static bool ContainsAny(this string s, string[] strings, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return strings.ToList().Any(x => s.Contains(x, stringComparison));
        }


        public static bool IsSimilar(this string s, string t)
        {
            s = s.ToLower();
            t = t.ToLower();
            if (s == t || s.Contains(t, StringComparison.OrdinalIgnoreCase) || t.Contains(s, StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }
        public static double Compute(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source.ToLower() == target.ToLower()) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source.ToLower(), target.ToLower());
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
        public static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
        internal static FontFamily GetFluentFont()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 11))
                return new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons");
            else
                return new Windows.UI.Xaml.Media.FontFamily("/Assets/Fonts/SegoeIcons.ttf#Segoe Fluent Icons");

        }
        public static string CheckChar(this char v)
        {
            var derp = "";
            if (Char.IsUpper(v))
            {
                derp = v.ToString();
            }
            else if (Char.IsNumber(v))
            {
                var derp2 = "#";
                derp = derp2;
            }
            else
            {
                var derp2 = "&";
                derp = derp2;
            }
            return derp.ToString();
        }
        //public static string RemoveTheThe(this string artist)
        //{
        //    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
        //    string a = resourceLoader.GetString("A");
        //    string fem_a = resourceLoader.GetString("Fem_A");
        //    string masc_a = resourceLoader.GetString("Masc_A");
        //    string an = resourceLoader.GetString("An");
        //    string fem_an = resourceLoader.GetString("Fem_An");
        //    string masc_an = resourceLoader.GetString("Masc_An");
        //    string the = resourceLoader.GetString("The");
        //    string masc_the = resourceLoader.GetString("Masc_The");
        //    string fem_the = resourceLoader.GetString("Fem_The");
        //    string plural_the = resourceLoader.GetString("Plural_The");
        //    string plural_masc_the = resourceLoader.GetString("Plural_Masc_The");
        //    string plural_fem_the = resourceLoader.GetString("Plural_Fem_The");
        //    List<string> articles = new List<string>
        //    {
        //        a,
        //        fem_a,
        //        masc_a,
        //        an,
        //        fem_an,
        //        masc_an,
        //        the,
        //        masc_the,
        //        fem_the,
        //        plural_the,
        //        plural_masc_the,
        //        plural_fem_the
        //    };
        //    articles.ForEach(x => x = x.ToLower());
        //    artist = artist.ToLower();
        //    foreach (string article in articles)
        //    {
        //        if (artist.StartsWith(article.ToLower() + " ") && App.AppSettingViewModel.IgnoreDeterminerWords)
        //        {
        //            //Debug.WriteLine(artist);
        //            artist = artist.Remove(0, (article.ToLower() + " ").Length);
        //            break;
        //        }
        //    }
        //    return artist;
        //    //if (artist.StartsWith("The ", StringComparison.InvariantCultureIgnoreCase) && App.AppSettingViewModel.IgnoreDeterminerWords)
        //    //{
        //    //    return artist.Substring(4);
        //    //}
        //    //else if (artist.StartsWith("A ", StringComparison.InvariantCultureIgnoreCase) && App.AppSettingViewModel.IgnoreDeterminerWords)
        //    //{
        //    //    return artist.Substring(2);
        //    //}
        //    //else
        //    //{
        //    //    return artist;
        //    //}
        //}

        public static string DePunctuate(this string s)
        {
            var sb = new StringBuilder();

            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            s = sb.ToString();
            return s;
        }

        public static string StripForSearch(this string v)
        {
            return v.DeAccent().DePunctuate();
        }
        //public static double Luminance(this ColorThiefDotNet.QuantizedColor color)
        //{
        //    return ((0.2126 * color.Color.R) + (0.7152 * color.Color.G) + (0.0722 * color.Color.B));
        //}
        public static double Luminance(this Windows.UI.Color color)
        {
            return ((0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B));
        }
        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }
        public static Color Lerp(this Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);

            // return the new colour
            return Color.FromArgb(255, r, g, b);
        }
        public static string DeAccent(this string v)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = v.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
        public static bool IsOneOf(this string s, IEnumerable<string> list)
        {
            bool isitem = false;
            foreach (var item in list)
            {
                if (item == s)
                {
                    isitem = true;
                    break;
                }
                else
                {
                    isitem = false;
                }
            }
            return isitem;
        }
    }
}
