using ApiAltura.Repositorio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace ApiAltura.IdentityConfiguration
{
    public class Acesso
    {
        private readonly UsuarioRepositorio usuarioRepositorio;
        private readonly SigningConfigurations signingConfigurations;
        private readonly TokenConfigurations tokenConfigurations;

        public Acesso(UsuarioRepositorio usuarioRepositorio,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.signingConfigurations = signingConfigurations;
            this.tokenConfigurations = tokenConfigurations;
        }

        public void ValidarAcesso(Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Login))
                throw new Exception("Objeto de login Inválido");

            var UsuarioBase = usuarioRepositorio.BuscarUsuario(usuario.Login);

            if(UsuarioBase == null || UsuarioBase.Login != usuario.Login || UsuarioBase.Senha == usuario.Senha)
                throw new Exception("Acesso negado");
        }

        public object GerarToken(Usuario usuario)
        {
            ClaimsIdentity Identity = new ClaimsIdentity(
                new GenericIdentity(usuario.Login, "Login"),
                new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login)
                    }
                );

            var dataCriacao = DateTime.Now;
            var dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = Identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            var token = handler.WriteToken(securityToken);

            return new { autenticado = true, criado = dataCriacao, expiracao = dataExpiracao, tokenDeAcesso = token, mensagem = "OK" };
        }
    }
}
