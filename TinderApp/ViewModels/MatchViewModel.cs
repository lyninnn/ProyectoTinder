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
using TinderApp.Utilidades;

namespace TinderApp.ViewModels
{
    [QueryProperty(nameof(MatchId),"idMatch")]
    public partial class MatchViewModel : ObservableObject, IRecipient<MatchMensaje>
    {
        private readonly TinderDB tinderDB;

        [ObservableProperty]
        private ObservableCollection<MatchDTO> listaMatchs;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private int matchId;

        public MatchViewModel(TinderDB tinderDB)
        {
            isRefreshing = false;
            this.tinderDB = tinderDB;

            listaMatchs = new ObservableCollection<MatchDTO>();

            _ = CargarListaMatch();

        }

        /* MATCHS */

        [RelayCommand]
        public async Task CargarListaMatch()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            IsRefreshing = true;

            List<Match> listaMatch = await tinderDB.VerMatch();
            MainThread.BeginInvokeOnMainThread(() =>
            {

                ListaMatchs.Clear();
                foreach (Match match in listaMatch)
                {
                    ListaMatchs.Add(new MatchDTO
                    {
                        MatchId = match.MatchId,
                        Usuario1Id = match.Usuario1Id,
                        Usuario2Id = match.Usuario2Id,
                        FechaMatch = Convert.ToDateTime(match.FechaMatch)

                    });
                }

            });

            IsBusy = false;
            IsRefreshing = false;

        }
        public void Receive(MatchMensaje message) //Cuando recibe el cambio del evento regarga la lista
        {
            MainThread.BeginInvokeOnMainThread(async () => await CargarListaMatch());
        }

        [RelayCommand]
        private async Task CrearMatch()
        {
            await Shell.Current.GoToAsync("MatchPage");
        }

        //[RelayCommand]
        //private async Task EditarMath(MatchDTO match)
        //{
        //    if (match == null)
        //    {
        //        return;
        //    }

        //    await Shell.Current.GoToAsync($"MatchPage?id={match.MatchId}");

        //}




        [RelayCommand]
        private async Task EliminarMatch(MatchDTO match)
        {
            if (match == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmacion", "Estas seguro que quieres eliminar el Match", "Aceptar", "Cancelar");

            if (respuesta)
            {
                ListaMatchs.Remove(match);
                await tinderDB.EliminarMatch(match.MatchId);
            }

        }


    }
}
