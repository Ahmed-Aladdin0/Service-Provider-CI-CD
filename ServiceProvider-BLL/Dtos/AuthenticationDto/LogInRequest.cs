﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider_BLL.Dtos.AuthenticationDto
{
    public record LogInRequest(
        string Email,
        string Password
    );
}
