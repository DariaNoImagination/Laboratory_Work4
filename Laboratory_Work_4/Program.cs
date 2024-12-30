using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace Лаба3
{
    public interface IReactable
    {
        void ReactToAction(PlayerAction action); /// Метод, который вызывается для того, чтобы объект отреагировал на действие игрока.
    }
}


    
