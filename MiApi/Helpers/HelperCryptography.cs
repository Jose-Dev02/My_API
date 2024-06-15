using System.Text;
using XSystem.Security.Cryptography;

namespace MiApi.Helpers
{
    public class HelperCryptography
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = string.Empty;
            for (int i = 0; i < 64; i++) 
            {
                int numero = random.Next(0,255);
                char letra = (char)numero;
                salt += letra;
            }
            return salt;
        }

        public static bool CompareArrays(byte[] a, byte[] b)
        {
           var iguales = true;

            if(a.Length != b.Length) return iguales = false;
            
            for(int i = 0;i < a.Length;i++)
            {
                if (a[i].Equals(b[i]) == false) return iguales = false;
            }

            return iguales;
        }

        public static byte[] EncryptarPwd(string password, string salt)
        {
            string contenido = password + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);

            for (int i = 1; i <= 107; i++)
            {
                salida = sha.ComputeHash(salida);
            }

            sha.Clear();
            return salida;
        }
    }
}
