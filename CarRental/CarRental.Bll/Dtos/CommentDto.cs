using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int VehicleModelId { get; set; }
    }
}
