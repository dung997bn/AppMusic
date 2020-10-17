using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Business
{
    public class Video
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string UrlMp4 { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Tags { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
