// --------------------------------------------------------------------------
// <copyright file="MailSettingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.12.6">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Criteria.MailService
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Common;
    using Fanex.MailDeliver.Model;
    using Fanex.Maintenance.Core;

    /// <summary>
    /// The mail setting view model.
    /// </summary>
    public class MailSettingViewModel
    {
        public MailSettingViewModel()
        {
        }

        public MailSettingViewModel(MailSetting mailSetting)
        {
            this.SettingName = mailSetting.SettingName;
            this.SettingId = mailSetting.SettingID;
            this.Server = mailSetting.Server;
            this.Port = mailSetting.Port;
            this.Username = mailSetting.Username;
            this.Password = !string.IsNullOrWhiteSpace(mailSetting.Password) ? Cryptography.CfsEnCrypt(mailSetting.Password) : string.Empty;
            this.IsAuth = mailSetting.IsAuth;
        }

        public int SettingId { get; set; }

        [Display(Name = "Setting_Name", ResourceType = typeof(ViewResource))]
        [StringLength(100, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string SettingName { get; set; }

        [Required]
        [Display(Name = "Server", ResourceType = typeof(ViewResource))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string Server { get; set; }

        [Required]
        [Display(Name = "Port", ResourceType = typeof(ViewResource))]
        public int Port { get; set; }

        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewResource))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(ViewResource))]
        public string Password { get; set; }

        [Display(Name = "IsAuth", ResourceType = typeof(ViewResource))]
        public bool IsAuth { get; set; }

        /// <summary>
        /// Get mail setting.
        /// </summary>
        /// <returns>Mail setting.</returns>
        public MailSetting GetMailSetting()
        {
            MailSetting mailSetting = new MailSetting
            {
                IsAuth = this.IsAuth,
                Password = this.Password,
                Port = this.Port,
                Server = this.Server,
                SettingID = this.SettingId,
                SettingName = this.SettingName,
                Username = this.Username
            };

            return mailSetting;
        }
    }
}