using System;
using System.IO;

namespace SinoptikWPF
{
    internal static class GlobalSettings
    {
        public static string URL { get; set; } = "https://sinoptik.ua/%D0%BF%D0%BE%D0%B3%D0%BE%D0%B4%D0%B0-%D0%BA%D0%B0%D0%BC%D0%B5%D0%BD%D1%81%D0%BA%D0%BE%D0%B5-303007130";

        public static string FilesFolder { get; set; } = Path.Combine(Environment.CurrentDirectory, "Files");

        public static int Hours { get; set; } = 8;
    }
}
