﻿using FluentValidation;
using SocialMediaCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaInfraestructure.Validators
{
    public class PostValidator : AbstractValidator<PostDTO> 
    {
        public PostValidator() {
            RuleFor(post => post.Description)
                    .NotNull()
                    .Length(10, 500);
            RuleFor(post => post.Date)
                  .NotNull()
                  .LessThan(DateTime.Now);
        }

    }
}
