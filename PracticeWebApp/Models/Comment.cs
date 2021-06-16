using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }      
        public string Text { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int? RepliedCommentId { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        public virtual Comment RepliedComment { get; set; }
        public virtual ICollection<Comment> RepliedComments { get; set; }

    }
}
