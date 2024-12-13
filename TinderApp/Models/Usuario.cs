using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.Models
{
    public class Usuario
    {

        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public int Edad { get; set; }
        public string? Genero { get; set; }
        public string? Ubicacion { get; set; }
        public string? Preferencias { get; set; }
        public string Foto { get; set; }
        public string Contraseña { get; set; }


    }
}
