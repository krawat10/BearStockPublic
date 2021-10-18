﻿namespace BearStock.Authorization.Helpers
{
    public class ClientTokenSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}