using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public class VideoEvent
    {
        public Guid RequestId { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Singer { get; set; }
        public string UrlMp4 { get; set; }
        public string UrlImage { get; set; }
        public bool IsActive { get; set; }
        public string Tags { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
