using System;

namespace App.Two.Client.Messaging
{
    internal class ClientOptions
    {
        public static string SectionName = "ClientApp";

        public string AppName { get; set; } = "<unknown>";

        public Uri? ServerHub { get; set; }
    }
}
