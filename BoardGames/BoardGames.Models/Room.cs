namespace BoardGames.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Room
    {
        private ICollection<User> users;

        public Room()
        {
            this.users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DefaultValue(4)]
        public int Capacity { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<User> Users
        {
            get
            {
                return this.users;
            }
            set
            {
                this.users = value;
            }
        }
    }
}
