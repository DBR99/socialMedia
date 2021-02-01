using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaCore.Entities
{

    //una clase abstracta es una clase que no se instancia es solo para heredar 

    //Este es un buen punto para datos de auditoria
    public abstract class BaseEntity
    {
        public int id { get; set;}

    }
}
