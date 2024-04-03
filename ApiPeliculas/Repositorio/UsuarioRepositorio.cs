using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private readonly string secretKey;

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration config)
        {
            secretKey = config.GetSection("ApiSettings").GetSection("Secreta").ToString();
            _db = db;
        }

        public Usuario GetUsuario(int id)
        {
            return _db.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _db.Usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool UsuarioUnico(string usuario)
        {
            var usuariobd = _db.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario);

            if (usuariobd is null)
                return true;           

            return false;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = Obtenermd5(usuarioRegistroDto.Password);

            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role,
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            usuario.Password = passwordEncriptado;

            return usuario;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = Obtenermd5(usuarioLoginDto.Password);

            var usuario = _db.Usuarios.FirstOrDefault(
                u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                && u.Password == passwordEncriptado
                );

            if (usuario is null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsuario));
            claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario,
            };

            return usuarioLoginRespuestaDto;
        }

        public string Obtenermd5(string pass)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(pass);
            data = x.ComputeHash(data);
            string result = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i].ToString("x2").ToLower();
            }

            return result;
        }
    }
}
