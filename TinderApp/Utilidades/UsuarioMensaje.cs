using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.DTOs;

namespace TinderApp.Utilidades
{
    public class UsuarioMensaje
    {
        public UsuarioDTO UsuarioActualizado { get; }

        public UsuarioMensaje(UsuarioDTO usuarioDTO)
        {

            UsuarioActualizado = usuarioDTO;

        }

    }
}
