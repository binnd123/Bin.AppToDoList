using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bin.AppToDoList.DAL.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DOB { get; set; }
        public bool? Gender { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime? ExpiredRefreshToken { get; set; }
        public Guid? RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
