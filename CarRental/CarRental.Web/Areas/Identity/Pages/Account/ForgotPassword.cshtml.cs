﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using CarRental.Web.Resources;
using System.Reflection;
using CarRental.Web.ViewRender;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Messages;

namespace CarRental.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IRazorViewToStringRender _render;
        private readonly IStringLocalizer _localizer;

        public ForgotPasswordModel(
            UserManager<User> userManager, 
            IEmailSender emailSender,
            ICloudStorageService cloudStorageService,
            IRazorViewToStringRender render,
            IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _cloudStorageService = cloudStorageService;
            _render = render;

            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "EMAIL_REQUIRED")]
            [EmailAddress(ErrorMessage = "EMAIL_INVALID")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                /*await _emailSender.SendEmailAsync(Input.Email, "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                var model = new EmailConfirmationDto(user.Name ?? user.UserName, HtmlEncoder.Default.Encode(callbackUrl));

                const string view = "/Views/Emails/ForgotPasswordEmail";
                var body = await _render.RenderViewToStringAsync($"{view}Html.cshtml", model);

                QueueEmailMessage queueEmail = new QueueEmailMessage(Input.Email, "", body, "Reset Password");
                await _cloudStorageService.SendMessage(queueEmail);
                
                //await _emailSender.SendEmailAsync(Input.Email, "Reset Password", body);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
