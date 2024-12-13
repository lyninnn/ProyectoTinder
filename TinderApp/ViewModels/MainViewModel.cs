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
        private UsuarioDTO usuarioDTOactual;

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

            await Shell.Current.GoToAsync($"UsuarioPage?idUsuario={usuario.User_id}");

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
        [RelayCommand]
        private async Task VerMatches()
        {
            await Shell.Current.GoToAsync("MatchPage");
        }


        [RelayCommand]
        public async Task DarLikeAsync(UsuarioDTO usuarioLikeado)
        {
            await Shell.Current.DisplayAlert("hola","ok","ok");
            if (usuarioLikeado == null)
            {
                await Shell.Current.DisplayAlert("Advertencia", "El usuario seleccionado no es válido.", "OK");
                return;
            }

            try
            {
                int usuarioActualId = UsuarioDTOactual.User_id; // Asume que tienes el ID del usuario actual disponible

                // Verificar si ya se dio un like previamente
                if (await VerificarLike(usuarioActualId, usuarioLikeado.User_id))
                {
                    await Shell.Current.DisplayAlert(
                        "Like ya enviado",
                        $"Ya has dado like a {usuarioLikeado.Nombre}.",
                        "OK"
                    );
                    return;
                }

                // Insertar el "like" en la base de datos
                var nuevoMatch = new Match
                {
                    Usuario1Id = usuarioActualId,
                    Usuario2Id = usuarioLikeado.User_id,
                    FechaMatch = Convert.ToString(DateTime.Now)
                };
                await tinderDB.InsertarMatch(nuevoMatch);

                // Comprobar si existe un match mutuo
                if (await EsMatchMutuo(usuarioActualId, usuarioLikeado.User_id))
                {
                    await Shell.Current.DisplayAlert(
                        "¡Match logrado!",
                        $"¡Tienes un match con {usuarioLikeado.Nombre}! Ahora pueden comenzar a chatear.",
                        "OK"
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "¡Like enviado!",
                        $"Has dado like a {usuarioLikeado.Nombre}.",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Hubo un problema al dar like: {ex.Message}", "OK");
            }
        }


        public async Task<bool> VerificarLike(int usuario1Id, int usuario2Id)
        {
            var likes = await tinderDB.VerMatch(); // Obtén todos los matches de la base de datos
            return likes.Any(m => m.Usuario1Id == usuario1Id && m.Usuario2Id == usuario2Id);
        }
        public async Task<bool> EsMatchMutuo(int usuario1Id, int usuario2Id)
        {
            var matches = await tinderDB.VerMatch(); // Obtén todos los matches de la base de datos
            return matches.Any(m =>
                (m.Usuario1Id == usuario1Id && m.Usuario2Id == usuario2Id) ||
                (m.Usuario2Id == usuario1Id && m.Usuario1Id == usuario2Id)
            );
        }






    }
}
