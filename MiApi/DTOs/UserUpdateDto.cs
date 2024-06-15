namespace MiApi.DTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public byte[] Clave { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
