//using Gabriel.Cat.S.Seguretat;
using Gabriel.Cat.S.Utilitats;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa
{
    internal interface IServidorKeyUser
    {
        bool ContainsUser(IdUnico idUser);
        //Key GetKey(IdUnico idUser);
    }
}
