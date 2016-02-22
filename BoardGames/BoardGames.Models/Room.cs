namespace BoardGames.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data.Common.Models;

    public class Room : AuditInfo, IDeletableEntity
    {
        private ICollection<User> users;

        public Room()
        {
            this.users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index("Name", IsUnique = true)]
        [MaxLength(16)]
        public string Name { get; set; }

        [Required]
        [DefaultValue(4)]
        public int Capacity { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

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

        public bool IsFull()
        {
            return this.Capacity == this.Users.Count;
        }
    }
}
