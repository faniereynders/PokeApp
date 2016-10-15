using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.Api.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl
        {
            get
            {
                return $"http://photos4.meetupstatic.com/photos/member/5/0/thumb_{Id}.jpeg";
            }
        }
    }
}
