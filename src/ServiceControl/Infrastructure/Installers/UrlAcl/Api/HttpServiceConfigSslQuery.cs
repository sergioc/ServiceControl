﻿// ReSharper disable MemberCanBePrivate.Global
namespace ServiceBus.Management.Infrastructure.Installers.UrlAcl.Api
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct HttpServiceConfigSslQuery
    {
        public HttpServiceConfigQueryType QueryDesc;

        public HttpServiceConfigSslKey KeyDesc;

        public uint Token;
    }
}
