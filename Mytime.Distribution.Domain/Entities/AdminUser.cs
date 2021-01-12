using System;
using System.ComponentModel.DataAnnotations;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    public class AdminUser : AggregateRoot
    {
        public AdminUser()
        {
        }

        public AdminUser(string name,
                         string nickName,
                         string pwd,
                         bool isAdmin,
                         EmployeeRole role,
                         string tel)
        {
            Name = name;
            NickName = nickName;
            Pwd = pwd;
            IsAdmin = isAdmin;
            Role = role;
            Tel = tel;
            Createat = DateTime.Now;
        }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string NickName { get; set; }
        [MaxLength(512)]
        public string Pwd { get; set; }
        public bool IsAdmin { get; set; }
        public EmployeeRole Role { get; set; }
        [Required]
        [MaxLength(32)]
        public string Tel { get; set; }
        public DateTime Createat { get; set; }
    }
}