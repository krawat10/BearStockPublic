using System.ComponentModel.DataAnnotations;

namespace BearStock.Authorization.Models
{
    public class UpdateModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}