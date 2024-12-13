using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderApp.DTOs;

namespace TinderApp.Utilidades
{
    public class MatchMensaje  
    {
        public MatchDTO MatchActualizado { get; }

        public MatchMensaje(MatchDTO matchsDTO)
        {

            MatchActualizado = matchsDTO;

        }

    }
}
