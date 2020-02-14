using System;

namespace Mathy.Shared.Entity
{
    public class User
    {
        public int UserID { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public string Company { get; set; }

        public string CellPhone { get; set; }

        public string TelPhone { get; set; }

        public DateTime? EnableDate { get; set; }

        public int DeleteFlag { get; set; }

        public string Role { get; set; }
    }
}
