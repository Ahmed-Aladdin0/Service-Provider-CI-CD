﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider_BLL.Dtos.ReviewDto
{
    public record UpdateReviewRequest(
        string UserId,
        int Rating,
        string? Comment
        );

}
