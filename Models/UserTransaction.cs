using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class UserTransaction
    {
        public User user {get; set;}
        public Transaction transaction {get;set;}
    }
}