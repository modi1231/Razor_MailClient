using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor_MailClient.Data
{
    /// <summary>
    /// Definining the base components of a 'folder'.
    /// </summary>
    public class Folder
    {
        public int ID { get; set; }

        public string NAME { get; set; }

        public bool SELECTED { get; set; }
    }
}
