using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.DTOs
{
    public partial class MatchDTO : ObservableObject
    {

        [ObservableProperty]
        private int matchId;

        [ObservableProperty]
        private int usuario1Id;

        [ObservableProperty]
        private int usuario2Id;

        [ObservableProperty]
        private DateTime fechaMatch;

    }
}
