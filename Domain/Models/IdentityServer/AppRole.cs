using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.IdentityServer
{
    public class AppRole
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
