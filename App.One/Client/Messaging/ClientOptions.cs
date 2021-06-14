using System;

namespace App.One.Client.Messaging
{
    internal class ClientOptions
    {
        public static string SectionName = "ClientApp";

        public string AppName { get; set; } = "<unknown>";

        public Uri? ServerHub { get; set; }
    }
}
