﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.Helpers
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Lifespan { get; set; }
    }
}
