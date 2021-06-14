﻿using System;

namespace App.One.Server.Messaging
{
    internal class ServerOptions
    {
        public static string SectionName = "ServerApp";

        public string AppName { get; set; } = "<unknown>";

        public Ping Ping { get; set; } = new();
    }

    public class Ping
    {
        public TimeSpan DueTime { get; set; } = TimeSpan.FromSeconds(60);

        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(60);
    }
}