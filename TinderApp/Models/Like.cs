using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.Models
{
    public class Like
    {
            public int id_like {  get; set; }
            public int id_user1 { get; set; } // Usuario que dio el Like
            public int id_user2 { get; set; }  // Usuario que recibió el Like
            public string fechaLike { get; set; }    // Fecha en la que se dio el Like
        

    }
}
