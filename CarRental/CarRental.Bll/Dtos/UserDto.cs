using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [Display(Name = "NAME")]
        public string Name { get; set; }
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
    }
}
