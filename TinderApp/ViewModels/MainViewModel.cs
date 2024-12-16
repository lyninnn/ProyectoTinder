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
            if (Session.UsuarioActual != null)
            {
                usuarioDTOactual = Session.UsuarioActual;
            }
          

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
        public async Task DarLike(UsuarioDTO usuarioLikeado)
        {
            if (usuarioLikeado == null)
            {
                await Shell.Current.DisplayAlert("Advertencia", "El usuario seleccionado no es válido.", "OK");
                return;
            }

            if (UsuarioDTOactual == null)
            {
                await Shell.Current.DisplayAlert("Error", "El usuario actual no está definido.", "OK");
                return;
            }

            try
            {
                int usuarioActualId = UsuarioDTOactual.User_id; // ID del usuario actual, ahora seguro que no es null

                // Verificar si ya se dio un like previamente
                bool likePrevio = await tinderDB.ExisteLikeReciproco(usuarioActualId, usuarioLikeado.User_id);
                if (!likePrevio)
                {
                    await Shell.Current.DisplayAlert(
                        "Like ya enviado",
                        $"Ya has dado like a {usuarioLikeado.Nombre}.",
                        "OK"
                    );
                    ListaUsuarios.Remove(usuarioLikeado);
                        var nuevoLike = new Like
                        {
                            id_user1 = usuarioActualId,
                            id_user2 = usuarioLikeado.User_id,
                            fechaLike = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        await tinderDB.InsertarLike(nuevoLike);
                    return;
                }
                else{
                        // Crear un registro de Match
                        Match nuevoMatch = new Match
                        {

                            Usuario1Id = usuarioActualId,
                            Usuario2Id = usuarioLikeado.User_id,
                            FechaMatch = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        await tinderDB.InsertarMatch(nuevoMatch);

                        await Shell.Current.DisplayAlert(
                            "¡Match logrado!",
                            $"¡Tienes un match con {usuarioLikeado.Nombre}! Ahora pueden comenzar a chatear.",
                            "OK"
                        );
                    }
                        MainThread.BeginInvokeOnMainThread(() => ListaUsuarios.Remove(usuarioLikeado));
                    }catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Hubo un problema al dar like: {ex.Message}", "OK");
            }
        }


        [RelayCommand]
        private async Task VerLikes()
        {
            await Shell.Current.GoToAsync("LikePage");
        }

        [RelayCommand]
        public async Task VerificarLikeReciproco(LikeDTO likeDTO)
        {
            if (likeDTO == null)
            {
                return;
            }

            bool existeReciproco = await tinderDB.ExisteLikeReciproco(likeDTO.Id_user1, likeDTO.Id_user2);

            if (existeReciproco)
            {
                await Shell.Current.DisplayAlert("¡Match!", "Existe un Like recíproco.", "Aceptar");
            }
            else
            {
                await Shell.Current.DisplayAlert("Sin Match", "No existe un Like recíproco.", "Aceptar");
            }
        }





    }
}
