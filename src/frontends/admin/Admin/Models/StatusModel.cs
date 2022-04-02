using System;

namespace Admin.Models
{
    public class StatusModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Uri Uri { get; set; }
    }
}