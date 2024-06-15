using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public byte[] Clave { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
