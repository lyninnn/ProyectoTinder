using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TinderApp.Models;

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
                // Validación básica: asegurarse de que ambos campos no estén vacíos.
                await Shell.Current.DisplayAlert("Error", "Por favor ingresa nombre y contraseña.", "OK");
                return;
            }

            try
            {
                // Verificar las credenciales en la base de datos
                var user = await database.VerificarCredencial(Nombre, Contraseña);

                if (user != null)
                {
                  

                    // Navegar a la página de usuario (o la página principal)
                    await Shell.Current.GoToAsync("MainPage");
                }
                else
                {
                    // Si no se encuentra el usuario
                    await Shell.Current.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: captura cualquier error y muestra un mensaje al usuario
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
