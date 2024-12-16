using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TinderApp.DTOs;
using TinderApp.Models;
using TinderApp.Utilidades;

namespace TinderApp.ViewModels
{
    public partial class LikeViewModel : ObservableObject, IRecipient<LikeMensaje>
    {
        private readonly TinderDB tinderDB;

        [ObservableProperty]
        private ObservableCollection<LikeDTO> listaLikes;

        [ObservableProperty]
        private LikeDTO likeDTOActual;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        public LikeViewModel(TinderDB tinderDB)
        {
            this.tinderDB = tinderDB;
            ListaLikes = new ObservableCollection<LikeDTO>();
            isRefreshing = false;
            _ = CargarListaLikes();
            WeakReferenceMessenger.Default.Register<LikeMensaje>(this);
        }

        /* LIKES */

        [RelayCommand]
        public async Task CargarListaLikes()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;

            try
            {
                Console.WriteLine("Cargando likes...");

                var likes = await tinderDB.VerLike(0); // Aquí se carga la lista de likes
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ListaLikes.Clear();
                    foreach (var like in likes)
                    {
                        ListaLikes.Add(new LikeDTO
                        {
                            Id_like = like.id_like,
                            Id_user1 = like.id_user1,
                            Id_user2 = like.id_user2,
                            FechaLike = Convert.ToDateTime(like.fechaLike)
                        });
                    }
                });

                Console.WriteLine("Likes cargados con éxito.");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error al cargar los likes: {ex.Message}", "OK");
                Console.WriteLine($"Error al cargar likes: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }


        [RelayCommand]
        public async Task CrearLike(LikeDTO likeDTO)
        {
            if (likeDTO == null)
            {
                return;
            }

            var like = new Like
            {
                id_user1 = likeDTO.Id_user1,
                id_user2 = likeDTO.Id_user2,
                fechaLike = Convert.ToString(likeDTO.FechaLike)
            };

            int resultado = await tinderDB.InsertarLike(like);

            if (resultado > 0)
            {
                await CargarListaLikes();
            }
        }

        [RelayCommand]
        public async Task EliminarLike(LikeDTO likeDTO)
        {
            if (likeDTO == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmación", "¿Estás seguro de eliminar este like?", "Aceptar", "Cancelar");

            if (respuesta)
            {
                ListaLikes.Remove(likeDTO);
                await tinderDB.EliminarLike(likeDTO.Id_like);
            }
        }

        public void Receive(LikeMensaje message)
        {
            MainThread.BeginInvokeOnMainThread(async () => await CargarListaLikes());
        }
    }
}

