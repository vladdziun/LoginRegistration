using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class LoginUser
{
    public string LoginEmail {get; set;}
    [DataType(DataType.Password)]
    public string LoginPassword { get; set; }
}