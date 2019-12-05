using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class UserEditDto
    {
        public int Id { get; set; }
        [Display(Name = "NAME")]
        public string Name { get; set; }
        [Display(Name = "USERNAME")]
        public string UserName { get; set; }
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Display(Name = "ROLE")]
        public ICollection<string> Roles { get; set; }
        [Display(Name = "ROLE")]
        public string RoleName { get; set; }
    }
}
