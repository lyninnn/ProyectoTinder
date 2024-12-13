using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.Models;
using TinderApp.DTOs;
using CommunityToolkit.Mvvm.Messaging;
using TinderApp.Utilidades;

namespace TinderApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRecipient<UsuarioMensaje>
    {

        private readonly TinderDB tinderDB;

        [ObservableProperty]
        private ObservableCollection<UsuarioDTO> listaUsuarios;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;



        public MainViewModel(TinderDB tinderDB)
        {
            isRefreshing = false;
            this.tinderDB = tinderDB;
            listaUsuarios = new ObservableCollection<UsuarioDTO>();
            _ = CargarListaUsuarios();
            WeakReferenceMessenger.Default.Register<UsuarioMensaje>(this);

        }


        /* USUARIOS */

        [RelayCommand]
        public async Task CargarListaUsuarios()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;

            List<Usuario> listUsuarios = await tinderDB.VerUsuario();
            MainThread.BeginInvokeOnMainThread(() =>
            {

                ListaUsuarios.Clear();
                foreach (Usuario usuario in listUsuarios)
                {
                    ListaUsuarios.Add(new UsuarioDTO
                    {
                        User_id = usuario.UsuarioId,
                        Nombre = usuario.Nombre,
                        Genero = usuario.Genero,
                        Edad = usuario.Edad,
                        Ubicacion = usuario.Ubicacion,
                        Preferencias = usuario.Preferencias,
                        Foto = usuario.Foto,
                        Contraseña= usuario.Contraseña
                    });
                }

            });

            IsBusy = false;
            IsRefreshing = false;

        }
        public void Receive(UsuarioMensaje message) //Cuando recibe el cambio del evento regarga la lista
        {
            MainThread.BeginInvokeOnMainThread(async () => await CargarListaUsuarios());
        }

        [RelayCommand]
        private async Task CrearUsuario()
        {
            await Shell.Current.GoToAsync("UsuarioPage");
        }

        [RelayCommand]
        private async Task EditarUsuario(UsuarioDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"UsuarioPage?id={usuario.User_id}");

        }

        [RelayCommand]
        private async Task EliminarUsuario(UsuarioDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmacion", "Estas seguro que quieres eliminar el usuario", "Aceptar", "Cancelar");

            if (respuesta)
            {
                ListaUsuarios.Remove(usuario);
                await tinderDB.EliminarUsuario(usuario.User_id);
            }

        }

       

        


    }
}
