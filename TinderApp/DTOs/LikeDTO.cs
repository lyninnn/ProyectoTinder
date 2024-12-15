using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.DTOs
{
    public partial class LikeDTO:ObservableObject
    {
        [ObservableProperty]
        public int id_like;

        [ObservableProperty]
        public int id_user1;

        [ObservableProperty]
        public int id_user2;

        [ObservableProperty]
        public DateTime fechaLike;


    }
}
