using ApiAltura.IdentityConfiguration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAltura.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly IConfiguration configuration;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Usuario BuscarUsuario(string login)
        {
            var usuarios = new List<Usuario>() { new Usuario { Login = "joao.fonseca", Senha = "123456" } };

            return usuarios.FirstOrDefault(u => u.Login == login);
        }
    }
}
