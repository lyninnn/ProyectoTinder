using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.DTOs;
using TinderApp.Models;

namespace TinderApp.Utilidades
{
    public static class Session
    {
        public static UsuarioDTO UsuarioActual { get; set; }
    }
}
