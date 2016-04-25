// --------------------------------------------------------------------------
// <copyright file="SearchLogViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.12">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;
    using Fanex.ConnectionString.Domain;
    using Fanex.ConnectionString.Model;
    using Fanex.Maintenance.Core.Helpers;

    /// <summary>
    /// The search log view model.
    /// </summary>
    public class SearchLogViewModel
    {
        private string _name;
        private string _username;

        public SearchLogViewModel()
        {
            _name = string.Empty;
            _username = string.Empty;
            From = DateTime.Now;
            To = DateTime.Now;
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        public string Username
        {
            get { return _username != null ? _username.Trim() : string.Empty; }
            set { _username = value; }
        }

        [DisplayFormat(DataFormatString = "{0:MMM dd,yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd,yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        public int FilterType { get; set; }

        public PagerList<LogExecSp> LogDatas { get; set; }
    }
}
