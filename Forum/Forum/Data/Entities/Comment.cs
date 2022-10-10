using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        
        public DateTime CreationTimeUtc
        {
            get
            {
                return this.creationTimeUtc.HasValue
                   ? this.creationTimeUtc.Value
                   : DateTime.Now;
            }

            set { this.creationTimeUtc = value; }
        }
        private DateTime? creationTimeUtc = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
