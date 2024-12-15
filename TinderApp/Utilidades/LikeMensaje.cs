using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.DTOs;

namespace TinderApp.Utilidades
{
    public class LikeMensaje
    {
        public LikeDTO LikeActualizado { get; }

        public LikeMensaje(LikeDTO likeDTO)
        {

            LikeActualizado = likeDTO;

        }
    }
}
