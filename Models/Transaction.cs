using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User Creator { get; set; }
        public int UserId { get; set; }

    }
}