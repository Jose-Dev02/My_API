namespace MiApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Correo { get; set; }
        public byte[] Clave { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
