using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.DTOs
{
    public partial class UsuarioDTO : ObservableObject
    {

        [ObservableProperty]
        private int user_id;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private int edad;

        [ObservableProperty]
        private string genero;

        [ObservableProperty]
        private string ubicacion;

        [ObservableProperty]
        private string preferencias;

        [ObservableProperty]
        private string foto;

        [ObservableProperty]
        private string contraseña;
    }
}
