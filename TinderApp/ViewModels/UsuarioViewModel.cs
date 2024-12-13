using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.Models;
using TinderApp.DTOs;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using TinderApp.Utilidades;

namespace TinderApp.ViewModels
{
    [QueryProperty(nameof(UsuarioId),"idUsuario")]
    public partial class UsuarioViewModel:ObservableObject
    {

        private readonly TinderDB tinderdb;


        [ObservableProperty]
        private UsuarioDTO usuarioDTO;

        [ObservableProperty]
        private string titulo;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private int usuarioId;

        public UsuarioViewModel(TinderDB tinderDB)
        {
            this.tinderdb = tinderDB;
            this.UsuarioDTO=new UsuarioDTO();
            this.titulo = "Nuevo usuario";
            
        }

        [RelayCommand]
        public async Task Guardar()
        {

            if (string.IsNullOrWhiteSpace(UsuarioDTO.Nombre) ||
                UsuarioDTO.Edad <= 0 ||
                string.IsNullOrWhiteSpace(UsuarioDTO.Genero) ||
                string.IsNullOrWhiteSpace(UsuarioDTO.Ubicacion) ||
                string.IsNullOrWhiteSpace(UsuarioDTO.Preferencias) ||
                string.IsNullOrWhiteSpace(UsuarioDTO.Contraseña))
            {
                await Shell.Current.DisplayAlert("Error", "No has introducido los valores correctamente", "OK");
             
            }

            Loading = true;


            bool success;

            Usuario usuario = new Usuario
            {
                Nombre = UsuarioDTO.Nombre,
                Edad = UsuarioDTO.Edad,
                Genero = UsuarioDTO.Genero,
                Ubicacion = UsuarioDTO.Ubicacion,
                Preferencias = UsuarioDTO.Preferencias,
                Foto = UsuarioDTO.Foto,
                Contraseña = UsuarioDTO.Contraseña
            };


            if (UsuarioDTO.User_id == 0)
            {

                //Utilizo el num de inserciones para simular el id, cuando es diferente de 0 se entiende que existe
                int idFalso = await tinderdb.InsertarUsuario(usuario);
                UsuarioDTO.User_id = idFalso;
                success = UsuarioDTO.User_id > 0;

            }
            else
            {
                usuario.UsuarioId = UsuarioId;
                success = await tinderdb.ActualizarUsuario(usuario) > 0;

            }

            if (success)
            {
                WeakReferenceMessenger.Default.Send(new UsuarioMensaje(UsuarioDTO)); //Se envía al MainViewModel que hay un evento que ha sido modificado.

                await Shell.Current.DisplayAlert("Éxito", "Todo bien!", "OK");
                await Shell.Current.GoToAsync(".."); //Vuelve al padre (MainPage)

            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se ha podido guardar nada!", "OK");
            }

            Loading = false;


        }

        partial void OnUsuarioIdChanged(int value)
        {
            if (value != 0)
            {
                CargarUsuario(value);
            }
        }



        public async void CargarUsuario(int value)
        {
            List<Usuario> listado = await tinderdb.VerUsuario();
            if (listado != null)
            {
                Usuario usuarioSeleccionado = listado.Find((e) => e.UsuarioId == UsuarioId);

                UsuarioDTO usuarioDTO = new UsuarioDTO
                {
                    User_id = usuarioSeleccionado.UsuarioId,
                    Nombre = usuarioSeleccionado.Nombre,
                    Edad = usuarioSeleccionado.Edad,
                    Ubicacion = usuarioSeleccionado.Ubicacion,
                    Genero = usuarioSeleccionado.Genero,
                    Preferencias = usuarioSeleccionado.Preferencias,
                    Foto = usuarioSeleccionado.Foto,
                };
                UsuarioDTO = usuarioDTO;
                Titulo = "Editar Usuario";

            }


        }

        [RelayCommand]
        public async Task SeleccionarFoto()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                UsuarioDTO.Foto = result.FullPath; // Ruta completa de la foto seleccionada
            }
        }



    }
}
