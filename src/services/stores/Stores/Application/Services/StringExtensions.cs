using System.Text.RegularExpressions;

namespace Stores.Application.Services
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string name) 
        { 
            string str = name.ToLower(); 
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); 
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim(); 
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str; 
        }
    }
}