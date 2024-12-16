using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TinderApp.Models;
using TinderApp.Views;
using TinderApp.Utilidades;
using TinderApp.DTOs;
namespace TinderApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly TinderDB database;

        [ObservableProperty]
        private string? nombre;

        [ObservableProperty]
        private string? contraseña;

     

        public LoginViewModel(TinderDB tinderdb)
        {
            database = tinderdb;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Contraseña))
            {
                await Shell.Current.DisplayAlert("Error", "Por favor ingresa nombre y contraseña.", "OK");
                return;
            }

            try
            {
                // Verificar las credenciales en la base de datos
                var user = await database.VerificarCredencial(Nombre, Contraseña);

                if (user != null)
                {
                    // Asignar el usuario actual a la sesión
                    Session.UsuarioActual =
                    new UsuarioDTO
                    {
                        User_id = user.UsuarioId,
                        Nombre = user.Nombre,
                        Genero = user.Genero,
                        Edad = user.Edad,
                        Ubicacion = user.Ubicacion,
                        Preferencias = user.Preferencias,
                        Foto = user.Foto,
                        Contraseña = user.Contraseña
                    };


                    // Navegar a la página principal
                    await Shell.Current.GoToAsync("MainPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Hubo un problema al iniciar sesión: {ex.Message}", "OK");
            }
        }
    

        [RelayCommand]
        private async Task Registrar()
        {
            // Navegar a la página de registro
            await Shell.Current.GoToAsync("UsuarioPage");
        }
    }
}
