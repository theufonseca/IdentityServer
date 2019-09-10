using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAltura.IdentityConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAltura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Acesso acesso;

        public LoginController(Acesso Acesso)
        {
            acesso = Acesso;
        }

        [AllowAnonymous]
        [HttpPost("/Autenticar")]
        public object Post([FromBody]Usuario usuario)
        {
            acesso.ValidarAcesso(usuario);

            return acesso.GerarToken(usuario);
        }
    }
}