﻿using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewCompanyModel
    {
        public string CompName { get; set; } = null!;

        public string CompFoundationYear { get; set; } = null!;

        public string CompWebSite { get; set; } = null!;

        public string CompContactMail { get; set; } = null!;

        public string CompAdress { get; set; } = null!;

        public string CompAddressTitle { get; set; } = null!;

        public string CompSektor { get; set; } = null!;

        public string CompDesc { get; set; } = null!;

        public string CompLogo { get; set; } = null!;

        public string ComLinkedin { get; set; } = null!;

        public int CompEmployeeCount { get; set; }

        public int CompUserId { get; set; }
        
    }
}
