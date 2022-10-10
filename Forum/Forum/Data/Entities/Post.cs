using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatioTimeUTC { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
//id, pavadinimas, aprasymas, data(gal), kategorija(foreign key), naudotojas(foreign key)