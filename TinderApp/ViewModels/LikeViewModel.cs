using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TinderApp.DTOs;
using TinderApp.Models;
using TinderApp.Utilidades;

namespace TinderApp.ViewModels
{
    public partial class LikeViewModel : ObservableObject
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
            listaLikes = new ObservableCollection<LikeDTO>();
            isRefreshing = false;
            _ = CargarListaLikes();
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

            var likes = await tinderDB.VerLike(0); // Cambiar "0" si quieres filtrar por usuario o ID específico
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ListaLikes.Clear();
                foreach (var like in likes)
                {
                    ListaLikes.Add(new LikeDTO
                    {
                        Id_like = like.id_like,
                        Id_user1 = like.id_user1,
                        id_user2 = like.id_user2,
                        FechaLike = Convert.ToDateTime(like.fechaLike)
                    });
                }
            });

            IsBusy = false;
            IsRefreshing = false;
        }

        //[RelayCommand]
        //public async Task CrearLike(LikeDTO likeDTO)
        //{
        //    if (likeDTO == null)
        //    {
        //        return;
        //    }

        //    var like = new Like
        //    {
        //        id_user1 = likeDTO.Id_user1,
        //        id_user2 = likeDTO.Id_user2,
        //        fechaLike = Convert.ToString(likeDTO.FechaLike)
        //    };

        //    int resultado = await tinderDB.InsertarLike(like);

        //    if (resultado > 0)
        //    {
        //        await CargarListaLikes();
        //    }
        //}

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

    }
}

