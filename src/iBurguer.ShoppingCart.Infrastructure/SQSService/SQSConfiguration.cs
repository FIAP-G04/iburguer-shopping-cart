﻿using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.SQSService
{
    [ExcludeFromCodeCoverage]
    public class SQSConfiguration
    {
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Queue { get; set; }
    }
}
